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
            var model = _mapper.Map<TModel>(dto);
            await BeforeSaveAsync(model);
            var rs = await _baseRepo.AddAsync(model);
            if (rs)
            {
                await AfterSaveAsync(model);
                return _mapper.Map<TDto>(model);
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
            var model = _mapper.Map<TModel>(dto);
            await BeforeSaveAsync(model);
            // await ValidateBeforeSave(model);
            var rs = await _baseRepo.UpdateAsync(model);
            if (rs)
            {
                await AfterSaveAsync(model);
                return _mapper.Map<TDto>(model);
            }
            else
            {
                throw new BaseException();
            }
        }


        #endregion
    }
}
