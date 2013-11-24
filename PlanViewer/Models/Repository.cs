using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanViewer.Models
{

        public interface IPlanRepository {
        void CreateNewPlan(Plan planToCreate);
        void DeletePlan(int id);
        void UpdatePlan(Plan planToUpdate);
        Plan GetPlanByID(int id);
        List<Plan> GetAllPlans();
        void SaveChanges();

        }
}