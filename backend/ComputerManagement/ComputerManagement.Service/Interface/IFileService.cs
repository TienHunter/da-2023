﻿using ComputerManagement.BO.DTO.File;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IFileService : IBaseService<FileDto, FileModel>
    {
        /// <summary>
        /// tải file lên server
        /// </summary>
        /// <param name="fileSource"></param>
        /// <returns></returns>
        Task<Guid> UploadFileAsync(FileSource fileSource);

        /// <summary>
        /// lấy danh sách file cài theo id phần mềm
        /// </summary>
        /// <param name="softwareId"></param>
        /// <returns></returns>
        Task<List<FileDto>> GetListFileBySoftwareId(Guid softwareId);

        /// <summary>
        /// tải file theo filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<byte[]> GetFileByFileName(string fileName);

        /// <summary>
        /// kiểm tra có update phần mềm ở agent không
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> CheckUpdateAsync(string fileName);

        /// <summary>
        /// kiểm tra có cài đặt phần mềm ở agent không
        /// </summary>
        /// <param name="softwareId"></param>
        /// <returns></returns>
        Task<bool> CheckInstallAsync(Guid softwareId);  
    }
}
