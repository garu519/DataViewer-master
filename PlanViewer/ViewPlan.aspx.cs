using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using PlanViewer.Models;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
//using Excel = Microsoft.Office.Interop.Excel;
namespace PlanViewer
{
    public partial class ViewPlan : System.Web.UI.Page
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
                Alert.Show("Нет прав доступа, пожалуйста зайдите как Заказчик");
                Response.Redirect("Account/Login.aspx");
            }
            bool hasAccess = false;
            foreach (var role in Roles.GetRolesForUser())
            {
                if (role.Equals(Global.customerRole)) hasAccess = true;
            }
            if (!hasAccess)
            {
                Alert.Show("Нет прав доступа, пожалуйста зайдите как Заказчик");
                Response.Redirect("Account/Login.aspx");
            }
            var db = new DBClassesDataContext();
            var query =
                from customer in db.Customers
                where customer.Email.Equals(user)
                select customer;
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
                //if (int.Parse(DropDownList1.SelectedValue) > 0)
                    //buildPlanTable();
            }
            catch
            {
            }
            if (!Page.IsPostBack)
            {
                //gvbind();
            }
        }
        [WebMethod(EnableSession = true)]
          public static object StudentList(int jtStartIndex, int jtPageSize, string jtSorting)
          {
         EntityPlanManagerRepository.EF_PlanRepository rep = new EntityPlanManagerRepository.EF_PlanRepository();
              try
              {
                  //Get data from database
                  int planCount = rep.GetAllPlans().ToArray().Length;
                  List<Plan> plans = rep.GetAllPlans();

                  //Return result to jTable
                  return new { Result = "OK", Records = plans, TotalRecordCount = planCount };
              }
              catch (Exception ex)
              {
                  return new { Result = "ERROR", Message = ex.Message };
              }
          }
        
        [WebMethod(EnableSession = true)]
        public static object DeletePlan(int PlanId)
        {
            EntityPlanManagerRepository.EF_PlanRepository rep = new EntityPlanManagerRepository.EF_PlanRepository();
            try
            {
                rep.DeletePlan(PlanId);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        [WebMethod(EnableSession = true)]
        public static object CreatePlan(Plan record)
        {
            EntityPlanManagerRepository.EF_PlanRepository rep = new EntityPlanManagerRepository.EF_PlanRepository();
            try
            {
                rep.CreateNewPlan(record);
                return new { Result = "OK", Record = record };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //buildPlanTable();
            GridView1.EditIndex = -1;
            gvbind();
        }

        [WebMethod(EnableSession = true)]
        public static object UpdatePlan(Plan record)
        {
            EntityPlanManagerRepository.EF_PlanRepository rep = new EntityPlanManagerRepository.EF_PlanRepository();
            try
            {
                rep.UpdatePlan(record);
                return new { Result = "OK" };
            }
            catch (Exception ex)
            {
                return new { Result = "ERROR", Message = ex.Message };
            }
        }
        
        //private void buildPlanTable()
        //{
        //    for (int i = 1; i < Table1.Rows.Count; i++)
        //    {
        //        Table1.Rows.RemoveAt(i);
        //    }
        //    var db = new DBClassesDataContext();
        //    var query =
        //        from plan in db.Plans
        //        where plan.PlanID == int.Parse(DropDownList1.SelectedValue)
        //        select plan;
        //    Plan[] results = query.ToArray<Plan>();
            
        //    foreach (Plan item in results)
        //    {

        //        TableRow tr = new TableRow();
        //        List<TableCell> cells = new List<TableCell>();
        //        TableCell c = new TableCell();
        //        c.Text = item.ID + "";
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.Object;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.WorkType;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.CostName;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.UnitName;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.Labor;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.Materials;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.Mechanisms;
        //        cells.Add(c);
        //        c = new TableCell();
        //        c.Text = item.Status + "";
        //        //c.Enabled = true;
        //        cells.Add(c);
        //        foreach (TableCell cell in cells)
        //        {
        //            tr.Cells.Add(cell);
        //        }
        //        Table1.Rows.Add(tr);
        //        planID = int.Parse(DropDownList1.SelectedValue);
        //    }
        //}

        protected void gvbind()
        {
            conn.Open();
            SqlCommand cmdd = new SqlCommand("Select ID, Object, WorkType, UnitName, CostName, Labor, Materials, Mechanisms from [Plan] where PlanID=" + planID, conn);
            SqlDataAdapter dda = new SqlDataAdapter(cmdd);
            DataSet dds = new DataSet();
            dda.Fill(dds);
            conn.Close();
            if (dds.Tables[0].Rows.Count > 0)
            {
                GridView2.DataSource = dds;
                GridView2.DataBind();
            }
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
                if (planID > 0)
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

        
        protected void PlansDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            
        }        

        protected void sendComment(object sender, EventArgs e)
        {
            
        }

        //метод отправки email
        public void sendEmail(string email, string subject, string text)
        {
            SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 25); //формируем письмо
            Smtp.Credentials = new NetworkCredential("abiturhse", "hseguest");
            Smtp.EnableSsl = true;
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("abiturhse@gmail.com");
            Message.To.Add(new MailAddress(email));
            Message.Subject = subject;
            Message.Body = text;
            Smtp.Send(Message); //отправляем письмо                  
        }

        protected void approve_Click(object sender, EventArgs e)
        {
            /*var db = new DBClassesDataContext();
            var query =
                from plan in db.Plans
              //  where plan.ID == planindex                
                select plan;
            query.ToArray()[0].Status = 1;
            try
            {
                db.SubmitChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
           // viewPlan();
             */
        }

                    

        protected void download_Click1(object sender, EventArgs e)
        {            
            gvbind();
            string filename = "Report.xls";
            Response.Clear();
            Response.ClearContent();
            
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter(); 
            HtmlTextWriter htm = new HtmlTextWriter(sw);
            GridView1.RenderControl(htm);
            Response.Write(sw.ToString());
            StringWriter sw2 = new StringWriter();
            HtmlTextWriter htm2 = new HtmlTextWriter(sw2);
            GridView2.RenderControl(htm2);
            Response.Write(sw2.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            planID = int.Parse(DropDownList1.SelectedValue);
            gvbind();
        }
    }
}