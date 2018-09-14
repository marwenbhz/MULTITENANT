namespace DATA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class toAzure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banques",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        BanqueName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        ProjectName = c.String(),
                        CategoryCode = c.Int(),
                        CompanyCode = c.Int(),
                        BanqueCode = c.Int(),
                        Description = c.String(),
                        Plan = c.String(),
                        Goals = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                        EstimatedTime = c.String(),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Banques", t => t.BanqueCode)
                .ForeignKey("dbo.Categories", t => t.CategoryCode)
                .ForeignKey("dbo.Companies", t => t.CompanyCode)
                .Index(t => t.CategoryCode)
                .Index(t => t.CompanyCode)
                .Index(t => t.BanqueCode);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        CompanyName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Country = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        ProjectCode = c.Int(nullable: false),
                        TeamLeaderCode = c.Int(),
                        TaskName = c.String(),
                        Description = c.String(),
                        Plan = c.String(),
                        Goals = c.String(),
                        Requirement = c.String(),
                        Tools = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        DeadLine = c.DateTime(nullable: false),
                        EstimatedTime = c.String(),
                        Complexity = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TaskID)
                .ForeignKey("dbo.Users", t => t.TeamLeaderCode)
                .ForeignKey("dbo.Projects", t => t.ProjectCode, cascadeDelete: true)
                .Index(t => t.ProjectCode)
                .Index(t => t.TeamLeaderCode);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        FullName = c.String(),
                        Bio = c.String(),
                        Country = c.Int(nullable: false),
                        UPN = c.String(),
                        Username = c.String(),
                        UserType = c.Int(nullable: false),
                        TenantID = c.String(),
                        TenantCode = c.Int(),
                        TeamCode = c.Int(),
                        TaskCode = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Teams", t => t.TeamCode)
                .ForeignKey("dbo.Tenants", t => t.TenantCode)
                .ForeignKey("dbo.Tasks", t => t.TaskCode)
                .Index(t => t.TenantCode)
                .Index(t => t.TeamCode)
                .Index(t => t.TaskCode);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        Owner = c.String(),
                        TeamName = c.String(),
                        TeamDescription = c.String(),
                    })
                .PrimaryKey(t => t.TeamID);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IssValue = c.String(),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        AdminConsented = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ProjectCode", "dbo.Projects");
            DropForeignKey("dbo.Users", "TaskCode", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "TeamLeaderCode", "dbo.Users");
            DropForeignKey("dbo.Users", "TenantCode", "dbo.Tenants");
            DropForeignKey("dbo.Users", "TeamCode", "dbo.Teams");
            DropForeignKey("dbo.Projects", "CompanyCode", "dbo.Companies");
            DropForeignKey("dbo.Projects", "CategoryCode", "dbo.Categories");
            DropForeignKey("dbo.Projects", "BanqueCode", "dbo.Banques");
            DropIndex("dbo.Users", new[] { "TaskCode" });
            DropIndex("dbo.Users", new[] { "TeamCode" });
            DropIndex("dbo.Users", new[] { "TenantCode" });
            DropIndex("dbo.Tasks", new[] { "TeamLeaderCode" });
            DropIndex("dbo.Tasks", new[] { "ProjectCode" });
            DropIndex("dbo.Projects", new[] { "BanqueCode" });
            DropIndex("dbo.Projects", new[] { "CompanyCode" });
            DropIndex("dbo.Projects", new[] { "CategoryCode" });
            DropTable("dbo.Tenants");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
            DropTable("dbo.Companies");
            DropTable("dbo.Categories");
            DropTable("dbo.Projects");
            DropTable("dbo.Banques");
        }
    }
}
