using AutoMapper;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public abstract class BaseService<TDto, TModel> : IBaseService<TDto, TModel>
    {

        #region Fields
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IBaseRepo<TModel> _baseRepo;
        protected readonly IMapper _mapper;
        protected readonly IUnitOfWork _uow;
        protected readonly ContextData _contextData;

        public BaseService(IServiceProvider serviceProvider, IBaseRepo<TModel> baseRepo)
        {
            _serviceProvider = serviceProvider;
            _baseRepo = baseRepo;
            _mapper = serviceProvider.GetService(typeof(IMapper)) as IMapper;
            _uow = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            _contextData = serviceProvider.GetService(typeof(ContextData)) as ContextData;
        }

        public async Task<TModel> AddAsync(TDto dto)
        {
            var model = _mapper.Map<TModel>(dto);
            await BeforeSaveAsync(model);
            var rs = await _baseRepo.AddAsync(model);
            if (rs)
            {
                await AfterSaveAsync(model);
                return model;
            }
            else
            {
                throw new BaseException();
            }

        }

        public virtual async Task AfterSaveAsync(TModel entity)
        {

        }

        public virtual async Task BeforeSaveAsync(TModel entity)
        {
            if (entity is BaseModel)
            {
                var baseModel = entity as BaseModel;

                switch (baseModel.CmEntityState)
                {
                    case CmEntityState.Add:
                        baseModel.Id = Guid.NewGuid();
                        baseModel.CreatedBy = _contextData.Fullname;
                        baseModel.CreatedAt = DateTime.Now;
                        baseModel.UpdatedBy = _contextData.Fullname;
                        baseModel.UpdatedAt = DateTime.Now;
                        break;
                    case CmEntityState.Update:
                        baseModel.UpdatedBy = _contextData.Fullname;
                        baseModel.UpdatedAt = DateTime.Now;
                        break;
                }
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entityExist = await _baseRepo.GetAsync(id);
            if (entityExist != null)
            {
                return await _baseRepo.DeleteAsync(entityExist);
            }else
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.NotFound
                };
            }
        }

        public Task<TModel> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<(List<TModel>, int)> GetListAsync(string keySearch, int pageNumber, int pageSize, List<string> fieldsSearch, Dictionary<string, string> fieldToSort)
        {
            var (entities, totalCount) = await _baseRepo.GetListAsync(keySearch, pageNumber, pageSize, fieldsSearch, fieldToSort);
            return (entities, totalCount);
        }

        public Task<TModel> UpdateAsync(TDto dto)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
