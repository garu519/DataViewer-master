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
            if (!Page.IsPostBack)
            {
                providerstring = "SELECT CONCAT (N'Подрядчик: ', Contractor.Name , N', План: ' , [Plan].Name) AS res, [Plan].PlanID FROM [Plan] INNER JOIN Contractor ON Contractor.ID = [Plan].Contractor INNER JOIN Customer ON [Plan].Customer = " + id + " GROUP BY [Plan].PlanID, [Plan].Name, Contractor.Name";
                //providerstring = "SELECT Contractor.Name, [Plan].Name FROM [Plan] INNER JOIN [Customer] ON [Customer].[ID] = [Plan].[Customer] INNER JOIN [Contractor] ON [Plan].Customer = Customer.ID where Contractor.ID=10 GROUP BY [Plan].PlanID , Customer.Name";
                SqlDataSource1.SelectCommand = string.Format(providerstring);
                DataBind();
            }
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
            if (Page.IsPostBack)
            {
                buildPlanTable();
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
            //GridView1.EditIndex = -1;
            //gvbind();
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
            //query.Cast<Plan>();
            Plan[] results = null;
            try
            {
                results = query.ToArray<Plan>();
                Table1.Caption = results[0].Name;
                if (results[0].Status < 3 || results[0].Status==4)
                {
                    Table1.Caption += ", " + "Не одобрен";
                    approve.Visible = true;
                    
                }
                else
                {
                    Table1.Caption += ", " + "Одобрен"; 
                }
                bool gotfacts = false;
                foreach (Plan item in results)
                {
                    var fact_query =
                        from fact in db.Facts
                        where fact.ExtPlanID == item.ID
                        select fact;
                    Fact facts;
                    try
                    {
                        facts = fact_query.First<Fact>();
                        gotfacts = true;
                    }
                    catch (Exception ex)
                    {
                        facts = new Fact { Labor = "er", Materials = "er", Mechanisms = "er" };
                    }

                    TableRow tr = new TableRow();
                    List<TableCell> cells = new List<TableCell>();
                    TableCell c = new TableCell();
                    c.Text = item.ID + "";
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    c.Visible = false;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.Object;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.WorkType;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.CostName;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.UnitName;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.Labor;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = facts.Labor;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    c.BackColor = System.Drawing.Color.PowderBlue;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.Materials;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = facts.Materials;
                    c.BackColor = System.Drawing.Color.PowderBlue;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.Mechanisms;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = facts.Mechanisms;
                    c.BackColor = System.Drawing.Color.PowderBlue;
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    cells.Add(c);
                    c = new TableCell();
                    c.Text = item.Status + "";
                    c.BorderStyle = BorderStyle.Solid;
                    c.BorderWidth = 1;
                    c.BorderColor = System.Drawing.Color.Black;
                    c.Visible = false;
                    //c.Enabled = true;
                    cells.Add(c);
                    foreach (TableCell cell in cells)
                    {
                        tr.Cells.Add(cell);
                    }
                    Table1.Rows.Add(tr);
                    planID = int.Parse(DropDownList1.SelectedValue);
                }
                if (!gotfacts)
                {
                    foreach (TableRow tr in Table1.Rows)
                    {
                        tr.Cells[6].Visible = false;
                        tr.Cells[8].Visible = false;
                        tr.Cells[10].Visible = false;
                    }
                }
                else
                {
                    foreach (TableRow tr in Table1.Rows)
                    {
                        tr.Cells[6].Visible = true;
                        tr.Cells[8].Visible = true;
                        tr.Cells[10].Visible = true;
                    }
                }
            }
            catch (FormatException fe)
            {
                Table1.Visible = false;
                return;
            }

            
        }

        /*protected void gvbind()
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
        }*/

        
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
            //var db = new DBClassesDataContext();
            //var query =
            //    from plan in db.Plans
            //    where plan.PlanID == int.Parse(DropDownList1.SelectedValue)
            //    select plan;
            //query.ToArray()[0].Status = 3;
            try
            {
                String updatePlan = "UPDATE [Plan] SET Status=3 WHERE PlanID=" + int.Parse(DropDownList1.SelectedValue);
                conn.Open();
                SqlCommand cmd = new SqlCommand(updatePlan, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                Alert.Show("Статус плана обновлён.");
            }
            catch { }
            //try
            //{
            //    db.SubmitChanges();
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
            //}

        }

        protected void reject_Click(object sender, EventArgs e)
        {
            //var db = new DBClassesDataContext();
            //var query =
            //    from plan in db.Plans
            //    where plan.PlanID == int.Parse(DropDownList1.SelectedValue)
            //    select plan;
            //query.ToArray()[0].Status = 4;
            //try
            //{
            //    db.SubmitChanges();
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
            //}
            try
            {
                String updatePlan = "UPDATE [Plan] SET Status=4 WHERE PlanID=" + int.Parse(DropDownList1.SelectedValue);
                conn.Open();
                SqlCommand cmd = new SqlCommand(updatePlan, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                Alert.Show("Статус плана обновлён.");
            }
            catch { }

        }

                    

        protected void download_Click1(object sender, EventArgs e)
        {            
            
            //gvbind();
            string filename = "Report.xls";
            Response.Clear();
            Response.ClearContent();
            
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/excel";
            //StringWriter sw = new StringWriter(); 
            //HtmlTextWriter htm = new HtmlTextWriter(sw);
            //Table1.RenderControl(htm);
            //Response.Write(sw.ToString());
            StringWriter sw2 = new StringWriter();
            HtmlTextWriter htm2 = new HtmlTextWriter(sw2);
            Table1.RenderControl(htm2);
            Response.Write(sw2.ToString());
            Response.End();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }

        protected void DropDownList1_DataBound(object sender, EventArgs e)
        {
            //planID = int.Parse(DropDownList1.SelectedValue);
            //gvbind();
            buildPlanTable();
        }

        protected void sendRequest_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587); //формируем письмо
                    Smtp.UseDefaultCredentials = false;
                    Smtp.Credentials = new NetworkCredential("techupmailer@gmail.com", "techup2013");                                        
                    Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    Smtp.EnableSsl = true;
                    MailMessage Message = new MailMessage();
                    Message.From = new MailAddress("techupmailer@gmail.com");
                    var db = new DBClassesDataContext();
                    var query =
                        from plan in db.Plans
                        where plan.PlanID == planID
                        select plan;
                    Plan pl = query.First();
                    var query2 =
                        from contr in db.Contractors
                        where contr.ID == pl.Contractor
                        select contr;
                    Contractor ct = query2.First();
                    var query3 =
                        from cust in db.Customers
                        where cust.ID == id
                        select cust;
                    Customer cs = query3.First();
                    Message.To.Add(new MailAddress(ct.Email));
                    Message.Subject = "Запрос по плану: " + pl.Name + " "+ Subject.Text + " ";
                    Message.Body = "Заказчик: "+cs.Name +"\n" + MessageText.Text;
                    Smtp.Send(Message); //отправляем письмо 
                    Alert.Show("Письмо успешно отправлено!");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.StackTrace);
                }
            }
        }
    }
}