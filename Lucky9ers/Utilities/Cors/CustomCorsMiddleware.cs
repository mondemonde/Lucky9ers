namespace Lucky99.Utilities.Cors
{
    public class CustomCorsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "OPTIONS")
            {
                // Handle preflight request
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                context.Response.Headers.Add("Access-Control-Max-Age", "86400"); // 24 hours
                context.Response.StatusCode = 200;
                return;
            }

            // Handle the actual request
            await next(context);
        }
    }
}
