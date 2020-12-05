using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;

namespace Shared.Middlewares
{
    public class JwtMiddleware
    {
        private const string AuthorizationHeader = "Authorization";
        private const string BearerPrefix = "Bearer ";

        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next,
            ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IExecutionContext executionContext,
            IExecutionContextHelper executionContextHelper)
        {
            try
            {
                var authHeader = context.Request.Headers[AuthorizationHeader].FirstOrDefault();
                if (authHeader == null)
                {
                    _logger.LogTrace("No auth header in request");
                    return;
                }

                var trimmedHeader = authHeader.Trim();
                if (!trimmedHeader.StartsWith(BearerPrefix))
                {
                    _logger.LogTrace("Authorization header is not started with Bearer ");
                    return;
                }

                var token = trimmedHeader.Remove(0, BearerPrefix.Length).Trim();
                executionContextHelper.FillExecutionContextFromJwt(token, executionContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception happened during validating token");
            }
            finally
            {
                // do nothing if jwt validation fails
                // execution context is not initialized 
                // auth filters will return 401 in this cases 
                await _next(context);
            }
        }
    }
}
