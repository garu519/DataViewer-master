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
            //Session["UserID"] = id;   
            //DropDownList1.DataBind();
            if (!Page.IsPostBack)
            {

                try
                {
                    if (int.Parse(DropDownList1.SelectedValue) > 0)
                        buildPlanTable();
                }
                catch
                {
                }
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
                gvbind();
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {            
            buildPlanTable();
        }
        protected void gvbind()
        {
            conn.Open();
            //SqlCommand cmd = new SqlCommand("Select ID, FactObject, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from Fact where FactID=" + planID, conn);
            SqlCommand cmd = new SqlCommand("Select ID, FactObject from Fact where FactID=" + planID, conn);
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
                    cmd = new SqlCommand("Select FactID, FactObject, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from Fact where FactID=" + planID, conn);
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
            Label lbldeleteid = (Label)row.FindControl("lblID");
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM detail where id='" + Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString()) + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();

        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            //GridViewRow row = (GridViewRow)GridView1.Rows[e.NewEditIndex];
            //TextBox tmp;
            //foreach (TableCell cell in row.Cells)
            //{
            //    try {
            //        tmp = (TextBox)cell.Controls[0];
            //        tmp.AutoPostBack = true;
            //    }
            //    catch{}
            //}
            gvbind();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //GridView1.DataBind();
            //int userid = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("ID");
            TextBox factObject = (TextBox)row.FindControl("FactObject");
            //TextBox txtname=(TextBox)gr.cell[].control[];
            //TextBox textName = (TextBox)row.Cells[0].Controls[0];
            //TextBox textadd = (TextBox)row.Cells[1].Controls[0];
            //TextBox textc = (TextBox)row.Cells[2].Controls[0];
            //TextBox textadd = (TextBox)row.FindControl("txtadd");
            //TextBox textc = (TextBox)row.FindControl("txtc");

            int factId = int.Parse(lblID.Text);
            //TextBox[] tb = new TextBox[8];
            //for (int i = 1; i < 2; i++)
            //{
            //    tb[i] = (TextBox)row.Cells[i].Controls[0];
            //    //tb[i].AutoPostBack = true;
            //    //tb[i].DataBind();
            
            //}            
            //GridView1.DataBind();
            
            conn.Open();                      
            //SqlCommand cmd = new SqlCommand("update Fact set FactObject='" + tb[1].Text 
            //    + "', WorkType='" + tb[2].Text
            //    + "', CostName='" + tb[3].Text
            //    + "', UnitName='" + tb[4].Text
            //    + "', Labor='" + tb[5].Text
            //    + "', Materials='" + tb[6].Text
            //    + "', Mechanisms='" + tb[7].Text
            //    + "', FactID=" + planID
            //    + ", Status=" + 1
            //    + " where ID=" + factId, conn);
            SqlCommand cmd = new SqlCommand("update Fact set FactObject='" + factObject.Text + "' where ID=" + factId, conn);
            cmd.ExecuteScalar();
            conn.Close();
            GridView1.EditIndex = -1;
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
        }        
    }
}