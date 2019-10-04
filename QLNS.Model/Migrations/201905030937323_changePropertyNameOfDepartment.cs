namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePropertyNameOfDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Department", "Priority", c => c.Long());
            DropColumn("dbo.Department", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Department", "Type", c => c.Long());
            DropColumn("dbo.Department", "Priority");
        }
    }
}
