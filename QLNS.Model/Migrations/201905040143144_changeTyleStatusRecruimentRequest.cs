namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTyleStatusRecruimentRequest : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RecruitmentRequest", "Status", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecruitmentRequest", "Status", c => c.Int(nullable: false));
        }
    }
}
