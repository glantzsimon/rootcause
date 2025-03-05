using K9.WebApplication.Packages;

namespace K9.WebApplication.Services
{
    public interface IBaseService
    {
        IServiceBasePackage My { get; }
    }
}