﻿using ComputerManagement.BO.DTO;
using ComputerManagement.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace ComputerManagement.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                // Endpoint allows anonymous access
                // Do something here
                await _next(context);
            }
            else
            {
                // Endpoint does not allow anonymous access
                // Do something else
                CreateContextDataFromJwt(context);
                await _next(context);
            }
        }

        private void CreateContextDataFromJwt(HttpContext context)
        {
            try
            {
                // Truy cập HttpContext từ IHttpContextAccessor
                var httpContext = _httpContextAccessor.HttpContext;
                // Lấy Access Token từ cookie
                var accessToken = httpContext.Request.Cookies["AccessToken"];

                // Kiểm tra xem Access Token có tồn tại hay không
                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Gán Access Token vào header "Authorization" của yêu cầu HTTP
                    context.Request.Headers["Authorization"] = "Bearer " + accessToken;
                }

                string authorizationHeader = context.Request.Headers["Authorization"];
                string token = null;

                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    // Extract the token from the "Bearer " prefix
                    token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    var contextData = context.RequestServices.GetService(typeof(ContextData)) as ContextData;

                    // Trích xuất thông tin từ claims để tạo đối tượng contextData
                    var username = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Username")?.Value;
                    var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Email")?.Value;
                    var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
                    var fullname = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "Name")?.Value;
                    if(Guid.TryParse(userId, out Guid uID))
                    {
                        contextData.UserId = uID;
                    }
                    contextData.Username = username;
                    contextData.Fullname = fullname;
                    contextData.Email = email;
                }
            }
            catch (Exception ex)
            {
                throw new BaseException
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                };
            }
        }
    }
}
