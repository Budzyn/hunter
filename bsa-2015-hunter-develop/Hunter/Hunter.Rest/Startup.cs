using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Interface.Base;
using Hunter.DataAccess.Interface.Repositories;
using Hunter.Services.Interfaces;
using Microsoft.Owin;
using Owin;
using Ninject;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.Identity;
using Hunter.DataAccess.Db;
using Hunter.DataAccess.Interface;
using Hunter.Services;
using Ninject.Web.Common;
using Hunter.Common.Concrete;
using Hunter.DataAccess.Db.Repositories;
using Hunter.DataAccess.Entities;
using Hunter.Services.Services;
using Hunter.Services.Services.Interfaces;
using Hunter.Tools.LinkedIn;

[assembly: OwinStartup(typeof(Hunter.Rest.Startup))]

namespace Hunter.Rest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureOAuthToken(app);
        }

        public static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IUserStore<User, int>>().To<HunterUserStore>();

            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            #region Repositories
            kernel.Bind<IActivityRepository>().To<ActivityRepository>();
            kernel.Bind<ICandidateRepository>().To<CandidateRepository>();
            kernel.Bind<ICardRepository>().To<CardRepository>();
            kernel.Bind<IFeedbackRepository>().To<FeedbackRepository>();
            kernel.Bind<IInterviewRepository>().To<InterviewRepository>();
            kernel.Bind<IPoolRepository>().To<PoolRepository>();
            kernel.Bind<IResumeRepository>().To<ResumeRepository>();
            kernel.Bind<ISpecialNoteRepository>().To<SpecialNoteRepository>();
            kernel.Bind<ITestRepository>().To<TestRepository>();
            kernel.Bind<IUserProfileRepository>().To<UserProfileRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IUserRoleRepository>().To<UserRoleRepository>();
            kernel.Bind<IVacancyRepository>().To<VacancyRepository>();
            kernel.Bind<IFileRepository>().To<FileRepository>();
            kernel.Bind<IScheduledNotificationRepository>().To<ScheduledNotificationRepository>();
            #endregion

            #region Services
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IPoolService>().To<PoolService>();
            kernel.Bind<ICandidateService>().To<CandidateService>();
            kernel.Bind<IActivityService>().To<ActivityService>();
            kernel.Bind<IVacancyService>().To<VacancyService>();
            kernel.Bind<IUserRoleService>().To<UserRoleService>();
            kernel.Bind<IUserProfileService>().To<UserProfileService>();
            kernel.Bind<IActivityPostService>().To<ActivityPostService>();
            kernel.Bind<IFileService>().To<FileService>();
            kernel.Bind<ITestService>().To<TestService>();
            kernel.Bind<IFeedbackService>().To<FeedbackService>();
            kernel.Bind<ISpecialNoteService>().To<SpecialNoteService>();
            kernel.Bind<IResumeService>().To<ResumeService>();
            kernel.Bind<IActivityHelperService>().To<ActivityHelperService>();
            kernel.Bind<ICardService>().To<CardService>();
            kernel.Bind<IScheduledNotificationService>().To<ScheduledNotificationService>();
            #endregion

            kernel.Bind<Common.Interfaces.ILogger>().To<Logger>();
            kernel.Bind<IPublicPageParser>().To<PublicPageParser>();

            return kernel;
        }
    }
}