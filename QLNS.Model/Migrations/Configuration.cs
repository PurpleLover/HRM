namespace QLNS.Model.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using QLNS.Model.IdentityEntities;
    using QLNS.Model.Seed;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QLNS.Model.QLNSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(QLNS.Model.QLNSContext context)
        {
            InitAccountSeed.init(context);
            InitDanhMucSeed.Init(context);
        }
    }
}
