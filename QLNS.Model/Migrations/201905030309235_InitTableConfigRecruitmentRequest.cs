namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTableConfigRecruitmentRequest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConfigRecruitmentRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestId = c.Long(),
                        GroupSkillId = c.Long(),
                        SkillId = c.Long(),
                        CategoryId = c.Long(),
                        CategoryData = c.String(),
                        AbsoluteNumber = c.Int(),
                        TextValue = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConfigRecruitmentRequest");
        }
    }
}
