namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDataTypeDepartment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Department", "Type", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Department", "Type", c => c.Long(nullable: false));
        }
    }
}
