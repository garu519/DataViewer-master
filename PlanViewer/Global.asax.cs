using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using PlanViewer;
using PlanViewer.Models;

namespace PlanViewer
{
    public class Global : HttpApplication
    {
        public const string customerRole = "Customer";
        public const string contractorRole = "Contractor";
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            if (!Roles.RoleExists(customerRole))
            {
                Roles.CreateRole(customerRole);
            }
            if (!Roles.RoleExists(contractorRole))
            {
                Roles.CreateRole(contractorRole);
            }  
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
