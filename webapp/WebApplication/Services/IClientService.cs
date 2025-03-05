using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Models;
using System.Collections.Generic;

namespace K9.WebApplication.Services
{
    public interface IClientService : IBaseService
    {
        Client GetOrCreateClientFromUser(User user);
        Client GetOrCreateClient(string stripeCustomerId, string fullName, string emailAddress,
            string phoneNumber = "", int? userId = null); 
        Client Find(int id);
        Client FindForUser(int userId);
        Client Find(string emailAddress);
        List<Client> ListClients();
        void EnableMarketingEmails(string externalId, bool value = true);
        bool AreMarketingEmailsEnableForContact(int id);
    }
}