using K9.WebApplication.Services;

namespace K9.WebApplication.Packages
{
    public interface IServicePackage : IServiceBasePackage
    {
        IMembershipService MembershipService { get; set; }
        IAccountService AccountService { get; set; }
        IUserService UserService { get; set; }
        IClientService ClientService { get; set; }
    }
}