using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Websocket
{
    public interface IWSService
    {
        /// <summary>
        /// check kết nối kênh socket
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        Task<bool> CheckConnect(string topic);

        /// <summary>
        /// gửi message
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(string topic, string message);

        /// <summary>
        /// close all socket
        /// </summary>
        /// <returns></returns>
        Task CloseAllConnectionsAsync();

        /// <summary>
        /// close socket by topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        Task<bool> CloseConnectionAsync(string topic);
    }
}
