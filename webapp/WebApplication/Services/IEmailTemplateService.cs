using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;

namespace K9.WebApplication.Services
{
    public interface IEmailTemplateService : IBaseService
    {
        EmailTemplate Find(int id);
        EmailTemplate FindSystemTemplate(ESystemEmailTemplate systemEmailTemplate);
        string ParseForUser(int emailTemplateId, User user, object data);
        string ParseForContact(int emailTemplateId, Client client, object data);
        string ParseForUser(EmailTemplate emailTemplate, User user, object data);
        string ParseForContact(EmailTemplate emailTemplate, Client client, object data);
        string ParseForUser(string title, string body, User user, object data);
        string ParseForContact(string title, string body, Client client, object data);
        string Parse(int emailTemplateId, string recipientFirstName, string unsubscribeLink, object data);
    }
}