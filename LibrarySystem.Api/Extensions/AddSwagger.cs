namespace LibrarySystem.Api.Extensions
{
    public static class AddSwagger
    {
        public static WebApplication UseSwaggerMiddleware( this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
