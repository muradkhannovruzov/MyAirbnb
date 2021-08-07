namespace AirbnbServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        Image_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Images", t => t.Image_Id)
                .Index(t => t.Id)
                .Index(t => t.Image_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Photos = c.Binary(storeType: "image"),
                        Home_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Homes", t => t.Home_Id)
                .Index(t => t.Id)
                .Index(t => t.Home_Id);
            
            CreateTable(
                "dbo.Messagings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account1ID = c.Int(nullable: false),
                        Account2ID = c.Int(nullable: false),
                        Account1Name = c.String(),
                        Account2Name = c.String(),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account1Message = c.String(),
                        Account2Message = c.String(),
                        Account1Readed = c.Boolean(nullable: false),
                        Account2Readed = c.Boolean(nullable: false),
                        Time = c.String(),
                        Account1Background = c.String(),
                        Account2Background = c.String(),
                        Messaging_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messagings", t => t.Messaging_Id)
                .Index(t => t.Id)
                .Index(t => t.Messaging_Id);
            
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Review = c.Int(nullable: false),
                        AccountMail = c.String(),
                        Number = c.String(),
                        Home_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Homes", t => t.Home_Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.AccountId)
                .Index(t => t.Home_Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountName = c.String(),
                        AccountId = c.Int(nullable: false),
                        Vote = c.Int(nullable: false),
                        PublicationID = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publications", t => t.PublicationID, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.PublicationID);
            
            CreateTable(
                "dbo.Homes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        SelectedImage = c.String(),
                        SelectedImageIndex = c.Int(nullable: false),
                        TheQuestion = c.String(),
                        HomeType = c.Int(nullable: false),
                        PlaceType = c.Int(nullable: false),
                        AdultsCount = c.Int(nullable: false),
                        ChildrenCount = c.Int(nullable: false),
                        InfantCount = c.Int(nullable: false),
                        BedrommsCount = c.Int(nullable: false),
                        BathrommsCount = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        lon = c.String(),
                        lan = c.String(),
                        City_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.City_Id)
                .Index(t => t.Id)
                .Index(t => t.City_Id);
            
            CreateTable(
                "dbo.Amenities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Icon = c.String(),
                        Home_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Homes", t => t.Home_Id)
                .Index(t => t.Id)
                .Index(t => t.Home_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        AccountRezvId = c.Int(nullable: false),
                        PublicationId = c.Int(nullable: false),
                        Account_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publications", t => t.PublicationId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.Account_Id)
                .Index(t => t.Id)
                .Index(t => t.PublicationId)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Reservations", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Publications", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Reservations", "PublicationId", "dbo.Publications");
            DropForeignKey("dbo.Publications", "Home_Id", "dbo.Homes");
            DropForeignKey("dbo.Images", "Home_Id", "dbo.Homes");
            DropForeignKey("dbo.Homes", "City_Id", "dbo.Cities");
            DropForeignKey("dbo.Amenities", "Home_Id", "dbo.Homes");
            DropForeignKey("dbo.Comments", "PublicationID", "dbo.Publications");
            DropForeignKey("dbo.Messagings", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Messages", "Messaging_Id", "dbo.Messagings");
            DropForeignKey("dbo.Accounts", "Image_Id", "dbo.Images");
            DropIndex("dbo.Countries", new[] { "Id" });
            DropIndex("dbo.Reservations", new[] { "Account_Id" });
            DropIndex("dbo.Reservations", new[] { "PublicationId" });
            DropIndex("dbo.Reservations", new[] { "Id" });
            DropIndex("dbo.Cities", new[] { "CountryId" });
            DropIndex("dbo.Cities", new[] { "Id" });
            DropIndex("dbo.Amenities", new[] { "Home_Id" });
            DropIndex("dbo.Amenities", new[] { "Id" });
            DropIndex("dbo.Homes", new[] { "City_Id" });
            DropIndex("dbo.Homes", new[] { "Id" });
            DropIndex("dbo.Comments", new[] { "PublicationID" });
            DropIndex("dbo.Comments", new[] { "Id" });
            DropIndex("dbo.Publications", new[] { "Home_Id" });
            DropIndex("dbo.Publications", new[] { "AccountId" });
            DropIndex("dbo.Publications", new[] { "Id" });
            DropIndex("dbo.Messages", new[] { "Messaging_Id" });
            DropIndex("dbo.Messages", new[] { "Id" });
            DropIndex("dbo.Messagings", new[] { "Account_Id" });
            DropIndex("dbo.Messagings", new[] { "Id" });
            DropIndex("dbo.Images", new[] { "Home_Id" });
            DropIndex("dbo.Images", new[] { "Id" });
            DropIndex("dbo.Accounts", new[] { "Image_Id" });
            DropIndex("dbo.Accounts", new[] { "Id" });
            DropTable("dbo.Countries");
            DropTable("dbo.Reservations");
            DropTable("dbo.Cities");
            DropTable("dbo.Amenities");
            DropTable("dbo.Homes");
            DropTable("dbo.Comments");
            DropTable("dbo.Publications");
            DropTable("dbo.Messages");
            DropTable("dbo.Messagings");
            DropTable("dbo.Images");
            DropTable("dbo.Accounts");
        }
    }
}
