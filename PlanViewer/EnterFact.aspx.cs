using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PlanViewer.Models;

namespace PlanViewer
{
    public partial class CreatePlan1 : System.Web.UI.Page
    {
        string user;
        protected int id;
        protected string providerstring;
        int planID;
        private static string connectionStr = WebConfigurationManager.ConnectionStrings["TeamProjectDBConnectionString1"].ConnectionString;
        private SqlConnection conn = new SqlConnection(connectionStr);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                user = Membership.GetUser().UserName;
            }
            catch (Exception ex)
            {
                Alert.Show("Нет прав доступа, пожалуйста зайдите как Подрядчик");
                Response.Redirect("Account/Login.aspx");
            }
            bool hasAccess = false;
            foreach (var role in Roles.GetRolesForUser())
            {
                if (role.Equals(Global.contractorRole)) hasAccess = true;
            }
            if (!hasAccess)
            {
                Alert.Show("Нет прав доступа, пожалуйста зайдите как Подрядчик");
                Response.Redirect("Account/Login.aspx");
            }
            var db = new DBClassesDataContext();
            var query =
                from contractor in db.Contractors
                where contractor.Email.Equals(user)
                select contractor;
            if (query != null)
            {
                id = query.ToArray()[0].ID;                
            }
            providerstring = "SELECT Customer.Name, [Plan].PlanID FROM Contractor INNER JOIN [Plan] ON Contractor.ID = [Plan].Contractor INNER JOIN Customer ON [Plan].Customer = Customer.ID where Contractor.ID=10 GROUP BY [Plan].PlanID , Customer.Name";
            SqlDataSource1.SelectCommand = string.Format(providerstring);
            DataBind();
            //Session["UserID"] = id;   
            //DropDownList1.DataBind();
            try
            {
                if (int.Parse(DropDownList1.SelectedValue) > 0)
                    buildPlanTable();
            }
            catch
            {
            }
            if (!Page.IsPostBack)
            {
                gvbind();    
            }
        }
        private void buildPlanTable()
        {
            for (int i = 1; i < Table1.Rows.Count; i++)
            {
                Table1.Rows.RemoveAt(i);
            }
            var db = new DBClassesDataContext();
            var query =
                from plan in db.Plans
                where plan.PlanID == int.Parse(DropDownList1.SelectedValue)
                select plan;
            Plan[] results = query.ToArray<Plan>();
            foreach (Plan item in results)
            {

                TableRow tr = new TableRow();
                List<TableCell> cells = new List<TableCell>();
                TableCell c = new TableCell();
                c.Text = item.ID + "";
                cells.Add(c);
                c = new TableCell();
                c.Text = item.Object;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.WorkType;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.CostName;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.UnitName;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.Labor;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.Materials;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.Mechanisms;
                cells.Add(c);
                c = new TableCell();
                c.Text = item.Status+"";
                //c.Enabled = true;
                cells.Add(c);
                foreach (TableCell cell in cells)
                {
                    tr.Cells.Add(cell);
                }
                Table1.Rows.Add(tr);
                planID = int.Parse(DropDownList1.SelectedValue);                
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            buildPlanTable();
            GridView1.EditIndex = -1;
            gvbind();
        }
        protected void gvbind()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select ID, FactObject, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from Fact where FactID=" + planID, conn);            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            if (ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                if (Table1.Rows.Count > 1)
                {
                    var db = new DBClassesDataContext();
                    var query =
                        from plan in db.Plans
                        where plan.PlanID == planID
                        select plan;
                    Plan[] plans = query.ToArray();
                    List<Fact> facts = new List<Fact>();
                    for (int i = 0; i < plans.Length; i++)
                    {
                        Fact f = new Fact { FactID = planID, CostName = plans[i].CostName, FactObject = plans[i].Object, Labor = plans[i].Labor, Materials = plans[i].Materials, Mechanisms = plans[i].Mechanisms, UnitName = plans[i].UnitName, WorkType = plans[i].WorkType, Status = 1 };
                        facts.Add(f);                        
                    }
                    db.Facts.InsertAllOnSubmit<Fact>(facts);
                    try
                    {
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.StackTrace);
                    }
                    conn.Open();
                    cmd = new SqlCommand("Select ID, FactObject, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from Fact where FactID=" + planID, conn);
                    da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                    }
                    else 
                    {
                        ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                        GridView1.DataSource = ds;
                        GridView1.DataBind();
                        int columncount = GridView1.Rows[0].Cells.Count;
                        GridView1.Rows[0].Cells.Clear();
                        GridView1.Rows[0].Cells.Add(new TableCell());
                        GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
                        GridView1.Rows[0].Cells[0].Text = "Ошибка";
                    }
                }                
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("ID");
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM Fact where ID=" + Convert.ToInt32(lbldeleteid.Text), conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;           
            gvbind();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {            
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("ID");
            TextBox factObject = (TextBox)row.FindControl("FactObject");
            TextBox worktype = (TextBox)row.FindControl("WorkType");
            TextBox costname = (TextBox)row.FindControl("CostName");
            TextBox unitname = (TextBox)row.FindControl("UnitName");
            TextBox labor = (TextBox)row.FindControl("Labor");
            TextBox materials = (TextBox)row.FindControl("Materials");
            TextBox mechanisms = (TextBox)row.FindControl("Mechanisms");

            int factId = int.Parse(lblID.Text);            
            GridView1.EditIndex = -1;
            conn.Open();
            SqlCommand cmd = new SqlCommand("update Fact set FactObject='" + factObject.Text
                + "', WorkType='" + worktype.Text
                + "', CostName='" + costname.Text
                + "', UnitName='" + unitname.Text
                + "', Labor='" + labor.Text
                + "', Materials='" + materials.Text
                + "', Mechanisms='" + mechanisms.Text
                + "', FactID=" + planID
                + ", Status=" + 1
                + " where ID=" + factId, conn);            
            cmd.ExecuteNonQuery();
            conn.Close();            
            gvbind();

            //GridView1.DataBind();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            gvbind();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            gvbind();
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            buildPlanTable();
            gvbind();
        }

        protected void approve_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("update Fact set Status=2 where FactID=" + planID, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Alert.Show("Значения сохранены");
            Response.Redirect("CreatePlan.aspx");
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM Fact where FactID=" + planID, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            Response.Redirect("CreatePlan.aspx");
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }        
    }
}