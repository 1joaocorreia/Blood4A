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
            context.Response.Redirect("/auth/login");
            return;
        }
        if (context.Request.Path == "/auth")
        {
            context.Response.Redirect("/auth/login");
        }

        await _next(context);
    }
}