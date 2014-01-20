using System.Data.Entity;

namespace ScholarshipManagementSystem.Models
{
    public class StudentContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<ScholarshipManagementSystem.Models.ScoringContext>());

        public StudentContext() : base("name=StudentContext")
        {
        }

        public DbSet<ScoringT> ScoringTs { get; set; }
        public DbSet<StudentInfo> StudentInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ScoringT>().HasRequired(s => s.ScoredStudent).WithMany(
              ).HasForeignKey(f => f.ScoredStudentInfoId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ScoringT>().HasRequired(s => s.ScoringStudent).WithMany(
              ).HasForeignKey(f => f.ScoringStudentInfoId).WillCascadeOnDelete(false);
        }

        public DbSet<BonusCompetition> BonusCompetitions { get; set; }
        public DbSet<BonusPaper> BonusPapers { get; set; }
        public DbSet<BonusProject> BonusProjects { get; set; }

        public DbSet<BonusT> BonusTs { get; set; }
        
        public DbSet<BonusCompetitionDetail> BonusCompetitionDetails { get; set; }
        public DbSet<BonusPaperDetail> BonusPaperDetails { get; set; }

        public DbSet<Study> Studys { get; set; }
        public DbSet<Dormitory> Dormitorys { get; set; }
        public DbSet<English> Englishs { get; set; }
        public DbSet<BigTableStandard> BigTableStandards { get; set; }

        public DbSet<Punishment> Punishments { get; set; }

        public DbSet<OnOff> OnOffs { get; set; }
    }
}
