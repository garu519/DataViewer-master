using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using PlanViewer.Models;
using PlanViewer.Account;

namespace PlanViewer.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (!OpenAuth.IsLocalUrl(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

        protected void regUser(object sender, EventArgs e)
        {
            TextBox userName = (TextBox)this.RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("UserName");
            TextBox email = (TextBox)this.RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Email");
            TextBox password = (TextBox)this.RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Password");
            TextBox password2 = (TextBox)this.RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("ConfirmPassword");

            if (userName.Text == "" || email.Text == "" || password.Text == ""
    || password2.Text == "")
            {
                Alert.Show("Пожалуйста, заполните все поля");
                return;
            }
            var db = new DBClassesDataContext();
            Contractor c = null;
            Customer cus = null;
            if (RadioButton1.Checked == true)//Заказчик
            {
                cus = new Customer { Name = userName.Text, Email = email.Text, Password = password.Text };
                db.Customers.InsertOnSubmit(cus);
            }
            else //Подрядчик
            {
                c = new Contractor { Name = userName.Text, Email = email.Text, Password = password.Text };
                db.Contractors.InsertOnSubmit(c);
            }
            try
            {
                db.SubmitChanges();
                Alert.Show("Запись успешно добавлена");
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Ошибка", "нет записи", true);
            }
            Response.Redirect("http://.../Default.aspx");
        }
    }
}