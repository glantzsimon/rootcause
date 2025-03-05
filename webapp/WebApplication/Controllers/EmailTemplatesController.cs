﻿using K9.Base.WebApplication.Filters;
using K9.Base.WebApplication.UnitsOfWork;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Extensions;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using K9.WebApplication.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using K9.WebApplication.Exceptions;

namespace K9.WebApplication.Controllers
{
    [RequirePermissions(Role = RoleNames.Administrators)]
    [Authorize]
    [RoutePrefix("emailtemplates")]
    public class EmailTemplatesController : BaseRootController<EmailTemplate>
    {
        private readonly IMailingListService _mailingListService;
        private readonly IMailerService _mailerService;

        public EmailTemplatesController(IControllerPackage<EmailTemplate> controllerPackage, IServicePackage servicePackage, IMailingListService mailingListService, IMailerService mailerService)
            : base(controllerPackage, servicePackage)
        {
            _mailingListService = mailingListService;
            _mailerService = mailerService;
        }

        [Route("send-to-user")]
        public ActionResult SendToUser(int id)
        {
            var emailTemplate = Repository.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }

            return View(new SendEmailTemplateViewModel
            {
                EmailTemplate = emailTemplate
            });
        }

        [Route("send-to-user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendToUser(SendEmailTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.UserId.HasValue)
                {
                    ModelState.AddModelError("UserId", Base.Globalisation.Dictionary.FieldIsRequired);
                }

                if (ModelState.IsValid)
                {
                    var user = My.UsersRepository.Find(model.UserId.Value);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "User not found");
                    }
                    else
                    {
                        try
                        {
                            _mailerService.SendEmailTemplateToUser(model.EmailTemplate.Id, user);
                            return View("SendEmailTemplateSuccess");
                        }
                        catch (NotFoundException e)
                        {
                            ViewBag.ErrorMessage = "Email Template not found";
                            return View("SendEmailTemplateFailure");
                        }
                        catch (Exception e)
                        {
                            ViewBag.ErrorMessage = e.GetFullErrorMessage();
                            return View("SendEmailTemplateFailure");
                        }
                    }
                }
            }
            return View(model);
        }

        [Route("send-to-mailinglist")]
        public ActionResult SendToMailingList(int id)
        {
            var emailTemplate = Repository.Find(id);
            if (emailTemplate == null)
            {
                return HttpNotFound();
            }

            ViewData["MailingListListItems"] = _mailingListService.GetMailingListListItems();
            return View(new SendEmailTemplateViewModel
            {
                EmailTemplate = emailTemplate
            });
        }

        [Route("send-to-mailinglist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendToMailingList(SendEmailTemplateViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!model.MailingListId.HasValue)
                {
                    ModelState.AddModelError("MailingListId", Base.Globalisation.Dictionary.FieldIsRequired);
                }

                if (ModelState.IsValid)
                {
                    var mailingList = _mailingListService.Find(model.MailingListId.Value, true);
                    if (mailingList == null)
                    {
                        ModelState.AddModelError("", "Mailing List not found");
                    }
                    else
                    {
                        try
                        {
                            var results = _mailerService.SendEmailTemplateToUsers(model.EmailTemplate.Id, mailingList.Users);
                            return View("SendEmailTemplateResults", results.OrderBy(e => e.IsSuccess).ThenBy(e => e.RecipientName).ToList());

                        }
                        catch (NotFoundException e)
                        {
                            ViewBag.ErrorMessage = "Email Template not found";
                            return View("SendEmailTemplateFailure");
                        }
                        catch (Exception e)
                        {
                            ViewBag.ErrorMessage = e.GetFullErrorMessage();
                            return View("SendEmailTemplateFailure");
                        }
                    }
                }
            }
            return View(model);
        }

        [Route("test")]
        public ActionResult TestEmailTemplate(int id)
        {
            try
            {
                _mailerService.TestEmailTemplate(id);
                return View("SendEmailTemplateSuccess");
            }
            catch (NotFoundException e)
            {
                ViewBag.ErrorMessage = "Email Template not found";
                return View("SendEmailTemplateFailure");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.GetFullErrorMessage();
                return View("SendEmailTemplateFailure");
            }
        }
    }
}