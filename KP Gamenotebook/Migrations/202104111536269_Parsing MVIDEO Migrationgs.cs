namespace KP_Gamenotebook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParsingMVIDEOMigrationgs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CPU",
                c => new
                    {
                        IDCPU = c.Int(name: "ID CPU", nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Countcores = c.Int(name: "Count cores"),
                        Frequency = c.String(),
                    })
                .PrimaryKey(t => t.IDCPU);
            
            CreateTable(
                "dbo.Model",
                c => new
                    {
                        IDmodel = c.Int(name: "ID model", nullable: false),
                        Name = c.String(nullable: false),
                        Href = c.String(nullable: false),
                        Price = c.String(nullable: false),
                        Bonuses = c.String(maxLength: 50),
                        Rating = c.String(),
                        SSD = c.String(nullable: false),
                        RAM = c.Int(nullable: false),
                        IDfirm = c.Int(name: "ID firm"),
                        ID_GC = c.Int(),
                        IDCPU = c.Int(name: "ID CPU"),
                    })
                .PrimaryKey(t => t.IDmodel)
                .ForeignKey("dbo.Firm", t => t.IDfirm, cascadeDelete: true)
                .ForeignKey("dbo.Graphiccard", t => t.ID_GC, cascadeDelete: true)
                .ForeignKey("dbo.CPU", t => t.IDCPU, cascadeDelete: true)
                .Index(t => t.IDfirm)
                .Index(t => t.ID_GC)
                .Index(t => t.IDCPU);
            
            CreateTable(
                "dbo.Firm",
                c => new
                    {
                        IDfirm = c.Int(name: "ID firm", nullable: false, identity: true),
                        Namefirm = c.String(name: "Name firm", nullable: false),
                    })
                .PrimaryKey(t => t.IDfirm);
            
            CreateTable(
                "dbo.Graphiccard",
                c => new
                    {
                        ID_GC = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID_GC);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        IDreview = c.Int(name: "ID review", nullable: false, identity: true),
                        Rating = c.String(nullable: false, maxLength: 50),
                        Reviewtext = c.String(name: "Review text"),
                        IDmodel = c.Int(name: "ID model"),
                    })
                .PrimaryKey(t => t.IDreview)
                .ForeignKey("dbo.Model", t => t.IDmodel, cascadeDelete: true)
                .Index(t => t.IDmodel);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Model", "ID CPU", "dbo.CPU");
            DropForeignKey("dbo.Reviews", "ID model", "dbo.Model");
            DropForeignKey("dbo.Model", "ID_GC", "dbo.Graphiccard");
            DropForeignKey("dbo.Model", "ID firm", "dbo.Firm");
            DropIndex("dbo.Reviews", new[] { "ID model" });
            DropIndex("dbo.Model", new[] { "ID CPU" });
            DropIndex("dbo.Model", new[] { "ID_GC" });
            DropIndex("dbo.Model", new[] { "ID firm" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.Reviews");
            DropTable("dbo.Graphiccard");
            DropTable("dbo.Firm");
            DropTable("dbo.Model");
            DropTable("dbo.CPU");
        }
    }
}
