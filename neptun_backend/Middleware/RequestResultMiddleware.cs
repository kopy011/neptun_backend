using neptun_backend.Entities;
using neptun_backend.Services;
using neptun_backend.Utils;
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
            if (context.Request.Path.Value.Contains("/api/user/alter-role"))
            {
                await _next(context);
                return;
            }

            //get user data
            string? neptunCode = context.User.FindFirst(c => c.Type == UserClaims.NEPTUNCODE)?.Value;
            bool isStudent = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Any(c => c.Value == Roles.STUDENT);

            //get the original req.body
            var originalBodyStream = context.Response.Body;
            var response = context.Response;
            response.Body = new MemoryStream();
            var requestBodyContent = await GetRequestBodyContent(context.Request);
            JObject json = String.IsNullOrEmpty(requestBodyContent) ? new JObject() : JObject.Parse(requestBodyContent);
            context.Request.Body = GenerateStreamFromString(json.ToString());

            await _next(context);

            //convert the respone to correct form
            var resultJson = await CreateResponseJson(context, response, isStudent, neptunCode);
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

        private async Task<JObject> CreateResponseJson(HttpContext context, HttpResponse response, bool isStudent, string neptunCode)
        {
            var responseBodyContent = await GetResponseBodyContent(response);
            try
            {
                var content = IsStudentCourseRequest(context, isStudent, neptunCode) ? await GetSerializedFilteredCourses(context, neptunCode) : JToken.Parse(responseBodyContent);

                JObject resultJson = new JObject
                {
                    {"content", string.IsNullOrEmpty(content.ToString()) ? "" : content },
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

        private async Task<string> GetSerializedFilteredCourses(HttpContext context, string neptunCode)
        {
            //szarul szűri le mert nem inculodja a kurva course-okat szóval lehet kellene a course-service és valahogy úgy megoldani a szűrést főnemesem
            //talán ha lenne egy courseService fv ami megcsinálja a filtert az gecijó lenne

            var coursesStr = await GetResponseBodyContent(context.Response);
            var courses = JsonConvert.DeserializeObject<IList<Course>>(coursesStr);
            var filteredCourses = courses.Where(course => course.Students != null && course.Students.Any(s => s.NeptunCode == neptunCode)).ToList();
            return filteredCourses.Count == 0 ? "" : JsonConvert.SerializeObject(courses);
        }

        private bool IsStudentCourseRequest(HttpContext context, bool isStudent, string neptunCode)
        {

            return context.Request.Path.Value.Contains("/api/course") && isStudent &&
                context.Request.Method == "GET" && neptunCode != null;
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
