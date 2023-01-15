using Microsoft.AspNetCore.Authorization;
using Microsoft.IO;
using System.Security.Claims;

namespace neptun_backend.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private const int ReadChunkBufferLength = 4096;
        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            //tell wether the endpoint requieres authorization or not
            Endpoint endpoint = context.GetEndpoint()!;
            bool isAuthRequiered = endpoint.Metadata.Any((m) => m.GetType() == typeof(AuthorizeAttribute));
            bool isAnonymous = endpoint.Metadata.Any((m) => m.GetType() == typeof(AllowAnonymousAttribute));
            if (isAuthRequiered && !isAnonymous)
            {
                await LogRequest(context);
            }

            await _next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            var logString = $"Http Request Information: {Environment.NewLine}" +
                            $"Method: {context.Request.Method} " +
                            $"Path: {context.Request.Path} " +
                            $"User id: {context.User.FindFirstValue(ClaimTypes.NameIdentifier)} " +
                            $"User name: {context.User.FindFirstValue(ClaimTypes.Name)} ";
            logString += context.Request.ContentType != null && context.Request.ContentType.StartsWith("multipart/form-data")
                ? "Request body: multipart/form-data (potentially binary data)"
                : $"Request body: {ReadStreamInChunks(requestStream)}";

            _logger.LogInformation(logString);

            context.Request.Body.Position = 0;
        }
        private static string ReadStreamInChunks(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            string result;
            using (var textWriter = new StringWriter())
            using (var reader = new StreamReader(stream))
            {
                var readChunk = new char[ReadChunkBufferLength];
                int readChunkLength;

                do
                {
                    readChunkLength = reader.ReadBlock(readChunk, 0, ReadChunkBufferLength);
                    textWriter.Write(readChunk, 0, readChunkLength);
                } while (readChunkLength > 0);

                result = textWriter.ToString();
            }

            return result;
        }

    }
}
