public class RedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/" || string.IsNullOrEmpty(context.Request.Path.Value))
        {
            context.Response.Redirect("Auth/Login");
            return;
        }

        await _next(context);
    }
}