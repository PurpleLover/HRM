using Autofac;
using AutoMapper;
using AutoMapper.Configuration;
using QLNS.Model.Entities;
using QLNS.Service.TD_HoSoUngVienService.DTO;
using QLNS.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLNS.Web.Core;
using QLNS.Service.RecruitmentSkillService.DTO;

namespace QLNS.Web.Modules
{
    public class AutoMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
                cfg.IgnoreUnmapped();
                cfg.CreateMap<DM_DulieuDanhmuc, SelectListItem>()
                       .ForMember(x => x.Text, y => y.MapFrom(x => x.Name.ToString()))
                       .ForMember(x => x.Value, y => y.MapFrom(x => x.Id.ToString()));
                cfg.CreateMap<RecruitmentSkill, RecruitmentSkillDTO>();
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}