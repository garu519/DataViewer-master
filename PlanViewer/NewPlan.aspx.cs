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
        public static int planID=-1;//пустая строка в GV
        public static int first = 0;
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
                try
                {
                    //очищать бд Plan от записей с PlanID==-1 и всплывающие окна ---> TODO

                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE [Plan] WHERE PlanId=" + -1, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    gvbind();
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
                GridView1.Rows[0].Cells[0].Text = "Для создания плана напишите его название и добавьте строки в таблицу";

            }                
            
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.Caption = PlanName.Text;
            GridView1.EditIndex = e.NewEditIndex;
            gvbind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView1.Caption = PlanName.Text;
            GridViewRow row = (GridViewRow)GridView1.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("ID");
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete FROM [Plan] where ID=" + Convert.ToInt32(lbldeleteid.Text), conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            //if (GridView1.Rows.Count > 1)
                gvbind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.Caption = PlanName.Text;
            GridView1.EditIndex = -1;
            gvbind();
        }

        

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView1.Caption = PlanName.Text;
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
          //  if (planID == -1) 
          //  planID = factId;
          SqlCommand cmd = new SqlCommand("update [Plan] set Object=N'" + factObject.Text
                + "', WorkType=N'" + worktype.Text
                + "', CostName=N'" + costname.Text
                + "', UnitName=N'" + unitname.Text
                + "', Labor=N'" + labor.Text
                + "', Materials=N'" + materials.Text
                + "', Mechanisms=N'" + mechanisms.Text
                + "', PlanID=" + planID
                + ", Status=" + 1
                + ", Name=N'" + PlanName.Text
                + "' where ID=" + factId, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
           
            gvbind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView1.Caption = PlanName.Text;
            if (e.CommandName.Equals("Insert"))
            {
                if (first == 0)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select ID, Object, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from [Plan] where PlanID=" + planID, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Close();

                    ds.Tables[0].Rows.Clear();
                    GridView1.DataSource = ds;
                    GridView1.DataBind();

                }
                GridViewRow row = GridView1.FooterRow;
                Label lblID = (Label)row.FindControl("ID");
                TextBox factObject = (TextBox)row.FindControl("FactObject");
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

                    if (planID==-1)
                    {
                        Plan plan = new Plan { Contractor = id,Object = factObject.Text, CostName = costname.Text, WorkType = worktype.Text, UnitName = unitname.Text, Labor = labor.Text, Materials = materials.Text, Mechanisms = mechanisms.Text, Customer = int.Parse(DropDownList1.SelectedValue), Status = 0, PlanID = -1, Name=PlanName.Text };
                        db.Plans.InsertOnSubmit(plan);
                        db.SubmitChanges();
                        var query =
                        from pl in db.Plans
                        where pl.PlanID==-1
                        select pl;
                        if (query != null)
                        {
                            planID = query.ToArray()[0].ID;
                        }
                        /*
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("update [Plan] set Contractor="+ id +", Object="+ factObject.Text +", CostName="+ costname.Text +", WorkType="+ worktype.Text + ", UnitName="+ unitname.Text+", Labor=" + labor.Text + ", Materials=" + materials.Text + ", Mechanisms="+mechanisms.Text +", PlanID=" + planID + ", Status=" + 1 + " where PlanID=" + -1, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        */
                        planID = planID + 1;
                        Plan plans = new Plan { Contractor = id, Object = factObject.Text, CostName = costname.Text, WorkType = worktype.Text, UnitName = unitname.Text, Labor = labor.Text, Materials = materials.Text, Mechanisms = mechanisms.Text, Customer = int.Parse(DropDownList1.SelectedValue), Status = 1, PlanID = planID, Name = PlanName.Text };
                        db.Plans.InsertOnSubmit(plans);
                        db.SubmitChanges();
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("DELETE [Plan] WHERE PlanID=" + -1, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    else
                    {
                        Plan plans = new Plan { Contractor = id, Object = factObject.Text, CostName = costname.Text, WorkType = worktype.Text, UnitName = unitname.Text, Labor = labor.Text, Materials = materials.Text, Mechanisms = mechanisms.Text, Customer = int.Parse(DropDownList1.SelectedValue), Status = 1, PlanID = planID, Name = PlanName.Text };
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

        protected void Finish_Click(object sender, EventArgs e)
        {

            Alert.Show("План успешно отправлен!");
           // Response.Redirect(Request.RawUrl);
           conn.Open();
           SqlCommand cmd = new SqlCommand("Select ID, Object, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from [Plan] where PlanID=" + planID, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
          
            ds.Tables[0].Rows.Clear();
            ds.Tables[0].Clear();

            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            GridView1.DataSource = ds;
            GridView1.DataBind();
            int columncount = GridView1.Rows[0].Cells.Count;
            GridView1.Rows[0].Cells.Clear();
            GridView1.Rows[0].Cells.Add(new TableCell());
            GridView1.Rows[0].Cells[0].ColumnSpan = columncount;
            GridView1.Rows[0].Cells[0].Text = "Для создания плана напишите его название и добавьте строки в таблицу";
            planID = -1;
        }

        protected void PlanName_TextChanged(object sender, EventArgs e)
        {
            GridView1.Caption = "";
            PlanName.Text = "Введите название плана";

        }
    }
}