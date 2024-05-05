using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using ComputerManagement.Common.Configs;
using ComputerManagement.Service.Interface;
using ComputerManagerment.Repos.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ComputerManagement.Service.Implement
{
    public class SoftwareService(IServiceProvider serviceProvider, ISoftwareRepo softwareRepo, IOptionsMonitor<FileConfig> fileConfig) : BaseService<SoftwareDto, SoftwareModel>(serviceProvider, softwareRepo), ISoftwareService
    {
        private readonly ISoftwareRepo _softwareRepo = softwareRepo;
        private readonly FileConfig _fileConfig = fileConfig.CurrentValue;

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var rs  = await base.DeleteAsync(id);

            // xóa file trong ổ cứng
            if(rs)
            {
                var filePaths = Directory.GetFiles(_fileConfig.StoreFile).ToList();
                foreach(string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    if (fileName.Contains(id.ToString()))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            return rs;
        }
    }
}
