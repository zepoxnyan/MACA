namespace MACAWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityID = c.Guid(nullable: false),
                        ActivityTypeID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        Weight = c.Double(nullable: false),
                        Remark = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityID)
                .ForeignKey("dbo.ActivityTypes", t => t.ActivityTypeID, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.ActivityTypeID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        ActivityTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Year = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityTypeID);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonID = c.Guid(nullable: false),
                        Surname = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        FullName = c.String(nullable: false),
                        Email = c.String(),
                        Description = c.String(),
                        AuthorID = c.Guid(),
                        AISID = c.String(),
                        Image = c.Binary(),
                        ImageThumb = c.Binary(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.String(maxLength: 128),
                        UserModifiedID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PersonID);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorID = c.Guid(nullable: false),
                        Surname = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        CREPCCode = c.String(),
                        ORCID = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            CreateTable(
                "dbo.AuthorTypes",
                c => new
                    {
                        AuthorTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorTypeID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Guid(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        EventTypeID = c.Guid(nullable: false),
                        Code = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.EventTypeID);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        FaqID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Question = c.String(nullable: false),
                        Answer = c.String(nullable: false),
                        Author = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.FaqID);
            
            CreateTable(
                "dbo.FeatureTypes",
                c => new
                    {
                        FeatureTypeId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedId = c.Guid(nullable: false),
                        UserModifiedId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.FeatureTypeId);
            
            CreateTable(
                "dbo.GrantBudgets",
                c => new
                    {
                        GrantBudgetID = c.Guid(nullable: false),
                        GrantID = c.Guid(nullable: false),
                        GrantBudgetsTypeID = c.Guid(nullable: false),
                        Amount = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GrantBudgetID)
                .ForeignKey("dbo.GrantBudgetsTypes", t => t.GrantBudgetsTypeID, cascadeDelete: true)
                .Index(t => t.GrantBudgetsTypeID);
            
            CreateTable(
                "dbo.GrantBudgetsTypes",
                c => new
                    {
                        GrantBudgetsTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GrantBudgetsTypeID);
            
            CreateTable(
                "dbo.GrantMembers",
                c => new
                    {
                        GrantMemberID = c.Guid(nullable: false),
                        GrantID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        GrantMemberTypeID = c.Guid(nullable: false),
                        Hours = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GrantMemberID)
                .ForeignKey("dbo.GrantMemberTypes", t => t.GrantMemberTypeID, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID)
                .Index(t => t.GrantMemberTypeID);
            
            CreateTable(
                "dbo.GrantMemberTypes",
                c => new
                    {
                        GrantMemberTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Coefficient = c.Double(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GrantMemberTypeID);
            
            CreateTable(
                "dbo.Grants",
                c => new
                    {
                        GrantID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        GrantStatusID = c.Guid(nullable: false),
                        Description = c.String(),
                        Start = c.Int(nullable: false),
                        End = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GrantID)
                .ForeignKey("dbo.GrantStatus", t => t.GrantStatusID, cascadeDelete: true)
                .Index(t => t.GrantStatusID);
            
            CreateTable(
                "dbo.GrantStatus",
                c => new
                    {
                        GrantStatusID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GrantStatusID);
            
            CreateTable(
                "dbo.Mentorships",
                c => new
                    {
                        MentorshipID = c.Guid(nullable: false),
                        ThesisTypeID = c.Guid(nullable: false),
                        MentorshipTypeID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        Student = c.String(nullable: false),
                        ThesisTitle = c.String(nullable: false),
                        Remarks = c.String(),
                        Year = c.String(nullable: false),
                        Semester = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MentorshipID)
                .ForeignKey("dbo.MentorshipTypes", t => t.MentorshipTypeID, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .ForeignKey("dbo.ThesisTypes", t => t.ThesisTypeID, cascadeDelete: true)
                .Index(t => t.ThesisTypeID)
                .Index(t => t.MentorshipTypeID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.MentorshipTypes",
                c => new
                    {
                        MentorshipTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        AISCode = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MentorshipTypeID);
            
            CreateTable(
                "dbo.ThesisTypes",
                c => new
                    {
                        ThesisTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        AISCode = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ThesisTypeID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Abstract = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        Source = c.String(),
                        SourceLink = c.String(),
                        DatePublished = c.DateTime(nullable: false),
                        Image = c.Binary(),
                        ImageAuthor = c.String(),
                        ImageDescription = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.String(maxLength: 128),
                        UserModifiedID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NewsID);
            
            CreateTable(
                "dbo.PersonUsers",
                c => new
                    {
                        PersonUserID = c.Guid(nullable: false),
                        UserID = c.String(maxLength: 128),
                        PersonID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PersonUserID);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        PositionTypeID = c.Guid(nullable: false),
                        Description = c.String(),
                        Year = c.Int(nullable: false),
                        Semester = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID)
                .ForeignKey("dbo.PositionTypes", t => t.PositionTypeID, cascadeDelete: true)
                .Index(t => t.PositionTypeID);
            
            CreateTable(
                "dbo.PositionTypes",
                c => new
                    {
                        PositionTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PositionTypeID);
            
            CreateTable(
                "dbo.PublicationAuthors",
                c => new
                    {
                        PublicationAuthorID = c.Guid(nullable: false),
                        AuthorID = c.Guid(nullable: false),
                        PublicationID = c.Guid(nullable: false),
                        AuthorTypeID = c.Guid(nullable: false),
                        Percent = c.Double(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationAuthorID)
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.AuthorTypes", t => t.AuthorTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Publications", t => t.PublicationID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.PublicationID)
                .Index(t => t.AuthorTypeID);
            
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        PublicationID = c.Guid(nullable: false),
                        PublicationTypeID = c.Guid(nullable: false),
                        PublicationTypeLocalID = c.Guid(nullable: false),
                        PublicationClassificationID = c.Guid(nullable: false),
                        PublicationStatusID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Journal = c.String(),
                        Year = c.Int(nullable: false),
                        Volume = c.Int(),
                        Issue = c.Int(),
                        Pages = c.String(),
                        DOI = c.String(),
                        Link = c.String(),
                        PreprintLink = c.String(),
                        Note = c.String(),
                        Editors = c.String(),
                        Publisher = c.String(),
                        Series = c.String(),
                        Address = c.String(),
                        Edition = c.String(),
                        BookTitle = c.String(),
                        Organization = c.String(),
                        Chapter = c.String(),
                        Keywords = c.String(),
                        KeywordsEN = c.String(),
                        TitleEN = c.String(),
                        Abstract = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationID)
                .ForeignKey("dbo.PublicationClassifications", t => t.PublicationClassificationID, cascadeDelete: true)
                .ForeignKey("dbo.PublicationStatus", t => t.PublicationStatusID, cascadeDelete: true)
                .ForeignKey("dbo.PublicationTypes", t => t.PublicationTypeID, cascadeDelete: true)
                .ForeignKey("dbo.PublicationTypeLocals", t => t.PublicationTypeLocalID, cascadeDelete: true)
                .Index(t => t.PublicationTypeID)
                .Index(t => t.PublicationTypeLocalID)
                .Index(t => t.PublicationClassificationID)
                .Index(t => t.PublicationStatusID);
            
            CreateTable(
                "dbo.PublicationClassifications",
                c => new
                    {
                        PublicationClassificationID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationClassificationID);
            
            CreateTable(
                "dbo.PublicationStatus",
                c => new
                    {
                        PublicationStatusID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationStatusID);
            
            CreateTable(
                "dbo.PublicationTypes",
                c => new
                    {
                        PublicationTypeID = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationTypeID);
            
            CreateTable(
                "dbo.PublicationTypeLocals",
                c => new
                    {
                        PublicationTypeLocalID = c.Guid(nullable: false),
                        PublicationTypeGroupID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationTypeLocalID)
                .ForeignKey("dbo.PublicationTypeGroups", t => t.PublicationTypeGroupID, cascadeDelete: true)
                .Index(t => t.PublicationTypeGroupID);
            
            CreateTable(
                "dbo.PublicationTypeGroups",
                c => new
                    {
                        PublicationTypeGroupID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationTypeGroupID);
            
            CreateTable(
                "dbo.PublicationClassificationCoefficients",
                c => new
                    {
                        PublicationClassificationCoefficientID = c.Guid(nullable: false),
                        PublicationClassificationID = c.Guid(nullable: false),
                        PublicationTypeGroupID = c.Guid(nullable: false),
                        Coefficient = c.Double(nullable: false),
                        Year = c.Int(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationClassificationCoefficientID)
                .ForeignKey("dbo.PublicationClassifications", t => t.PublicationClassificationID, cascadeDelete: true)
                .ForeignKey("dbo.PublicationTypeGroups", t => t.PublicationTypeGroupID, cascadeDelete: true)
                .Index(t => t.PublicationClassificationID)
                .Index(t => t.PublicationTypeGroupID);
            
            CreateTable(
                "dbo.PublicationTypeLocalCoefs",
                c => new
                    {
                        PublicationTypeLocalCoefID = c.Guid(nullable: false),
                        PublicationTypeLocalID = c.Guid(nullable: false),
                        Year = c.Int(nullable: false),
                        Coefficient = c.Double(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.PublicationTypeLocalCoefID)
                .ForeignKey("dbo.PublicationTypeLocals", t => t.PublicationTypeLocalID, cascadeDelete: true)
                .Index(t => t.PublicationTypeLocalID);
            
            CreateTable(
                "dbo.SocialLinks",
                c => new
                    {
                        SocialLinkID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        SocialLinkTypeID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.SocialLinkID);
            
            CreateTable(
                "dbo.SocialLinkTypes",
                c => new
                    {
                        SocialLinkTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        UrlShortcut = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.SocialLinkTypeID);
            
            CreateTable(
                "dbo.StudyLevels",
                c => new
                    {
                        StudyLevelID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.StudyLevelID);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Year = c.String(nullable: false),
                        Semester = c.Int(nullable: false),
                        AISCode = c.String(nullable: false),
                        ShortName = c.String(nullable: false),
                        Department = c.String(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.SubjectID);
            
            CreateTable(
                "dbo.Teachings",
                c => new
                    {
                        TeachingID = c.Guid(nullable: false),
                        TeachingTypeID = c.Guid(nullable: false),
                        SubjectID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        Hours = c.Double(nullable: false),
                        Remark = c.String(),
                        Weight = c.Double(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TeachingID)
                .ForeignKey("dbo.People", t => t.PersonID, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .ForeignKey("dbo.TeachingTypes", t => t.TeachingTypeID, cascadeDelete: true)
                .Index(t => t.TeachingTypeID)
                .Index(t => t.SubjectID)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.TeachingTypes",
                c => new
                    {
                        TeachingTypeID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        AISCode = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.Guid(nullable: false),
                        UserModifiedID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.TeachingTypeID);
            
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        TeamMemberID = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Affiliation = c.String(),
                        HomepageLink = c.String(),
                        Image = c.Binary(nullable: false),
                        PagePosition = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        UserCreatedID = c.String(maxLength: 128),
                        UserModifiedID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.TeamMemberID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachings", "TeachingTypeID", "dbo.TeachingTypes");
            DropForeignKey("dbo.Teachings", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Teachings", "PersonID", "dbo.People");
            DropForeignKey("dbo.PublicationTypeLocalCoefs", "PublicationTypeLocalID", "dbo.PublicationTypeLocals");
            DropForeignKey("dbo.PublicationClassificationCoefficients", "PublicationTypeGroupID", "dbo.PublicationTypeGroups");
            DropForeignKey("dbo.PublicationClassificationCoefficients", "PublicationClassificationID", "dbo.PublicationClassifications");
            DropForeignKey("dbo.PublicationAuthors", "PublicationID", "dbo.Publications");
            DropForeignKey("dbo.Publications", "PublicationTypeLocalID", "dbo.PublicationTypeLocals");
            DropForeignKey("dbo.PublicationTypeLocals", "PublicationTypeGroupID", "dbo.PublicationTypeGroups");
            DropForeignKey("dbo.Publications", "PublicationTypeID", "dbo.PublicationTypes");
            DropForeignKey("dbo.Publications", "PublicationStatusID", "dbo.PublicationStatus");
            DropForeignKey("dbo.Publications", "PublicationClassificationID", "dbo.PublicationClassifications");
            DropForeignKey("dbo.PublicationAuthors", "AuthorTypeID", "dbo.AuthorTypes");
            DropForeignKey("dbo.PublicationAuthors", "AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Positions", "PositionTypeID", "dbo.PositionTypes");
            DropForeignKey("dbo.Mentorships", "ThesisTypeID", "dbo.ThesisTypes");
            DropForeignKey("dbo.Mentorships", "PersonID", "dbo.People");
            DropForeignKey("dbo.Mentorships", "MentorshipTypeID", "dbo.MentorshipTypes");
            DropForeignKey("dbo.Grants", "GrantStatusID", "dbo.GrantStatus");
            DropForeignKey("dbo.GrantMembers", "PersonID", "dbo.People");
            DropForeignKey("dbo.GrantMembers", "GrantMemberTypeID", "dbo.GrantMemberTypes");
            DropForeignKey("dbo.GrantBudgets", "GrantBudgetsTypeID", "dbo.GrantBudgetsTypes");
            DropForeignKey("dbo.Activities", "PersonID", "dbo.People");
            DropForeignKey("dbo.Activities", "ActivityTypeID", "dbo.ActivityTypes");
            DropIndex("dbo.Teachings", new[] { "PersonID" });
            DropIndex("dbo.Teachings", new[] { "SubjectID" });
            DropIndex("dbo.Teachings", new[] { "TeachingTypeID" });
            DropIndex("dbo.PublicationTypeLocalCoefs", new[] { "PublicationTypeLocalID" });
            DropIndex("dbo.PublicationClassificationCoefficients", new[] { "PublicationTypeGroupID" });
            DropIndex("dbo.PublicationClassificationCoefficients", new[] { "PublicationClassificationID" });
            DropIndex("dbo.PublicationTypeLocals", new[] { "PublicationTypeGroupID" });
            DropIndex("dbo.Publications", new[] { "PublicationStatusID" });
            DropIndex("dbo.Publications", new[] { "PublicationClassificationID" });
            DropIndex("dbo.Publications", new[] { "PublicationTypeLocalID" });
            DropIndex("dbo.Publications", new[] { "PublicationTypeID" });
            DropIndex("dbo.PublicationAuthors", new[] { "AuthorTypeID" });
            DropIndex("dbo.PublicationAuthors", new[] { "PublicationID" });
            DropIndex("dbo.PublicationAuthors", new[] { "AuthorID" });
            DropIndex("dbo.Positions", new[] { "PositionTypeID" });
            DropIndex("dbo.Mentorships", new[] { "PersonID" });
            DropIndex("dbo.Mentorships", new[] { "MentorshipTypeID" });
            DropIndex("dbo.Mentorships", new[] { "ThesisTypeID" });
            DropIndex("dbo.Grants", new[] { "GrantStatusID" });
            DropIndex("dbo.GrantMembers", new[] { "GrantMemberTypeID" });
            DropIndex("dbo.GrantMembers", new[] { "PersonID" });
            DropIndex("dbo.GrantBudgets", new[] { "GrantBudgetsTypeID" });
            DropIndex("dbo.Activities", new[] { "PersonID" });
            DropIndex("dbo.Activities", new[] { "ActivityTypeID" });
            DropTable("dbo.TeamMembers");
            DropTable("dbo.TeachingTypes");
            DropTable("dbo.Teachings");
            DropTable("dbo.Subjects");
            DropTable("dbo.StudyLevels");
            DropTable("dbo.SocialLinkTypes");
            DropTable("dbo.SocialLinks");
            DropTable("dbo.PublicationTypeLocalCoefs");
            DropTable("dbo.PublicationClassificationCoefficients");
            DropTable("dbo.PublicationTypeGroups");
            DropTable("dbo.PublicationTypeLocals");
            DropTable("dbo.PublicationTypes");
            DropTable("dbo.PublicationStatus");
            DropTable("dbo.PublicationClassifications");
            DropTable("dbo.Publications");
            DropTable("dbo.PublicationAuthors");
            DropTable("dbo.PositionTypes");
            DropTable("dbo.Positions");
            DropTable("dbo.PersonUsers");
            DropTable("dbo.News");
            DropTable("dbo.ThesisTypes");
            DropTable("dbo.MentorshipTypes");
            DropTable("dbo.Mentorships");
            DropTable("dbo.GrantStatus");
            DropTable("dbo.Grants");
            DropTable("dbo.GrantMemberTypes");
            DropTable("dbo.GrantMembers");
            DropTable("dbo.GrantBudgetsTypes");
            DropTable("dbo.GrantBudgets");
            DropTable("dbo.FeatureTypes");
            DropTable("dbo.FAQs");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Events");
            DropTable("dbo.AuthorTypes");
            DropTable("dbo.Authors");
            DropTable("dbo.People");
            DropTable("dbo.ActivityTypes");
            DropTable("dbo.Activities");
        }
    }
}
