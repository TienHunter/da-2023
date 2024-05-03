using ComputerManagement.BO.DTO.File;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Configs;
using ComputerManagement.Common.Enums;
using ComputerManagement.Common.Exceptions;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Implement
{
    public class FileService(IServiceProvider serviceProvider, IFileRepo fileRepo, IOptionsMonitor<FileConfig> fileConfig) : BaseService<FileDto, FileModel>(serviceProvider, fileRepo), IFileService
    {
        private readonly IFileRepo _fileRepo = fileRepo;
        private readonly ISoftwareRepo _softwareRepo = serviceProvider.GetService(typeof(ISoftwareRepo)) as ISoftwareRepo;
        private readonly FileConfig _fileConfig = fileConfig.CurrentValue;

        public async Task<Guid> UploadFileAsync(FileSource fileSource)
        {
            // validate
            if (fileSource.FilePath == null || fileSource.FilePath.Length == 0)
            {
                throw new ArgumentException("File is empty or null.");
            }

            _ = await _softwareRepo.GetAsync(fileSource.SoftwareId) ?? throw new BaseException
            {
                StatusCode = HttpStatusCode.NotFound,
                Code = ServiceResponseCode.NotFoundSoftwareModel
            };

            _ = await _fileRepo.GetQueryable().Where(f => f.SoftwareId == fileSource.SoftwareId && f.Version == fileSource.Version).FirstOrDefaultAsync() ?? throw new BaseException
            {
                StatusCode = HttpStatusCode.Conflict,
                Code = ServiceResponseCode.ConflicFileVersion
            };

            var filePath = await this.StoreFileAsync(fileSource.FilePath, _fileConfig.StoreFile, fileSource.SoftwareId.ToString());

            var fileModel = new FileModel
            {
                FileName = fileSource.SoftwareId.ToString() + Path.GetExtension(fileSource.FilePath.FileName),
                ContentType = fileSource.FilePath.ContentType,
                Size = fileSource.FilePath.Length,
            };

            var newGuid = await this.BeforeAddAsync(fileModel);

            await _fileRepo.AddAsync(fileModel);
            return newGuid;

        }
        public async Task<string> StoreFileAsync(IFormFile file, string directoryPath, string fileName)
        {
            var filePath = Path.Combine(directoryPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
