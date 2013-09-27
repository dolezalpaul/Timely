using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AutoMapper;
using Moravia.Timely.Business;
using Moravia.Timely.Controllers;
using SimpleInjector;

namespace Moravia.Timely
{
    public interface IAutoRestMapper
    {
        void Map(Container container);
    }

    public class AutoRestMapper<TEntity, TViewModel> : IAutoRestMapper
        where TEntity : Entity
        where TViewModel : ViewModel, new()
    {
        public Service<TEntity> Service { get; set; }

        public void Map(Container container)
        {
            Mapper.CreateMap<TEntity, int>().ConvertUsing<OutputBusinessConverter<TEntity>>();
            Mapper.CreateMap<int, TEntity>().ConvertUsing<InputBusinessConverter<TEntity>>();

            Mapper.CreateMap<TEntity, TViewModel>()
                .ForMember(dest => dest._sideloads, opt => opt.Ignore());
            Mapper.CreateMap<TViewModel, TEntity>()
                .ConstructUsing(rc =>
                {
                    TViewModel viewModel = rc.SourceValue as TViewModel;
                    return viewModel.id == 0 ?
                        Activator.CreateInstance<TEntity>() :
                        Service.Get(viewModel.id);
                })
                .ForMember(dest => dest.is_resolved, opt => opt.Ignore());
        }
    }

    public static class AutoMapperExtension
    {
        public static Container Container { get; set; }

        public static IMappingExpression<TEntity, TViewModel> IncludeSingle<TEntity, TViewModel, TReference>(
            this IMappingExpression<TEntity, TViewModel> expression, Func<TEntity, TReference> reference)
            where TEntity : Entity
            where TViewModel : ViewModel
            where TReference : Entity
        {
            var service = Container.GetInstance<Service<TReference>>();
            return expression.AfterMap((model, view) =>
            {
                var includable = reference(model);
                var resolved = service.Resolve(includable);
                var mapped = Mapper.Map<ViewModel>(resolved);
                view._sideloads.Add(mapped);
            });
        }

        public static IMappingExpression<TEntity, TViewModel> IncludeMany<TEntity, TViewModel, TReference>(
            this IMappingExpression<TEntity, TViewModel> expression, Func<TEntity, IList<TReference>> references)
            where TEntity : Entity
            where TViewModel : ViewModel
            where TReference : Entity
        {
            var service = Container.GetInstance<Service<TReference>>();
            return expression.AfterMap((model, view) =>
            {
                var includable = references(model);
                foreach (var reference in includable)
                {
                    var resolved = service.Resolve(reference);
                    var mapped = Mapper.Map<ViewModel>(resolved);
                    view._sideloads.Add(mapped);
                }
            });
        }
    }

    public class MapperConfig
    {
        public static void RegisterMapper(Container container)
        {
            Mapper.Initialize(config =>
            {
                config.ConstructServicesUsing(type => container.GetInstance(type));
            });

            var controllerTypes = typeof(IRestController).Assembly.GetExportedTypes()
                .Where(t => typeof(IRestController).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var controllerType in controllerTypes)
            {
                var controller = container.GetInstance(controllerType) as IRestController;
                var autoRestMapperType = typeof(AutoRestMapper<,>).MakeGenericType(controller.EntityType, controller.ViewModelType);
                var autoRestMapper = container.GetInstance(autoRestMapperType) as IAutoRestMapper;
                autoRestMapper.Map(container);
            }

            AutoMapperExtension.Container = container;
            Mapper.CreateMap<Models.Team, ViewModels.Team>().IncludeMany(t => t.users);
        }
    }
}