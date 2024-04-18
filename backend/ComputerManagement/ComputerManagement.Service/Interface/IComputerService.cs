using ComputerManagement.BO.DTO;
using ComputerManagement.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Interface
{
    public interface IComputerService : IBaseService<ComputerDto, Computer>
    {
        /// <summary>
        /// lấy ra thông tin máy tính theo địa chỉ mac address
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        Task<ComputerDto> GetComputerByMacAddress(string macAddress);

        /// <summary>
        /// cập nhật state của máy tính theo địa chỉ mac
        /// </summary>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        Task<bool> UpdateStateByMacAddressAsync(string macAddress);

        /// <summary>
        /// Lấy danh sách computer theo computerRoomId
        /// </summary>
        /// <param name="computerRoomId"></param>
        /// <returns></returns>
        Task<List<ComputerDto>> GetListComputerByComputerRoomIdAsync(Guid computerRoomId, PagingParam pagingParam);
    }
}
