namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRecruitmentRequest1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RecruitmentRequest", "SkillGroups", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecruitmentRequest", "SkillGroups", c => c.String(nullable: false));
        }
    }
}
