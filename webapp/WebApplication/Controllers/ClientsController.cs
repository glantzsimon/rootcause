using K9.Base.WebApplication.Extensions;
using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using NLog;
using System;
using System.Linq;
using System.Web.Mvc;

namespace K9.WebApplication.Controllers
{
    [Authorize]
    [RequirePermissions(Role = RoleNames.Administrators)]
    public class ClientsController : BaseRootController<Client>
    {
        private readonly IRepository<Donation> _donationRepository;
        private readonly IMailChimpService _mailChimpService;
        
        public ClientsController(IControllerPackage<Client> controllerPackage, IServicePackage servicePackage,
            IRepository<Donation> donationRepository, IMailChimpService mailChimpService) 
            : base(controllerPackage, servicePackage)
        {
            _donationRepository = donationRepository;
            _mailChimpService = mailChimpService;

            RecordUpdated += ContactsController_RecordUpdated;
        }

        private void ContactsController_RecordUpdated(object sender, Base.WebApplication.EventArgs.CrudEventArgs e)
        {
            var contact = (Client)e.Item;
            var user = My.UsersRepository.Find(u => u.EmailAddress == contact.EmailAddress).FirstOrDefault();

            if (user != null && user.IsUnsubscribed != contact.IsUnsubscribed)
            {
                user.IsUnsubscribed = contact.IsUnsubscribed;

                try
                {
                    My.UsersRepository.Update(user);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Error,
                        $"ContactsController => ContactsController_RecordUpdated => Could not update user => UserId: {user.Id} => Error: {ex.GetFullErrorMessage()}");
                    throw;
                }
            }
        }

        public ActionResult ImportContactsFromDonations()
        {
            var existing = Repository.List();

            var contactsToImport = _donationRepository.Find(c => !string.IsNullOrEmpty(c.CustomerEmail) && existing.All(e => e.EmailAddress != c.CustomerEmail))
                .Select(e => new Client
                {
                    FullName = e.CustomerName,
                    EmailAddress = e.CustomerEmail
                }).ToList();

            Repository.CreateBatch(contactsToImport);

            return RedirectToAction("Index");
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SignUpToNewsLetter()
        {
            return View(new Client());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SignUpToNewsLetter(Client client)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Repository.Exists(_ => _.EmailAddress == client.EmailAddress))
                    {
                        ModelState.AddModelError("EmailAddress", K9.Globalisation.Dictionary.DuplicateContactError);
                    }
                    else
                    {
                        Repository.Create(client);
                        return RedirectToAction("SignUpSuccess");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.GetFullErrorMessage());
                    ModelState.AddErrorMessageFromException<Client>(ex, client);
                }
            }

            return View("", client);
        }

        public ActionResult SignUpSuccess()
        {
            return View();
        }

        public ActionResult AddAllContactsToMailChimp()
        {
            _mailChimpService.AddAllContacts();
            return RedirectToAction("MailChimpImportSuccess");
        }

        public ActionResult MailChimpImportSuccess()
        {
            return View();
        }
    }
}
