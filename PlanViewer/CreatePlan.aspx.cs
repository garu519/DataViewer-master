using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PlanViewer.Models;
namespace PlanViewer
{
    public partial class createPlan : System.Web.UI.Page
    {
        string user;
        int id;
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
        }

        protected void ButtonSend_Click(object sender, EventArgs e)
        {
            PlansDataSource.Insert();
            GridView1.EditIndex = GridView1.Rows.Count;   
            //GridView1.Rows.Add();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public static class Alert
    {

    /// <summary>
    /// Shows a client-side JavaScript alert in the browser.
    /// </summary>
    /// <param name="message">The message to appear in the alert.</param>
    public static void Show(string message)
    {
       // Cleans the message to allow single quotation marks
       string cleanMessage = message.Replace("'", "\\'");
       string script ="<script type=\"text/javascript\">alert('"+ cleanMessage +"');</script>";

       // Gets the executing web page
       Page page = HttpContext.Current.CurrentHandler as Page;

       // Checks if the handler is a Page and that the script isn't allready on the Page
       if (page !=null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
       {
          page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
       }
    }    
}
}