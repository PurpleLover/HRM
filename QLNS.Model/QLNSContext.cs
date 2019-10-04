using Microsoft.AspNet.Identity.EntityFramework;
using QLNS.Model.Entities;
using QLNS.Model.IdentityEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLNS.Model
{
    public class QLNSContext : IdentityDbContext<AppUser, AppRole, long, AppLogin, AppUserRole, AppClaim>
    {

        public QLNSContext()
            : base("Name=QLNSContext")
        {
            //sử dụng cho việc unit test
            Database.SetInitializer<QLNSContext>(null);
        }
        public DbSet<Audit> Audit { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<Operation> Operation { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleOperation> RoleOperation { get; set; }
        public DbSet<DM_DulieuDanhmuc> DM_DulieuDanhmuc { get; set; }
        public DbSet<DM_NhomDanhmuc> DM_NhomDanhmuc { get; set; }
        public DbSet<CommonConfiguration> CommonConfiguration { get; set; }
        public DbSet<RecruitmentRequest> RecruitmentRequest { get; set; }
        public DbSet<RecruitmentSkill> RecruitmentSkill { get; set; }
        public DbSet<RecruitmentSkillDetail> RecruitmentSkillDetail { get; set; }
        public DbSet<ConfigRecruitmentRequest> ConfigRecruitmentRequest { get; set; }
        public DbSet<TD_HoSoUngVien> TD_HoSoUngVien { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<TaiLieuDinhKem> TaiLieuDinhKem { get; set; }
        public DbSet<TD_DotTuyenDung> TD_DotTuyenDung { get; set; }
        public DbSet<TD_YeuCauVaDotTuyenDung> TD_YeuCauVaDotTuyenDung { get; set; }
        public DbSet<TD_KetQuaPhongVan> TD_KetQuaPhongVan { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<TD_QuanLyMauTest> TD_QuanLyMauTest { get; set; }
        public DbSet<TD_CauHinhSangLocUngVien> TD_CauHinhSangLocUngVien { get; set; }
        public static QLNSContext Create()
        {
            return new QLNSContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().ToTable("AppUser");
            modelBuilder.Entity<AppUserRole>().ToTable("AppUserRole");
            modelBuilder.Entity<AppRole>().ToTable("AppRole");
            modelBuilder.Entity<AppClaim>().ToTable("AppClaim");
            modelBuilder.Entity<AppLogin>().ToTable("AppLogin");

            modelBuilder.Entity<AppUser>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AppRole>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AppClaim>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identityName = Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }
    }
}
