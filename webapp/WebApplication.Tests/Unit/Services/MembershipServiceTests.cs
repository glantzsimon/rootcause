﻿using K9.Base.DataAccessLayer.Models;
using K9.Base.WebApplication.Config;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Helpers;
using K9.SharedLibrary.Models;
using K9.WebApplication.Config;
using K9.WebApplication.Helpers;
using K9.WebApplication.Packages;
using K9.WebApplication.Services;
using K9.WebApplication.Services.Stripe;
using Moq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using K9.WebApplication.Exceptions;
using Xunit;

namespace K9.WebApplication.Tests.Unit.Services
{

    public class MembershipModelserviceTests
    {
        private readonly Mock<IServiceBasePackage> _GetToTheRootKiPackage = new Mock<IServiceBasePackage>();
        private readonly Mock<IRepository<User>> _usersRepository = new Mock<IRepository<User>>();
        private readonly Mock<IRepository<UserMembership>> _userMembershipRepository = new Mock<IRepository<UserMembership>>();
        private readonly Mock<IRepository<MembershipOption>> _membershipOptionRepository = new Mock<IRepository<MembershipOption>>();
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        private readonly Mock<IMailer> _mailer = new Mock<IMailer>();
        private readonly Mock<IOptions<WebsiteConfiguration>> _config = new Mock<IOptions<WebsiteConfiguration>>();
        private readonly Mock<IAuthentication> _authentication = new Mock<IAuthentication>();
        private readonly Mock<IOptions<StripeConfiguration>> _stripeConfig = new Mock<IOptions<StripeConfiguration>>();
        private readonly Mock<IStripeService> _stripeService = new Mock<IStripeService>();
        private readonly Mock<IClientService> _contactService = new Mock<IClientService>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private MembershipService _membershipservice;

        private static readonly int _userId = 1;

        private User _user = new User
        {
            Id = _userId,
            Username = "Simon"
        };

        private readonly MembershipOption _freeMembership = new MembershipOption
        {
            Id = 1,
            Name = "Free Membership",
            SubscriptionType = MembershipOption.ESubscriptionType.Free,
            Price = 10
        };

        private readonly MembershipOption _weeklyMembership = new MembershipOption
        {
            Id = 2,
            Name = "Weekly Platinum Membership",
            SubscriptionType = MembershipOption.ESubscriptionType.WeeklyPlatinum,
            Price = 90
        };

        private readonly MembershipOption _platinumMonthlyMembership = new MembershipOption
        {
            Id = 3,
            Name = "Monthly Platinum Membership",
            SubscriptionType = MembershipOption.ESubscriptionType.MonthlyPlatinum,
            Price = 20
        };

        private readonly MembershipOption _platinumYearlyMembership = new MembershipOption
        {
            Id = 4,
            Name = "Yearly Platinum Membership",
            SubscriptionType = MembershipOption.ESubscriptionType.AnnualPlatinum,
            Price = 180
        };

        public MembershipModelserviceTests()
        {
            var membershipOptions = new List<MembershipOption>
            {
                _freeMembership,
                _weeklyMembership,
                _platinumMonthlyMembership,
                _platinumYearlyMembership
            };

            HttpContext.Current = Helpers.Helpers.CreateHttpContextWithSession();

            _config.SetupGet(_ => _.Value).Returns(new WebsiteConfiguration
            {
                CompanyLogoUrl = "http://local",
                CompanyName = "Glantz Consulting",
                SupportEmailAddress = "support@gc.com"
            });

            _stripeConfig.SetupGet(_ => _.Value).Returns(new StripeConfiguration
            {
                SecretKey = "sk_12348765",
                PublishableKey = "pk_09872345"
            });

            _membershipOptionRepository.Setup(_ => _.List()).Returns(membershipOptions);
            _membershipOptionRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<MembershipOption, bool>>>())).Returns(membershipOptions);
            _membershipOptionRepository.Setup(_ => _.Find(_freeMembership.Id)).Returns(_freeMembership);
            _membershipOptionRepository.Setup(_ => _.Find(_weeklyMembership.Id)).Returns(_weeklyMembership);
            _membershipOptionRepository.Setup(_ => _.Find(_platinumMonthlyMembership.Id)).Returns(_platinumMonthlyMembership);
            _membershipOptionRepository.Setup(_ => _.Find(_platinumYearlyMembership.Id)).Returns(_platinumYearlyMembership);

            _userService = new Mock<IUserService>();
            _userService.Setup(_ => _.Find(It.IsAny<int>())).Returns(_user);

            _usersRepository.Setup(_ => _.Find(It.IsAny<int>())).Returns(_user);
            
            _GetToTheRootKiPackage = new Mock<IServiceBasePackage>();
            _GetToTheRootKiPackage.Setup(_ => _.UsersRepository).Returns(_usersRepository.Object);
            
            SessionHelper.SetCurrentUserId(_userId);
            SessionHelper.SetCurrentUserName("simon");

            _membershipservice = new MembershipService(
                _GetToTheRootKiPackage.Object,
                _membershipOptionRepository.Object,
                _userMembershipRepository.Object,
                new Mock<IRepository<Promotion>>().Object,
                new Mock<IRepository<Consultation>>().Object,
                new Mock<IRepository<UserConsultation>>().Object,
                new Mock<IConsultationService>().Object,
                new Mock<IPromotionService>().Object,
                _contactService.Object,
                new Mock<IEmailTemplateService>().Object);
        }

        [Fact]
        public void GetMembershipModel_StandardMonthly_CanUpgradeThree()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    UserId = _userId,
                    MembershipOptionId = _freeMembership.Id,
                    MembershipOption = _freeMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var model = _membershipservice.GetMembershipViewModel();

            Assert.Equal(1, _membershipservice.GetActiveUserMemberships(_userId).Count);
            Assert.Equal(userMembershipModels.First(), _membershipservice.GetActiveUserMembership());
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsSelected));
            Assert.Equal(3, model.MembershipModels.Count(_ => _.IsUpgrade));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSubscribed));
            Assert.Equal(3, model.MembershipModels.Count(_ => _.IsSelectable));
        }

        [Fact]
        public void GetMembershipModel_StandardYearly_CanUpgradeTwo()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    UserId = _userId,
                    MembershipOptionId = _weeklyMembership.Id,
                    MembershipOption = _weeklyMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddYears(1)
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var model = _membershipservice.GetMembershipViewModel();

            Assert.Equal(1, _membershipservice.GetActiveUserMemberships(_userId).Count);
            Assert.Equal(userMembershipModels.First(), _membershipservice.GetActiveUserMembership());
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsSelected));
            Assert.Equal(2, model.MembershipModels.Count(_ => _.IsUpgrade));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSubscribed));
            Assert.Equal(2, model.MembershipModels.Count(_ => _.IsSelectable));
        }

        [Fact]
        public void GetMembershipModel_PlatinumMonthly_CanUpgradeOne()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    UserId = _userId,
                    MembershipOptionId = _platinumMonthlyMembership.Id,
                    MembershipOption = _platinumMonthlyMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var model = _membershipservice.GetMembershipViewModel();

            Assert.Equal(1, _membershipservice.GetActiveUserMemberships(_userId).Count);
            Assert.Equal(userMembershipModels.First(), _membershipservice.GetActiveUserMembership());
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsSelected));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsUpgrade));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSubscribed));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSelectable));
        }

        [Fact]
        public void GetMembershipModel_PlatinumYearly_CanUpgradeNone()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    Id = 7,
                    UserId = _userId,
                    MembershipOptionId = _platinumYearlyMembership.Id,
                    MembershipOption = _platinumYearlyMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddYears(1),
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var model = _membershipservice.GetMembershipViewModel();

            Assert.Equal(1, _membershipservice.GetActiveUserMemberships(_userId).Count);
            Assert.Equal(userMembershipModels.First(), _membershipservice.GetActiveUserMembership());
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsSelected));
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsUpgrade));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSubscribed));
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsSelectable));
            Assert.Equal(7, model.MembershipModels.First().ActiveUserMembershipId);
        }

        [Fact]
        public void GetMembershipModel_CanUpgradeOne_AndSwitchOne()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var scheduledStartsOn = startsOn.AddMonths(1).AddDays(5);
            var scheduledUserMembership = new UserMembership
            {
                Id = 8,
                UserId = _userId,
                MembershipOptionId = _freeMembership.Id,
                MembershipOption = _freeMembership,
                StartsOn = scheduledStartsOn,
                EndsOn = scheduledStartsOn.AddMonths(1),
                IsAutoRenew = true
            };
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    Id = 7,
                    UserId = _userId,
                    MembershipOptionId = _platinumMonthlyMembership.Id,
                    MembershipOption = _platinumMonthlyMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                },
                scheduledUserMembership
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            _userMembershipRepository.Setup(_ => _.List())
                .Returns(userMembershipModels);

            var model = _membershipservice.GetMembershipViewModel();

            Assert.Equal(1, _membershipservice.GetActiveUserMemberships(_userId).Count);
            Assert.Equal(userMembershipModels.First(), _membershipservice.GetActiveUserMembership());
            Assert.Equal(0, model.MembershipModels.Count(_ => _.IsSelected));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsUpgrade));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSubscribed));
            Assert.Equal(1, model.MembershipModels.Count(_ => _.IsSelectable));
            Assert.Equal(7, model.MembershipModels.First().ActiveUserMembershipId);
        }

        [Fact]
        public void GetSwitchMembershipModel_ShouldThrowError_NoSubscriptions()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>().ToList();

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var ex = Assert.Throws<Exception>(() => _membershipservice.GetSwitchMembershipModel(1));
            Assert.Equal(Globalisation.Dictionary.SwitchMembershipErrorNotSubscribed, ex.Message);
        }

        [Fact]
        public void GetSwitchMembershipModel_ShouldThrowError_SameSubscriptionId()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    Id = 7,
                    UserId = _userId,
                    MembershipOptionId = _freeMembership.Id,
                    MembershipOption = _freeMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var ex = Assert.Throws<UserAlreadySubscribedException>(() => _membershipservice.GetSwitchMembershipModel(_freeMembership.Id));
            Assert.Equal(Globalisation.Dictionary.SwitchMembershipErrorAlreadySubscribed, ex.Message);
        }

        [Fact]
        public void GetSwitchMembershipModel_ShouldThrowError_ScheduledIsSameSubscriptionId()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var scheduledStartsOn = startsOn.AddMonths(1).AddDays(1);
            var scheduledUserMembership = new UserMembership
            {
                Id = 8,
                UserId = _userId,
                MembershipOptionId = _weeklyMembership.Id,
                MembershipOption = _weeklyMembership,
                StartsOn = scheduledStartsOn,
                EndsOn = scheduledStartsOn.AddMonths(1),
                IsAutoRenew = true
            };
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    Id = 7,
                    UserId = _userId,
                    MembershipOptionId = _platinumMonthlyMembership.Id,
                    MembershipOption = _platinumMonthlyMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                },
                scheduledUserMembership
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);


            var ex = Assert.Throws<UpgradeNotPossibleException>(() => _membershipservice.GetSwitchMembershipModel(_weeklyMembership.Id));
            Assert.Equal(Globalisation.Dictionary.CannotSwitchMembershipError, ex.Message);
        }

        [Fact]
        public void GetSwitchMembershipModel_IsUpgrade()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    Id = 7,
                    UserId = _userId,
                    MembershipOptionId = _freeMembership.Id,
                    MembershipOption = _freeMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);


            var result = _membershipservice.GetSwitchMembershipModel(_weeklyMembership.Id);
            Assert.True(result.IsUpgrade);
        }

        [Fact]
        public void GetPurchaseMembershipModel_ShouldThrowError_IfAlreadySubscribed()
        {
            AuthenticateUser();

            var startsOn = DateTime.Today.AddDays(-7);
            var userMembershipModels = new List<UserMembership>
            {
                new UserMembership
                {
                    Id = 7,
                    UserId = _userId,
                    MembershipOptionId = _weeklyMembership.Id,
                    MembershipOption = _weeklyMembership,
                    StartsOn = startsOn,
                    EndsOn = startsOn.AddMonths(1)
                }
            };

            _userMembershipRepository.Setup(_ => _.Find(It.IsAny<System.Linq.Expressions.Expression<Func<UserMembership, bool>>>()))
                .Returns(userMembershipModels);

            var ex = Assert.Throws<UserAlreadySubscribedException>(() => _membershipservice.GetPurchaseMembershipModel(_weeklyMembership.Id));
            Assert.Equal(Globalisation.Dictionary.PurchaseMembershipErrorAlreadySubscribed, ex.Message);
        }

        private void AuthenticateUser()
        {
            _authentication.SetupGet(_ => _.IsAuthenticated).Returns(true);
            _authentication.SetupGet(_ => _.CurrentUserId).Returns(_userId);
        }
    }
}
