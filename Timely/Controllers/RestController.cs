using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Moravia.Timely.Controllers
{
    public interface IRestController
    {
        Type EntityType { get; }
        Type ViewModelType { get; }
    }

    public abstract class RestController<TEntity, TViewModel> : ApiController, IRestController
        where TEntity : Entity
        where TViewModel : ViewModel
    {
        public Type EntityType { get { return typeof(TEntity); } }
        public Type ViewModelType { get { return typeof(TViewModel); } }
        public Service<TEntity> Service { get; set; }

        public IEnumerable<TViewModel> Get()
        {
            return Mapper.Map<IEnumerable<TViewModel>>(Service.Get());
        }

        public TViewModel Get(int id)
        {
            var entity = Service.Get(id);
            if (entity == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return Mapper.Map<TViewModel>(Service.Get(id));
        }

        public TViewModel Post(TViewModel viewModel)
        {
            viewModel.id = 0;
            var entity = Mapper.Map<TEntity>(viewModel);
            entity = Service.Post(entity);
            return Mapper.Map<TViewModel>(entity);
        }

        public TViewModel Put(int id, TViewModel viewModel)
        {
            viewModel.id = id;
            var entity = Mapper.Map<TEntity>(viewModel);
            entity = Service.Put(id, entity);
            return Mapper.Map<TViewModel>(entity);
        }

        public void Delete(int id)
        {
            Service.Delete(id);
        }
    }
}