namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initTableTD_QuanLyMauTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TD_QuanLyMauTest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        FileDirectory = c.String(),
                        Category = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TD_QuanLyMauTest");
        }
    }
}
