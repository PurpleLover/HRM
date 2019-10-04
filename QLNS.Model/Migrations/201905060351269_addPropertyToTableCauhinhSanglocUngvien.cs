namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPropertyToTableCauhinhSanglocUngvien : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TD_CauHinhSangLocUngVien", "DotTuyenDungId", c => c.Long());
            AddColumn("dbo.TD_CauHinhSangLocUngVien", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TD_CauHinhSangLocUngVien", "Priority");
            DropColumn("dbo.TD_CauHinhSangLocUngVien", "DotTuyenDungId");
        }
    }
}
