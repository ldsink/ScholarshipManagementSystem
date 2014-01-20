using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScholarshipManagementSystem.Models
{
    using System.Data.Entity;

    public class StudentContextInitializer : DropCreateDatabaseIfModelChanges<StudentContext>
    {
        protected override void Seed(StudentContext context)
        {
            // BigTable
            var standards = new List<BigTableStandard>()
            {
                new BigTableStandard() { Id = 1, Grade = "F11", Study = 0.6F, Bonus = 0.2F, Dormitory = 0.1F, Scoring = 0.1F}
            };
            standards.ForEach(p => context.BigTableStandards.Add(p));
            context.SaveChanges();
        }
    }
}