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
    public partial class NewPlan : System.Web.UI.Page
    {
        string user;
        int id;
        int counter;
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
            if (!Page.IsPostBack)
            {
                counter = 0;
                try
                {
                    Plan plan = new Plan { Contractor = id, CostName = "", WorkType = "", UnitName = "", Labor = "", Materials = "", Mechanisms = "", Status = 0, PlanID=-1 };
                    db.Plans.InsertOnSubmit(plan);
                    db.SubmitChanges();
                    var queryy =
                    from pl in db.Plans
                    where pl.Status == 0
                    select pl;
                    if (queryy != null)
                    {
                        planID = queryy.ToArray()[0].ID;
                    }
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update [Plan] set PlanID=" + planID + ", Status=" + 0 + " where Status=0", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //  gvbind();
                    //GridView1.EditIndex = 0;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.StackTrace);
                }

            }
        }
        protected void gvbind()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select ID, Object, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from [Plan] where PlanID=" + planID, conn);
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
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            gvbind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("ID");
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM Plan where ID=" + Convert.ToInt32(lbldeleteid.Text), conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (GridView1.Rows.Count > 1)
                gvbind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            gvbind();
        }

        

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("ID");
            TextBox factObject = (TextBox)row.FindControl("Object");
            TextBox worktype = (TextBox)row.FindControl("WorkType");
            TextBox costname = (TextBox)row.FindControl("CostName");
            TextBox unitname = (TextBox)row.FindControl("UnitName");
            TextBox labor = (TextBox)row.FindControl("Labor");
            TextBox materials = (TextBox)row.FindControl("Materials");
            TextBox mechanisms = (TextBox)row.FindControl("Mechanisms");

            int factId = int.Parse(lblID.Text);
            GridView1.EditIndex = -1;
            conn.Open();
            SqlCommand cmd = new SqlCommand("update [Plan] set Object='" + factObject.Text
                + "', WorkType='" + worktype.Text
                + "', CostName='" + costname.Text
                + "', UnitName='" + unitname.Text
                + "', Labor='" + labor.Text
                + "', Materials='" + materials.Text
                + "', Mechanisms='" + mechanisms.Text
                + "', PlanID=" + planID
                + ", Status=" + 1
                + " where ID=" + factId, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            gvbind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                GridViewRow row = GridView1.FooterRow;
                Label lblID = (Label)row.FindControl("ID");
                TextBox factObject = (TextBox)row.FindControl("Object");
                TextBox worktype = (TextBox)row.FindControl("WorkType");
                TextBox costname = (TextBox)row.FindControl("CostName");
                TextBox unitname = (TextBox)row.FindControl("UnitName");
                TextBox labor = (TextBox)row.FindControl("Labor");
                TextBox materials = (TextBox)row.FindControl("Materials");
                TextBox mechanisms = (TextBox)row.FindControl("Mechanisms");                
                GridView1.EditIndex = -1;
                var db = new DBClassesDataContext();

                try
                {

                    if (counter == 0)
                    {
                        Plan plan = new Plan { Contractor = id, CostName = costname.Text, WorkType = worktype.Text, UnitName = unitname.Text, Labor = labor.Text, Materials = materials.Text, Mechanisms = mechanisms.Text, Customer = int.Parse(DropDownList1.SelectedValue), Status = 0, PlanID = -1 };
                        db.Plans.InsertOnSubmit(plan);
                        db.SubmitChanges();
                        var query =
                        from pl in db.Plans
                        where pl.Status == 0
                        select pl;
                        if (query != null)
                        {
                            planID = query.ToArray()[0].ID;
                        }
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("update [Plan] set PlanID=" + planID + ", Status=" + 1 + " where Status=0", conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        counter++;
                    }
                    else
                    {
                        Plan plans = new Plan { Contractor = id, CostName = costname.Text, WorkType = worktype.Text, UnitName = unitname.Text, Labor = labor.Text, Materials = materials.Text, Mechanisms = mechanisms.Text, Customer = int.Parse(DropDownList1.SelectedValue), Status = 1, PlanID = planID };
                        db.Plans.InsertOnSubmit(plans);
                        db.SubmitChanges();
                    }
                    gvbind();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.StackTrace);
                }
            }
        }
    }
}