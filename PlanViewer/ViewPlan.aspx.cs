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
namespace PlanViewer
{
    public partial class ViewPlan : System.Web.UI.Page
    {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //viewPlan();
        }
        
        protected void PlansDataSource_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            
        }        
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void sendComment(object sender, EventArgs e)
        {
            /*string emailString = email;
            string subject = "Комментарий заказчика";
            string text = "Здравствуйте,  " + name + "!" + ".\n" +
            "Вы успешно зарегистрированы в системе взаимодействия подрядчиков и заказчиков." + ".\n" +
            "Ваша роль в системе: заказчик " + "\nС уважением, администрация сервиса.";
            sendEmail(emailString, subject, text);*/
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

        protected void Button1_Click(object sender, EventArgs e)
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

        protected void Button2_Click(object sender, EventArgs e)
        {
           /* var db = new DBClassesDataContext();
            var query =
                from plan in db.Plans
               // where plan.ID == planindex
                select plan;
            query.ToArray()[0].Status = 2;
            try
            {
                db.SubmitChanges();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
         //   viewPlan();
            string email;
            try
            {
              //  email = contr[0].Email;
            }
            catch (Exception)
            {
                email = "";
            }
            ClientScript.RegisterStartupScript(this.GetType(), "mailto",
          //     "<script type = 'text/javascript'>parent.location='mailto:" + email +
               "'</script>");
            */
        }
    }
}