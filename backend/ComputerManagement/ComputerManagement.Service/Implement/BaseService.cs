using AutoMapper;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
        #endregion

        public BaseService(IServiceProvider serviceProvider, IBaseRepo<TModel> baseRepo)
        {
            _serviceProvider = serviceProvider;
            _baseRepo = baseRepo;
            _mapper = serviceProvider.GetService(typeof(IMapper)) as IMapper;
            _uow = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            _contextData = serviceProvider.GetService(typeof(ContextData)) as ContextData;
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            
            var model = await BeforeAddAsync(dto);
            await this.ValidateBeforeAddAsync(model);
            var rs = await _baseRepo.AddAsync(model);
            if (rs)
            {
                await AfterAddAsync(model);
                return _mapper.Map<TDto>(model);
            }
            else
            {
                throw new BaseException();
            }

        }

        public async Task AfterAddAsync(TModel entity)
        {
            
        }

        public virtual async Task AfterSaveAsync(TModel entity)
        {

        }

        public Task AfterUpdateAsync(TModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TModel> BeforeAddAsync(TDto dto)
        {
            var model = _mapper.Map<TModel>(dto);
            if (model is BaseModel)
            {
                var baseModel = model as BaseModel;

                baseModel.Id = Guid.NewGuid();
                baseModel.CreatedBy = _contextData.Fullname;
                baseModel.CreatedAt = DateTime.Now;
                baseModel.UpdatedBy = _contextData.Fullname;
                baseModel.UpdatedAt = DateTime.Now;
                
            }
            return model;

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

        public async Task<TModel> BeforeUpdateAsync(TDto dto)
        {
            var model = _mapper.Map<TModel>(dto);
            if (model is BaseModel)
            {
                var baseModel = model as BaseModel;
                baseModel.UpdatedBy = _contextData.Fullname;
                baseModel.UpdatedAt = DateTime.Now;

            }
            return model;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entityExist = await _baseRepo.GetAsync(id);
            if (entityExist == null)
            {
                this.ThrowNotFoundException();
            }
            return await _baseRepo.DeleteAsync(entityExist);
        }

        public async Task<TDto> GetAsync(Guid id)
        {
            var model = await _baseRepo.GetAsync(id);
            if(model == null)
            {
                var nameCode = "NotFound" + nameof(TModel);
                if (Enum.TryParse(typeof(ServiceResponseCode), nameCode, out object enumValue))
                {
                    ServiceResponseCode code = (ServiceResponseCode)enumValue;
                    throw new BaseException
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Code = code
                    };
                }
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Code = ServiceResponseCode.Error
                };
            }
            return _mapper.Map<TDto>(model);
        }
        public async Task<(List<TDto>, int)> GetListAsync(PagingParam pagingParam)
        {
            var (entities, totalCount) = await _baseRepo.GetListAsync(pagingParam.KeySearch, pagingParam.PageNumber, pagingParam.PageSize, pagingParam.FieldSort, pagingParam.SortAsc);
            var dtos = _mapper.Map<List<TDto>>(entities);
            return (dtos, totalCount);
        }

        public async Task<TDto> UpdateAsync(TDto dto)
        {
            var model = await BeforeUpdateAsync(dto);
            await this.ValidateBeforeUpdateAsync(model);
            var rs = await _baseRepo.UpdateAsync(model);
            if (rs)
            {
                await AfterUpdateAsync(model);
                return _mapper.Map<TDto>(model);
            }
            else
            {
                throw new BaseException();
            }
        }

        public virtual async Task ValidateBeforeAddAsync(TModel model)
        {
            
        }

        public virtual async Task ValidateBeforeDeleteAsync(Guid id)
        {
            var modelExist = await _baseRepo.GetAsync(id);
            if(modelExist == null)
            {
                this.ThrowNotFoundException();
            }
        }

        public virtual async Task ValidateBeforeUpdateAsync(TModel model)
        {
            if (model is BaseModel)
            {
                var baseModel = model as BaseModel;
                var modelExist = await _baseRepo.GetAsync(baseModel.Id);
                if(modelExist == null)
                {
                    this.ThrowNotFoundException();
                }    
            }
        }

        protected void ThrowNotFoundException()
        {
            var nameCode = "NotFound" + nameof(TModel);
            if (Enum.TryParse(typeof(ServiceResponseCode), nameCode, out object enumValue))
            {
                ServiceResponseCode code = (ServiceResponseCode)enumValue;
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Code = code
                };
            }
            throw new BaseException
            {
                StatusCode = HttpStatusCode.NotFound,
                Code = ServiceResponseCode.Error
            };
        }
        
    }
}
