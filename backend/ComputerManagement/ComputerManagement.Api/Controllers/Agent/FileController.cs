using ComputerManagement.API.Controllers.Agent;
using ComputerManagement.BO.DTO;
using ComputerManagement.BO.DTO.File;
using ComputerManagement.BO.Models;
using ComputerManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComputerManagement.Api.Controllers.Agent
{

    public class FileController(IFileService fileService) : BaseController<FileDto, FileModel>(fileService)
    {
        private readonly IFileService _fileService = fileService;

        /// <summary>
        /// lấy file về theo tên file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet("get-file/{filename}")]
        public async Task<IActionResult> GetFile([FromRoute][Required] string filename)
        {
            // do something

            var fileBytes = await _fileService.GetFileByFileName(filename);

            return File(fileBytes, "application/octet-stream");
        }

        /// <summary>
        /// check update phần mềm
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpGet("CheckUpdate/{filename}")]
        public async Task<IActionResult> CheckUpdate([FromRoute] [Required] string filename)
        {
            var rs = new ServiceResponse();
            rs.Data = await _fileService.CheckUpdateAsync(filename);
            return Ok(rs);
        }

        /// <summary>
        /// check update phần mềm
        /// </summary>
        /// <param name="softwareId"></param>
        /// <returns></returns>
        [HttpGet("CheckInstall/{softwareId}")]
        public async Task<IActionResult> CheckUpdate([FromRoute][Required] Guid softwareId)
        {
            var rs = new ServiceResponse();
            rs.Data = await _fileService.CheckInstallAsync(softwareId);
            return Ok(rs);
        }
    }
}
