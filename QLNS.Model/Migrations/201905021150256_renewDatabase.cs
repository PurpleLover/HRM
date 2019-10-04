namespace QLNS.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renewDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audit",
                c => new
                    {
                        AuditID = c.Guid(nullable: false),
                        SessionID = c.String(),
                        IPAddress = c.String(),
                        UserName = c.String(),
                        URLAccessed = c.String(),
                        TimeAccessed = c.DateTime(nullable: false),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.AuditID);
            
            CreateTable(
                "dbo.CommonConfiguration",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConfigName = c.String(nullable: false, maxLength: 250),
                        ConfigCode = c.String(nullable: false, maxLength: 250),
                        ConfigData = c.String(nullable: false, maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Department",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Code = c.String(nullable: false, maxLength: 250),
                        ParentId = c.Long(),
                        Type = c.Long(nullable: false),
                        Level = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DM_DulieuDanhmuc",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GroupId = c.Long(),
                        Name = c.String(nullable: false, maxLength: 250),
                        Code = c.String(nullable: false, maxLength: 250),
                        Note = c.String(maxLength: 250),
                        Priority = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DM_NhomDanhmuc",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 150),
                        GroupCode = c.String(nullable: false, maxLength: 150),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Module",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 250),
                        Name = c.String(nullable: false, maxLength: 250),
                        Order = c.Int(nullable: false),
                        IsShow = c.Boolean(nullable: false),
                        Icon = c.String(),
                        ClassCss = c.String(maxLength: 250),
                        StyleCss = c.String(maxLength: 250),
                        Link = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Operation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 250),
                        URL = c.String(nullable: false, maxLength: 250),
                        Code = c.String(nullable: false),
                        Css = c.String(maxLength: 250),
                        IsShow = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecruitmentRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        UntilDate = c.DateTime(nullable: false),
                        Title = c.String(nullable: false, maxLength: 250),
                        Comment = c.String(maxLength: 250),
                        ReasonClose = c.String(maxLength: 250),
                        PositionId = c.Int(nullable: false),
                        EstimateQuantity = c.Int(nullable: false),
                        SkillGroups = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecruitmentSkill",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 250),
                        Skills = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecruitmentSkillDetail",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DataType = c.Int(nullable: false),
                        CategoryId = c.Long(),
                        CategoryData = c.String(),
                        UpperNumber = c.Int(),
                        LowerNumber = c.Int(),
                        AbsoluteNumber = c.Int(),
                        TextValue = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Code = c.String(nullable: false, maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleOperation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        OperationId = c.Long(nullable: false),
                        IsAccess = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AppUserRole",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AppRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.TaiLieuDinhKem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        KichThuoc = c.Long(),
                        NgayPhatHanh = c.DateTime(),
                        TenTaiLieu = c.String(nullable: false, maxLength: 500),
                        LoaiTaiLieu = c.String(maxLength: 250),
                        Item_ID = c.Long(),
                        MoTa = c.String(),
                        DuongDanFile = c.String(),
                        DinhDangFile = c.String(maxLength: 250),
                        SoLuongDownload = c.Int(nullable: false),
                        UserId = c.Long(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TD_DotTuyenDung",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenDot = c.String(nullable: false, maxLength: 250),
                        NgayBatDau = c.DateTime(),
                        NgayKetThuc = c.DateTime(),
                        TrangThai = c.String(nullable: false, maxLength: 50),
                        GhiChu = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TD_HoSoUngVien",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IDDotTuyenDung = c.Int(),
                        IdYeuCauTuyenDung = c.Int(),
                        NgayNopHoSo = c.DateTime(nullable: false),
                        HoTen = c.String(nullable: false, maxLength: 250),
                        KenhUngTuyen = c.String(maxLength: 250),
                        GioiTinh = c.String(maxLength: 50),
                        NgaySinh = c.DateTime(),
                        DienThoai = c.String(maxLength: 250),
                        Email = c.String(maxLength: 250),
                        DiaChiHienTai = c.String(maxLength: 500),
                        CMND = c.String(maxLength: 50),
                        NgayCapCMND = c.DateTime(),
                        NoiCapCMND = c.String(maxLength: 500),
                        NoiSinh = c.String(maxLength: 500),
                        DanToc = c.String(maxLength: 100),
                        TonGiao = c.String(maxLength: 100),
                        QuocTich = c.String(maxLength: 100),
                        QueQuan = c.String(maxLength: 100),
                        DiaChiThuongTru = c.String(),
                        XuatThan = c.String(maxLength: 250),
                        ThuNhapGanNhat = c.Decimal(precision: 18, scale: 2),
                        ThuNhapMongMuon = c.Decimal(precision: 18, scale: 2),
                        GhiChu = c.String(),
                        Avatar = c.String(),
                        LyDoUngTuyen = c.String(),
                        NguoiGioiThieu = c.String(maxLength: 250),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TD_KetQuaPhongVan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DiemSo = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TD_YeuCauVaDotTuyenDung",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IdYeuCauTuyenDung = c.Int(nullable: false),
                        IdDotTuyenDung = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppUser",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Email = c.String(maxLength: 256),
                        PhoneNumber = c.String(),
                        BirthDay = c.DateTime(),
                        Gender = c.Int(nullable: false),
                        Address = c.String(),
                        FullName = c.String(maxLength: 250),
                        Avatar = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AppClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AppLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AppUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserRole", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppLogin", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppClaim", "UserId", "dbo.AppUser");
            DropForeignKey("dbo.AppUserRole", "RoleId", "dbo.AppRole");
            DropIndex("dbo.AppLogin", new[] { "UserId" });
            DropIndex("dbo.AppClaim", new[] { "UserId" });
            DropIndex("dbo.AppUser", "UserNameIndex");
            DropIndex("dbo.AppUserRole", new[] { "RoleId" });
            DropIndex("dbo.AppUserRole", new[] { "UserId" });
            DropIndex("dbo.AppRole", "RoleNameIndex");
            DropTable("dbo.AppLogin");
            DropTable("dbo.AppClaim");
            DropTable("dbo.AppUser");
            DropTable("dbo.UserRole");
            DropTable("dbo.TD_YeuCauVaDotTuyenDung");
            DropTable("dbo.TD_KetQuaPhongVan");
            DropTable("dbo.TD_HoSoUngVien");
            DropTable("dbo.TD_DotTuyenDung");
            DropTable("dbo.TaiLieuDinhKem");
            DropTable("dbo.AppUserRole");
            DropTable("dbo.AppRole");
            DropTable("dbo.RoleOperation");
            DropTable("dbo.Role");
            DropTable("dbo.RecruitmentSkillDetail");
            DropTable("dbo.RecruitmentSkill");
            DropTable("dbo.RecruitmentRequest");
            DropTable("dbo.Operation");
            DropTable("dbo.Module");
            DropTable("dbo.DM_NhomDanhmuc");
            DropTable("dbo.DM_DulieuDanhmuc");
            DropTable("dbo.Department");
            DropTable("dbo.CommonConfiguration");
            DropTable("dbo.Audit");
        }
    }
}
