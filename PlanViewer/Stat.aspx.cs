using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using PlanViewer.Models;
using System.Data;

namespace PlanViewer
{
    public partial class _Default : Page
    {
        string user;
        int id;
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
           conn.Open();
           SqlCommand cmd1 = new SqlCommand("Select DISTINCT [Fact].Materials from [Plan],[Fact] where [Fact].FactID = [Plan].PlanID AND [Plan].Customer=" + id, conn);
           SqlCommand cmd2 = new SqlCommand("Select DISTINCT [Plan].Materials from [Plan],[Fact] where [Fact].FactID = [Plan].PlanID AND [Plan].Customer=" + id, conn);
            //  SqlDataAdapter da = new SqlDataAdapter(cmd);
          //  DataTable dt = new DataTable();
         //   da.Fill(dt);
         //       Chart1.DataSource = dt;
         //       Chart1.DataBind();
         //       Chart1.Series["Series1"].XValueMember = "Res";
            try
            {
                List<int> rr = new List<int>();
                SqlDataReader rd = cmd1.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                        rr.Add(int.Parse(rd[0].ToString()));
                }
                int[] arr1 = rr.ToArray();
                conn.Close();
                conn.Open();
                rr = new List<int>();
                rd = cmd2.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                        rr.Add(int.Parse(rd[0].ToString()));
                }
                int[] arr2 = rr.ToArray();

                int[] res = new int[arr2.Length];
                for (int i = 0; i < arr1.Length; i++) res[i] = arr1[i] - arr2[i];

                Chart1.DataSource = res;
                Chart1.DataBindTable(res, "Разница");
                conn.Close();
            }
            catch
            {

            }
          //  Response.Redirect("/Account/Login.aspx");
            
        }
    }
}