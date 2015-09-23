using System.Data.Entity.Migrations;

namespace Hunter.DataAccess.Entities.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Entities.HunterDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Hunter.DataAccess.Db.HunterDbContext";
        }

        protected override void Seed(Hunter.DataAccess.Entities.HunterDbContext context)
        {
            HunterDbInitializer.SeedClearDb(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
