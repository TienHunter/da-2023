using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ComputerManagement.Service.Websocket
{
    public class WSService : IWSService
    {
        private readonly ConcurrentDictionary<string, WebSocket> _socket;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly string Uri = "ws://localhost:44313/";
        public WSService(IHttpContextAccessor httpContextAccessor)
        {
            _socket = new ConcurrentDictionary<string, WebSocket>();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CheckConnect(string topic)
        {
            var rs = false;
            if (!_socket.ContainsKey(topic) || _socket[topic] == null)
            {
                rs = await OpenConnectionAsync(topic);
            }
            return rs;
        }

        public async Task SendMessage(string topic, string message)
        {
            if(await this.CheckConnect(topic))
            {
                _socket.TryGetValue(topic, out WebSocket ws);
                if(ws != null && ws.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(message);
                    await ws.SendAsync(buffer,WebSocketMessageType.Text,true, CancellationToken.None);
                }
            }
        }

        /// <summary>
        /// mở kết nối theo topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private async Task<bool> OpenConnectionAsync(string topic)
        {
            var rs = false;
            try
            {
                // Tạo yêu cầu WebSocket và chấp nhận kết nối
                var context = _httpContextAccessor.HttpContext;
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                if(webSocket != null)
                {
                    _socket.TryAdd(topic, webSocket);
                    rs = true;
                }
            }
            catch (Exception ex)
            {
                // logger;
            }
            finally
            {
                
            }
            return rs;
        }

        public async Task CloseAllConnectionsAsync()
        {
            foreach (var sessionId in _socket.Keys)
            {
                await CloseConnectionAsync(sessionId);
            }
        }

        public async Task<bool> CloseConnectionAsync(string sessionId)
        {
            if (_socket.TryRemove(sessionId, out var webSocket))
            {
                try
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                    return true; // Đã đóng kết nối thành công
                }
                catch (WebSocketException)
                {
                    // Xử lý lỗi nếu có
                    return false; // Không thể đóng kết nối
                }
            }
            return false; // Kết nối không tồn tại
        }
    }
}
