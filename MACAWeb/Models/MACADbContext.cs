using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MACAWeb.Models
{
    public class MACADbContext : DbContext
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonUser> PersonUsers { get; set; }
        public DbSet<PublicationAuthor> PublicationAuthors { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorType> AuthorTypes { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }
        public DbSet<GrantBudget> GrantBudgets { get; set; }
        public DbSet<GrantBudgetsType> GrantBudgetTypes { get; set; }
        public DbSet<GrantMember> GrantMembers { get; set; }
        public DbSet<Grant> Grants { get; set; }
        public DbSet<GrantStatus> GrantStatuses { get; set; }
        public DbSet<GrantMemberType> GrantMemberTypes { get; set; }
        public DbSet<Mentorship> Mentorships { get; set; }
        public DbSet<ThesisType> ThesisTypes { get; set; }
        public DbSet<MentorshipType> MentorshipTypes { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionType> PositionTypes { get; set; }
        public DbSet<PublicationClassificationCoefficient> PublicationClassificationCoefficients { get; set; }
        public DbSet<PublicationClassification> PublicationClassifications { get; set; }
        public DbSet<PublicationTypeGroup> PublicationTypeGroups { get; set; }
        public DbSet<PublicationType> PublicationTypes { get; set; }
        public DbSet<PublicationTypeLocal> PublicationTypesLocal { get; set; }
        public DbSet<PublicationStatus> PublicationStatus { get; set; }
        public DbSet<PublicationTypeLocalCoef> PublicationTypesLocalCoef { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<SocialLinkType> SocialLinkTypes { get; set; }
        public DbSet<StudyLevel> StudyLevels { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teaching> Teachings { get; set; }
        public DbSet<TeachingType> TeachingTypes { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        
        public DbSet<Interest> Interests { get; set; }
        public DbSet<ConferenceTalk> ConferenceTalks { get; set; }
        public DbSet<SeminarTalk> SeminarTalks { get; set; }
  
        public MACADbContext() : base("MACA") { }
    }
}