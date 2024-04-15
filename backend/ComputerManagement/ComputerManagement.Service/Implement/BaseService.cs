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

        public virtual async Task<Guid> AddAsync(TDto dto)
        {
            TModel? model = _mapper.Map<TModel>(dto);
            var newId = await BeforeAddAsync(model);
            await this.ValidateBeforeAddAsync(model);
            var rs = await _baseRepo.AddAsync(model);
            if (rs)
            {
                await AfterAddAsync(model);
                return newId;
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

        public virtual async Task AfterUpdateAsync(TModel entity)
        {

        }

        public virtual async Task<Guid> BeforeAddAsync(TModel model)
        {
            var newId = Guid.NewGuid();
            if (model is BaseModel)
            {
                var baseModel = model as BaseModel;

                baseModel.Id = newId;
                baseModel.CreatedBy = _contextData.Fullname;
                baseModel.CreatedAt = DateTime.Now;
                baseModel.UpdatedBy = _contextData.Fullname;
                baseModel.UpdatedAt = DateTime.Now;

            }
            return newId;

        }
        public virtual async Task BeforeUpdateAsync(TModel model)
        {
            if (model is BaseModel)
            {
                var baseModel = model as BaseModel;
                baseModel.UpdatedBy = _contextData.Fullname;
                baseModel.UpdatedAt = DateTime.Now;

            }
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entityExist = await _baseRepo.GetAsync(id);
            this.CheckNullModel(entityExist);
            return await _baseRepo.DeleteAsync(entityExist);
        }

        public virtual async Task<TDto> GetAsync(Guid id)
        {
            var model = await _baseRepo.GetAsync(id);
            this.CheckNullModel(model);
            return _mapper.Map<TDto>(model);
        }
        public virtual async Task<(List<TDto>, int)> GetListAsync(PagingParam pagingParam)
        {
            var (entities, totalCount) = await _baseRepo.GetListAsync(pagingParam.KeySearch, pagingParam.PageNumber, pagingParam.PageSize, pagingParam.FieldSort, pagingParam.SortAsc);
            var dtos = _mapper.Map<List<TDto>>(entities);
            return (dtos, totalCount);
        }

        public virtual async Task<bool> UpdateAsync(TDto dto, Guid id)
        {
            var modelExist = await _baseRepo.GetAsync(id);
            this.CheckNullModel(modelExist);
            _mapper.Map(dto, modelExist);
            await BeforeUpdateAsync(modelExist);
            await this.ValidateBeforeUpdateAsync(modelExist);
            var rs = await _baseRepo.UpdateAsync(modelExist);
            if (rs)
            {
                await AfterUpdateAsync(modelExist);
                return rs;
            }
            else
            {
                throw new BaseException();
            }
        }

        public virtual async Task ValidateBeforeAddAsync(TModel model)
        {

        }

        public virtual async Task ValidateBeforeDeleteAsync(TModel model)
        {

        }

        public virtual async Task ValidateBeforeUpdateAsync(TModel model)
        {
        }

        protected void CheckNullModel(TModel? model)
        {
            if (model == null)
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
}
