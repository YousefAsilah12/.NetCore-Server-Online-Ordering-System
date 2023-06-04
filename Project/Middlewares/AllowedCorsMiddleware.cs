
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Project.Middlewares
{
    public class AllowedCorsMiddleware
    {
        private readonly RequestDelegate _next;
        public AllowedCorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //מקבל בק/ה מהלקוח כל המידע של הבקשה
        public async Task Invoke(HttpContext context)
        {
            // פתרון לבעית ה CORS
            // אחר לשמות השרתים שמותר להם לעבור דרך הרשת 

          //  context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //שהשרת יכול לקבל אחראי לסוג ה HEADERS 
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "*" });
            //context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "*" });
            context.Response.Headers.Add("Access-Control-Allow-Methods",new[] { "*" });
            

            await _next(context);
        
        }
    }
}
