namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTrangThaiHoSo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TD_HoSoUngVien", "TrangThai", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TD_HoSoUngVien", "TrangThai");
        }
    }
}
