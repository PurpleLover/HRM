namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRecruitmentRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecruitmentRequest", "TemplateId", c => c.Long());
            AddColumn("dbo.RecruitmentRequest", "IsTemplate", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecruitmentRequest", "IsTemplate");
            DropColumn("dbo.RecruitmentRequest", "TemplateId");
        }
    }
}
