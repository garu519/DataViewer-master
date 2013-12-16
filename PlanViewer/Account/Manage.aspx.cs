using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
using System.Net.Mail;
using System.Net;
using PlanViewer.Models;
namespace PlanViewer.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected bool CanRemoveExternalLogins
        {
            get;
            private set;
        }
        private string user;
        protected void Page_Load()
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
            
            //foreach (var role in Roles.GetRolesForUser())
            //{
            //    if (role.Equals(Global.contractorRole))
            //    {
            //        Response.Redirect("../NewPlan.aspx");
            //        return;
            //    }
            //    else 
            //    {
            //        Response.Redirect("../ViewPlan.aspx");
            //        return;             
            //    }
            //}
            if (!IsPostBack)
            {
                // Determine the sections to render
                var hasLocalPassword = OpenAuth.HasLocalPassword(User.Identity.Name);                
                changePassword.Visible = hasLocalPassword;

                CanRemoveExternalLogins = hasLocalPassword;

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Ваш пароль был успешно изменён. Новые данные для доступа в систему отправлены на Вашу почту."                        
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }
            }

        }
        


        public IEnumerable<OpenAuthAccountData> GetExternalLogins()
        {
            var accounts = OpenAuth.GetAccountsForUser(User.Identity.Name);
            CanRemoveExternalLogins = CanRemoveExternalLogins || accounts.Count() > 1;
            return accounts;
        }

        public void RemoveExternalLogin(string providerName, string providerUserId)
        {
            var m = OpenAuth.DeleteAccount(User.Identity.Name, providerName, providerUserId)
                ? "?m=RemoveLoginSuccess"
                : String.Empty;
            Response.Redirect("~/Account/Manage" + m);
        }


        protected static string ConvertToDisplayDateTime(DateTime? utcDateTime)
        {
            // You can change this method to convert the UTC date time into the desired display
            // offset and format. Here we're converting it to the server timezone and formatting
            // as a short date and a long time string, using the current thread culture.
            return utcDateTime.HasValue ? utcDateTime.Value.ToLocalTime().ToString("G") : "[never]";
        }       

        protected void Unnamed_ChangedPassword(object sender, EventArgs e)
        {
            string newPwd = ChangePwd.NewPassword;
            
            try
            {
                SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587); //формируем письмо
                Smtp.UseDefaultCredentials = false;
                Smtp.Credentials = new NetworkCredential("techupmailer@gmail.com", "techup2013");
                Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                Smtp.EnableSsl = true;
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("techupmailer@gmail.com");
                string connectionStr = WebConfigurationManager.ConnectionStrings["TeamProjectDBConnectionString1"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectionStr);
                string qupdate = ""; 
                
                foreach (var role in Roles.GetRolesForUser())
                {
                    if (role.Equals(Global.contractorRole))
                    {
                        qupdate = "UPDATE Contractor SET Password='"+newPwd+"' WHERE email='"+user+"'";
                        break;
                    }
                    else
                    {
                        qupdate = "UPDATE Customer SET Password='" + newPwd + "' WHERE email='" + user + "'";
                        break;
                    }
                }
                conn.Open();
                SqlCommand cmdd = new SqlCommand(qupdate, conn);
                cmdd.ExecuteNonQuery();
                conn.Close();
                Message.To.Add(new MailAddress(Membership.GetUser().UserName));
                Message.Subject = "Изменение данных для доступа в систему";
                Message.Body = "Ваши обновленные данные: \n" +"email: " + user + "\n" + "Пароль: " + newPwd;
                Smtp.Send(Message); //отправляем письмо                 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.StackTrace);
            }
        }

              
    }
}