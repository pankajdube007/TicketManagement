using System.Data.Entity;
using System.Diagnostics;
using TicketManagement.Models;

namespace TicketManagement.Concrete
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=DatabaseConnection")
        {
            //Database.SetInitializer<DatabaseContext>(null);
            //Database.SetInitializer<DatabaseContext>(new CreateDatabaseIfNotExists<DatabaseContext>());
            //Database.SetInitializer<DatabaseContext>(new DropCreateDatabaseIfModelChanges<DatabaseContext>());
            //Database.SetInitializer<DatabaseContext>(new DropCreateDatabaseAlways<DatabaseContext>());
            Database.SetInitializer<DatabaseContext>(new DatabaseDBInitializer());
            Database.Log = (query) => Debug.Write(query);

        }

        public DbSet<MenuCategory> MenuCategory { get; set; }
        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<SubMenuMaster> SubMenuMasters { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Usermaster> Usermasters { get; set; }

        public DbSet<PasswordMaster> PasswordMaster { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<SavedAssignedRoles> SavedAssignedRoles { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketDetails> TicketDetails { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<AttachmentDetails> AttachmentDetails { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<TicketReply> TicketReply { get; set; }
        public DbSet<TicketReplyDetails> TicketReplyDetails { get; set; }
        public DbSet<ReplyAttachment> ReplyAttachment { get; set; }
        public DbSet<ReplyAttachmentDetails> ReplyAttachmentDetails { get; set; }
        public DbSet<AgentCheckInStatusSummary> AgentCheckInStatusSummary { get; set; }
        public DbSet<AgentCheckInStatusDetails> AgentCheckInStatusDetails { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<ProfileImage> ProfileImage { get; set; }
        public DbSet<ProfileImageStatus> ProfileImageStatus { get; set; }
        public DbSet<Signatures> Signatures { get; set; }
        public DbSet<SmtpEmailSettings> SmtpEmailSettings { get; set; }
        public DbSet<TicketDeleteLockStatus> TicketDeleteLockStatus { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<AgentCategoryAssigned> AgentCategoryAssigned { get; set; }
        public DbSet<Knowledgebase> Knowledgebase { get; set; }
        public DbSet<KnowledgebaseAttachments> KnowledgebaseAttachments { get; set; }
        public DbSet<KnowledgebaseDetails> KnowledgebaseDetails { get; set; }
        public DbSet<KnowledgebaseType> KnowledgebaseType { get; set; }
        public DbSet<HolidayList> HolidayList { get; set; }
        public DbSet<BusinessHoursType> BusinessHoursType { get; set; }
        public DbSet<BusinessHours> BusinessHours { get; set; }
        public DbSet<BusinessHoursDetails> BusinessHoursDetails { get; set; }
        public DbSet<SlaPolicies> SlaPolicies { get; set; }
        public DbSet<CategoryConfigration> CategoryConfigration { get; set; }
        public DbSet<DefaultTicketSettings> DefaultTicketSettings { get; set; }
        public DbSet<TicketEscalationHistory> TicketEscalationHistory { get; set; }
        public DbSet<RegisterVerification> RegisterVerification { get; set; }
        public DbSet<ResetPasswordVerification> ResetPasswordVerification { get; set; }
        public DbSet<OverdueTypes> OverdueTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuCategory>().ToTable("MenuCategory");
            modelBuilder.Entity<MenuMaster>().ToTable("MenuMaster");
            modelBuilder.Entity<SubMenuMaster>().ToTable("SubMenuMaster");
            modelBuilder.Entity<Role>().ToTable("RoleMaster");
            modelBuilder.Entity<Usermaster>().ToTable("Usermaster");
            modelBuilder.Entity<PasswordMaster>().ToTable("PasswordMaster");
            modelBuilder.Entity<UserTokens>().ToTable("UserTokens");
            modelBuilder.Entity<SavedAssignedRoles>().ToTable("SavedAssignedRoles");
            modelBuilder.Entity<Category>().ToTable("CategoryMaster");
            modelBuilder.Entity<Priority>().ToTable("Priority");
            modelBuilder.Entity<Tickets>().ToTable("Tickets");
            modelBuilder.Entity<TicketDetails>().ToTable("TicketDetails");
            modelBuilder.Entity<Attachments>().ToTable("Attachments");
            modelBuilder.Entity<AttachmentDetails>().ToTable("AttachmentDetails");
            modelBuilder.Entity<TicketStatus>().ToTable("TicketStatus");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<TicketReply>().ToTable("TicketReply");
            modelBuilder.Entity<TicketReplyDetails>().ToTable("TicketReplyDetails");
            modelBuilder.Entity<ReplyAttachment>().ToTable("ReplyAttachment");
            modelBuilder.Entity<ReplyAttachmentDetails>().ToTable("ReplyAttachmentDetails");
            modelBuilder.Entity<AgentCheckInStatusSummary>().ToTable("AgentCheckInStatusSummary");
            modelBuilder.Entity<AgentCheckInStatusDetails>().ToTable("AgentCheckInStatusDetails");
            modelBuilder.Entity<TicketHistory>().ToTable("TicketHistory");
            modelBuilder.Entity<ProfileImage>().ToTable("ProfileImage");
            modelBuilder.Entity<ProfileImageStatus>().ToTable("ProfileImageStatus");
            modelBuilder.Entity<Signatures>().ToTable("Signatures");
            modelBuilder.Entity<SmtpEmailSettings>().ToTable("SmtpEmailSettings");
            modelBuilder.Entity<TicketDeleteLockStatus>().ToTable("TicketDeleteLockStatus");
            modelBuilder.Entity<GeneralSettings>().ToTable("GeneralSettings");
            modelBuilder.Entity<AgentCategoryAssigned>().ToTable("AgentCategoryAssigned");
            modelBuilder.Entity<Knowledgebase>().ToTable("Knowledgebase");
            modelBuilder.Entity<KnowledgebaseAttachments>().ToTable("KnowledgebaseAttachments");
            modelBuilder.Entity<KnowledgebaseDetails>().ToTable("KnowledgebaseDetails");
            modelBuilder.Entity<KnowledgebaseType>().ToTable("KnowledgebaseType");
            modelBuilder.Entity<HolidayList>().ToTable("HolidayList");
            modelBuilder.Entity<BusinessHoursType>().ToTable("BusinessHoursType");
            modelBuilder.Entity<BusinessHours>().ToTable("BusinessHours");
            modelBuilder.Entity<BusinessHoursDetails>().ToTable("BusinessHoursDetails");
            modelBuilder.Entity<SlaPolicies>().ToTable("SlaPolicies");
            modelBuilder.Entity<CategoryConfigration>().ToTable("CategoryConfigration");
            modelBuilder.Entity<DefaultTicketSettings>().ToTable("DefaultTicketSettings");
            modelBuilder.Entity<TicketEscalationHistory>().ToTable("TicketEscalationHistory");
            modelBuilder.Entity<RegisterVerification>().ToTable("RegisterVerification");
            modelBuilder.Entity<ResetPasswordVerification>().ToTable("ResetPasswordVerification");
            modelBuilder.Entity<OverdueTypes>().ToTable("OverdueTypes");

            base.OnModelCreating(modelBuilder);
        }

    }
        
    public class DatabaseDBInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
           // base.Seed(context);
        }
    }
}