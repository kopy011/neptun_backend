using Microsoft.AspNetCore.Identity;
using neptun_backend.Entities;
using neptun_backend.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Text;

namespace neptun_backend.Middleware
{
    public class RequestResultMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResultMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine(context.User.FindFirst(c => c.Value != null));

            //get the original req.body
            var originalBodyStream = context.Response.Body;
            var response = context.Response;
            response.Body = new MemoryStream();
            var requestBodyContent = await GetRequestBodyContent(context.Request);
            JObject json = String.IsNullOrEmpty(requestBodyContent) ? new JObject() : JObject.Parse(requestBodyContent);
            context.Request.Body = GenerateStreamFromString(json.ToString());

            await _next(context);

            //convert the respone to correct form
            var resultJson = await CreateResponseJson(response);
            byte[] resultByteArray = Encoding.UTF8.GetBytes(resultJson.ToString());

            //set response options
            response.ContentLength = resultByteArray.Length;
            response.ContentType = "application/json";
            await originalBodyStream.WriteAsync(resultByteArray);
        }

        private Stream GenerateStreamFromString(string streamString)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(streamString);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private async Task<JObject> CreateResponseJson(HttpResponse response)
        {
            var responseBodyContent = await GetResponseBodyContent(response);
            try
            {

                JObject resultJson = new JObject
                {
                    {"content", string.IsNullOrEmpty(responseBodyContent) ? "" : JToken.Parse(responseBodyContent)},
                    {"statusCode", response.StatusCode},
                    {"identity", Guid.NewGuid()}
                };

                return resultJson;
            }
            catch
            {
                return new JObject
                {
                    {"error message", responseBodyContent },
                    {"statusCode", response.StatusCode },
                    {"identity", Guid.NewGuid() }
                };
            }
        }

        //private async Task<string> GetSerializedFilteredRollingStocks(HttpContext context, string username, ICourseService courseService, UserManager<ApplicationUser> userManager)
        //{
        //    var rollingStockStr = await GetResponseBodyContent(context.Response);
        //    var rollingStocks = JsonConvert.DeserializeObject<IList<RollingStock>>(rollingStockStr);
        //    var user = await userManager.FindByNameAsync(username);
        //    rollingStocks = rollingStocks.Where(rollingStock =>
        //    {
        //        var foundRollingStock = rollingStockService.GetById(rollingStock.Id);
        //        return foundRollingStock.Owner.Name == user.RailwayCompanyName;
        //    }).ToList();
        //    return JsonConvert.SerializeObject(rollingStocks);
        //}

        private bool IsCourseRequest(HttpContext context, string username)
        {

            return context.Request.Path.Value.Contains("/api/course") &&
                context.Request.Method == "GET" && username != null;
        }


        private async Task<string> GetRequestBodyContent(HttpRequest request)
        {
            request.EnableBuffering();

            var bodyText = await new StreamReader(request.Body).ReadToEndAsync();

            request.Body.Position = 0;

            return bodyText;
        }

        private async Task<string> GetResponseBodyContent(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string bodyText = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyText;
        }
    }
}
