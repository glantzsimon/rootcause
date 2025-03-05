using K9.DataAccessLayer.Models;
using System.Data.Entity;

namespace K9.DataAccessLayer.Database
{
    public class LocalDb : Base.DataAccessLayer.Database.Db
    {
        public DbSet<Client> Contacts { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<MembershipOption> MembershipOptions { get; set; }
        public DbSet<UserMembership> UserMemberships { get; set; }
        public DbSet<Promotion> PromoCodes { get; set; }
        public DbSet<UserPromotion> UserPromoCodes { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<UserConsultation> UserConsultations { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<UserOTP> UserOtps { get; set; }
        public DbSet<EmailQueueItem> EmailQueueItems { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<MailingList> MailingLists { get; set; }
        public DbSet<MailingListUser> MailingListUsers { get; set; }
        public DbSet<MailingListContact> MailingListContacts { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<ArticleSection> ArticleSections { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientForbiddenFood> ClientForbiddenFoods { get; set; }
        public DbSet<DietaryRecommendation> DietaryRecommendations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }
        public DbSet<ClientProduct> ClientProducts { get; set; }

        public DbSet<Protocol> Protocols { get; set; }
        public DbSet<ProtocolProduct> ProtocolProducts { get; set; }
        public DbSet<ProtocolProductPack> ProtocolProductPacks { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ProtocolSection> ProtocolSections { get; set; }
        public DbSet<ProtocolSectionProduct> ProtocolSectionProducts { get; set; }
        public DbSet<ProtocolActivity> ProtocolActivities { get; set; }
        public DbSet<ProtocolDietaryRecommendation> ProtocolDietaryRecommendations { get; set; }
        public DbSet<ProtocolFoodItem> ProtocolFoodItems { get; set; }

        public DbSet<Models.HealthQuestionnaire> HealthQuestionnaires { get; set; }
    }
}
