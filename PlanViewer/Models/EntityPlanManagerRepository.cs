using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlanViewer.Models
{
    public class EntityPlanManagerRepository
    {
        public class EF_PlanRepository : PlanViewer.Models.IPlanRepository
        {

            private DBClassesDataContext _db = new DBClassesDataContext();

            public Plan GetPlanByID(int id)
            {
                return _db.Plans.FirstOrDefault(d => d.ID == id);
            }

            public List<Plan> GetAllPlans()
            {
                return _db.Plans.ToList();
            }

            public void UpdatePlan(Plan planToUpdate)
            {
                _db.Plans.InsertOnSubmit(planToUpdate);
               _db.SubmitChanges();
            }

            public void CreateNewPlan(Plan planToCreate)
            {
                _db.Plans.InsertOnSubmit(planToCreate);
                _db.SubmitChanges();
                //   return contactToCreate;
            }

            public void SaveChanges()
            {
                _db.SubmitChanges();
            }

            public void DeletePlan(int id)
            {
                var conToDel = GetPlanByID(id);
                _db.Plans.DeleteOnSubmit(conToDel);
                _db.SubmitChanges();
            }

        }
    }
}