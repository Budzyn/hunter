namespace Hunter.DataAccess.Entities
{
    using System.Data.Entity;

    public partial class HunterDbContext : DbContext
    {
        static HunterDbContext()
        {
            Database.SetInitializer(new HunterDbInitializer());
        }

        public HunterDbContext()
            //: base("Hunter")
            : base("name=HunterDb")
        {
            //Database.SetInitializer(new HunterDbInitializer());
        }

        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<Candidate> Candidate { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Interview> Interview { get; set; }
        public virtual DbSet<Pool> Pool { get; set; }
        public virtual DbSet<Resume> Resume { get; set; }
        public virtual DbSet<SpecialNote> SpecialNote { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Vacancy> Vacancy { get; set; }
        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<ScheduledNotification> ScheduledNotification { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.Card)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.Pool)
                .WithMany(e => e.Candidate)
                .Map(m => m.ToTable("Candidate_Pool").MapLeftKey("CandidateId").MapRightKey("PoolId"));

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.SpecialNote)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(e => e.Feedback)
                .WithRequired(e => e.Card)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(e => e.Interview)
                .WithRequired(e => e.Card)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Card>()
                .HasMany(e => e.Test)
                .WithRequired(e => e.Card)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Interview>()
                .Property(e => e.Comment)
                .IsFixedLength();

//            modelBuilder.Entity<Pool>()
//                .HasMany(e => e.Vacancy)
//                .WithRequired(e => e.Pool)
//                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.Pool)
                .WithMany(e => e.Vacancy)
                .Map(m =>
                {
                    m.ToTable("Vacancy_Pool");
                    m.MapLeftKey("VacancyId");
                    m.MapRightKey("PoolId");
                });

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Candidate)
                .WithOptional(e => e.UserProfile)
                .HasForeignKey(e => e.AddedByProfileId);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Card)
                .WithOptional(e => e.UserProfile)
                .HasForeignKey(e => e.AddedByProfileId);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Feedback)
                .WithOptional(e => e.UserProfile)
                .HasForeignKey(e => e.ProfileId);

            modelBuilder.Entity<UserProfile>()
               .HasMany(e => e.Test)
               .WithOptional(e => e.AssignedUserProfile)
               .HasForeignKey(e => e.AssignedUserProfileId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRole>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>()
                .HasMany(e => e.Test)
                .WithOptional(e => e.AssignedUserProfile)
                .HasForeignKey(e => e.AssignedUserProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.Card)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.SpecialNote)
                .WithOptional(e => e.Vacancy)
                .WillCascadeOnDelete(false);

        }
    }
}
