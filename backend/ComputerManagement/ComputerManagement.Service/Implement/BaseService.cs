using AutoMapper;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Lib.Interface;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Configs;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagement.Service.Websocket;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        protected readonly IEmailService _emailService;
        #endregion

        public BaseService(IServiceProvider serviceProvider, IBaseRepo<TModel> baseRepo)
        {
            _serviceProvider = serviceProvider;
            _baseRepo = baseRepo;
            _mapper = serviceProvider.GetService(typeof(IMapper)) as IMapper;
            _uow = serviceProvider.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            _contextData = serviceProvider.GetService(typeof(ContextData)) as ContextData;
            _emailService = serviceProvider.GetService(typeof(IEmailService)) as IEmailService;

        }

        public virtual async Task<Guid> AddAsync(TDto dto)
        {
            await this.HandleDataBeforeMapAddAsync(dto);
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

        public virtual async Task<Guid> UpdateAsync(TDto dto, Guid id)
        {
            var modelExist = await _baseRepo.GetAsync(id);
            this.CheckNullModel(modelExist);
            await ValidateBeforeMapUpdateAsync(dto, modelExist);
            _mapper.Map(dto, modelExist);
            await BeforeUpdateAsync(modelExist);
            await this.ValidateBeforeUpdateAsync(modelExist);
            var rs = await _baseRepo.UpdateAsync(modelExist);
            if (rs)
            {
                await AfterUpdateAsync(modelExist);
                return id;
            }
            else
            {
                throw new BaseException();
            }
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

        public async Task AfterAddAsync(TModel entity)
        {

        }

        public virtual async Task AfterSaveAsync(TModel entity)
        {

        }

        public virtual async Task AfterUpdateAsync(TModel entity)
        {

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
        public virtual async Task ValidateBeforeMapUpdateAsync(TDto dto, TModel model)
        {
        }

        public virtual async Task ValidateBeforeDeleteRangeAsync(List<TModel> models)
        {
            
        }

        public async Task<bool> DeleteRangeAsync(List<Guid> ids)
        {
            var models = await _baseRepo.GetListByIdsAsync(ids);

            await this.ValidateBeforeDeleteRangeAsync(models);

            return await _baseRepo.DeleteRangeAsync(models);
        }

        public virtual async Task HandleDataBeforeMapAddAsync(TDto dto)
        {
           
        }

        /// <summary>
        /// tạo task chạy luồng riêng
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        protected Task CreateAndRunTaskAsync(Func<Task> function)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await function().ConfigureAwait(false);
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }finally
                {

                }
            });
        }
    }
}
