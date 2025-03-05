using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.WebApplication.Helpers;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using System;

namespace K9.WebApplication.Services
{
    public abstract class BaseService : CachableBase, IBaseService
    {
        public IServiceBasePackage My { get; }

        public BaseService(IServiceBasePackage package)
        {
            My = package;
        }

        public void SendEmailToGetToTheRootAboutFailure(string errorMessage, int? userId = null)
        {
            var title = "A user encountered an error at an important step in their journey.";
            var body = TemplateParser.Parse(Globalisation.Dictionary.GeneralErrorEmail,
                new
                {
                    UserId = userId ?? Current.UserId,
                    ErrorMessage = errorMessage
                });

            try
            {
                My.Mailer.SendEmail(
                    title,
                    body,
                    My.WebsiteConfiguration.SupportEmailAddress,
                    My.WebsiteConfiguration.CompanyName);
            }
            catch (Exception ex)
            {
                My.Logger.Error(ex.GetFullErrorMessage());
            }
        }
    }
}