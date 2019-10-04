namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePropertyOfQuanlyMauTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TD_CauHinhSangLocUngVien", "InterviewerName", c => c.String());
            AddColumn("dbo.TD_QuanLyMauTest", "Size", c => c.Long());
            AlterColumn("dbo.TD_CauHinhSangLocUngVien", "Type", c => c.String(nullable: false));
            AlterColumn("dbo.TD_QuanLyMauTest", "Category", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TD_QuanLyMauTest", "Category", c => c.Long());
            AlterColumn("dbo.TD_CauHinhSangLocUngVien", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.TD_QuanLyMauTest", "Size");
            DropColumn("dbo.TD_CauHinhSangLocUngVien", "InterviewerName");
        }
    }
}
