﻿using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using K9.WebApplication.Exceptions;
using K9.WebApplication.Models;
using K9.WebApplication.Packages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K9.WebApplication.Services
{
    public class MailerService : BaseService, IMailerService
    {
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IMailingListService _mailingListService;
        private readonly IPromotionService _promotionService;
        private readonly IEmailQueueService _emailQueueService;
        private readonly IRepository<Promotion> _promotionsRepository;

        public MailerService(IServiceBasePackage my, IEmailTemplateService emailTemplateService, IMailingListService mailingListService, IPromotionService promotionService, IEmailQueueService emailQueueService, IRepository<Promotion> promotionsRepository)
            : base(my)
        {
            _emailTemplateService = emailTemplateService;
            _mailingListService = mailingListService;
            _promotionService = promotionService;
            _emailQueueService = emailQueueService;
            _promotionsRepository = promotionsRepository;
        }

        public void TestEmailTemplate(int id)
        {
            var emailTemplate = _emailTemplateService.Find(id);
            if (emailTemplate == null)
            {
                My.Logger.Error($"MailerService => TestEmailTemplate => Email Template {id} not found");
                throw new NotFoundException();
            }
            var systemUser = My.UsersRepository.Find(e => e.Username == "SYSTEM").FirstOrDefault();
            SendEmailTemplateToUser(emailTemplate.Id, systemUser, true);
        }

        public void SendEmailTemplateToUser(int id, User user)
        {
            SendEmailTemplateToUser(id, user, false);
        }

        public List<MailingListSendResultItem> SendEmailTemplateToUsers(int id, List<User> users)
        {
            var results = new List<MailingListSendResultItem>();

            try
            {
                var emailTemplate = _emailTemplateService.Find(id);
                if (emailTemplate == null)
                {
                    My.Logger.Error($"MailerService => SendEmailTemplateToUsers => Email Template {id} not found");
                    throw new NotFoundException();
                }

                if (emailTemplate.PromotionId.HasValue)
                {
                    var promotion = _promotionService.Find(emailTemplate.PromotionId.Value);

                    foreach (var user in users)
                    {
                        var mailingListSendResultItem = new MailingListSendResultItem
                        {
                            UserId = user.Id,
                            EmailAddress = user.EmailAddress,
                            RecipientName = user.FullName
                        };

                        if (!user.IsUnsubscribed)
                        {
                            try
                            {
                                _promotionService.SendPromotionFromTemplateToUser(user.Id, emailTemplate, promotion,
                                    true, TimeSpan.FromMinutes(1));
                                mailingListSendResultItem.IsSuccess = true;
                            }
                            catch (Exception e)
                            {
                                My.Logger.Error(
                                    $"MailerService => SendEmailTemplateToUsers => {e.GetFullErrorMessage()}");
                                mailingListSendResultItem.ErrorMessage = e.GetFullErrorMessage();
                            }

                            results.Add(mailingListSendResultItem);
                        }
                        else
                        {
                            mailingListSendResultItem.IsSuccess = false;
                            mailingListSendResultItem.ErrorMessage = Globalisation.Dictionary.UserIsUnsubscribed;
                            results.Add(mailingListSendResultItem);
                        }
                    }
                }
                else
                {
                    foreach (User user in users)
                    {
                        var mailingListSendResultItem = new MailingListSendResultItem
                        {
                            UserId = user.Id,
                            EmailAddress = user.EmailAddress,
                            RecipientName = user.FullName
                        };

                        if (!user.IsUnsubscribed)
                        {
                            try
                            {
                                var parsedTemplate = _emailTemplateService.Parse(
                                    emailTemplate.Id,
                                    user.FirstName,
                                    My.UrlHelper.AbsoluteAction("UnsubscribeUser", "UsersController",
                                        new { externalId = user.Name }), null);

                                _emailQueueService.AddEmailToQueueForUser(emailTemplate.Id, user.Id,
                                    emailTemplate.Subject, parsedTemplate, EEmailType.General, TimeSpan.FromMinutes(1));

                                mailingListSendResultItem.IsSuccess = true;
                            }
                            catch (Exception e)
                            {
                                mailingListSendResultItem.ErrorMessage = e.GetFullErrorMessage();
                                My.Logger.Error(
                                    $"MailerService => SendEmailTemplateToUsers => {e.GetFullErrorMessage()}");
                            }

                            results.Add(mailingListSendResultItem);
                        }
                        else
                        {
                            mailingListSendResultItem.IsSuccess = false;
                            mailingListSendResultItem.ErrorMessage = Globalisation.Dictionary.UserIsUnsubscribed;
                            results.Add(mailingListSendResultItem);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                My.Logger.Error($"MailerService => SendEmailTemplateToUsers => {e.GetFullErrorMessage()}");
                throw new Exception($"MailerService => SendEmailTemplateToUsers => An error occured when sending mail to users. Please check the logs for details.");
            }

            return results;
        }

        private void SendEmailTemplateToUser(int id, User user, bool isTest)
        {
            try
            {
                var emailTemplate = _emailTemplateService.Find(id);
                if (emailTemplate == null)
                {
                    My.Logger.Error($"MailerService => SendEmailTemplateToUser => Email Template {id} not found");
                    throw new NotFoundException();
                }

                if (user.IsUnsubscribed && !isTest)
                {
                    throw new Exception(Globalisation.Dictionary.UserIsUnsubscribed);
                }

                if (emailTemplate.PromotionId.HasValue)
                {
                    var promotion = _promotionService.Find(emailTemplate.PromotionId.Value);
                    _promotionService.SendPromotionFromTemplateToUser(user.Id, emailTemplate, promotion, false, null, isTest);
                    return;
                }
                else if (emailTemplate.MembershipOptionId.HasValue)
                {
                    var discount = emailTemplate.SystemEmailTemplate == ESystemEmailTemplate.FirstMembershipReminder ?
                        EDiscount.FirstDiscount :
                        emailTemplate.SystemEmailTemplate == ESystemEmailTemplate.SecondMembershipReminder ?
                            EDiscount.SecondDiscount :
                            emailTemplate.SystemEmailTemplate == ESystemEmailTemplate.ThirdMembershipReminder ?
                                EDiscount.ThirdDiscount :
                                EDiscount.None;

                    var promotion = _promotionsRepository
                        .Find(e => e.MembershipOptionId == emailTemplate.MembershipOptionId.Value &&
                                   e.Discount == discount &&
                                   e.Name == emailTemplate.Name).FirstOrDefault();

                    if (promotion != null)
                    {
                        _promotionService.SendPromotionFromTemplateToUser(user.Id, emailTemplate, promotion, false, null, isTest);
                        return;
                    }
                }

                var parsedTemplate = _emailTemplateService.Parse(
                        emailTemplate.Id,
                        user.FirstName,
                        My.UrlHelper.AbsoluteAction("UnsubscribeUser", "UsersController", new { externalId = user.Name }), null);

                _emailQueueService.AddEmailToQueueForUser(emailTemplate.Id, user.Id, emailTemplate.Subject, parsedTemplate, EEmailType.General, TimeSpan.FromSeconds(1), isTest);
            }
            catch (Exception e)
            {
                My.Logger.Error($"MailerService => SendEmailTemplateToUser => Error: {e.GetFullErrorMessage()}");
                throw;
            }
        }
    }
}