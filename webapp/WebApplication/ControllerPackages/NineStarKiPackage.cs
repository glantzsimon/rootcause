using K9.Base.DataAccessLayer.Models;
using K9.Base.WebApplication.Config;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Helpers;
using K9.SharedLibrary.Models;
using K9.WebApplication.Config;
using K9.WebApplication.Services;
using NLog;

namespace K9.WebApplication.Packages
{
    public class ServicePackage : ServiceBasePackage, IServicePackage
    {
        public ServicePackage(ILogger logger, IDataSetsHelper datasetsHelper, IRoles roles, IFileSourceHelper fileSourceHelper, IAuthentication authentication, IMailer mailer, IMembershipService membershipService, IAccountService accountService, IUserService userService,
           IClientService clientService, IRepository<User> usersRepository, IRepository<Role> rolesRepository, IRepository<UserRole> userRolesRepository, IRepository<Client> contactsRepository, IRepository<SystemSetting> systemSettingsRepository, IOptions<DefaultValuesConfiguration> defaultValuesConfiguration, IOptions<SmtpConfiguration> smtpConfiguration,
            IOptions<ApiConfiguration> apiConfiguration, IOptions<WebsiteConfiguration> websiteConfiguration, IOptions<GoogleConfiguration> googleConfiguration)
        : base(logger, datasetsHelper, roles, fileSourceHelper, authentication, mailer, usersRepository, rolesRepository, userRolesRepository, contactsRepository, systemSettingsRepository,
            defaultValuesConfiguration, smtpConfiguration, apiConfiguration, websiteConfiguration, googleConfiguration)
        {
            MembershipService = membershipService;
            AccountService = accountService;
            UserService = userService;
            ClientService = clientService;
        }
        
        public IMembershipService MembershipService { get; set; }
        public IAccountService AccountService { get; set; }
        public IUserService UserService { get; set; }
        public IClientService ClientService { get; set; }
        
    }
}
