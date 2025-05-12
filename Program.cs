using QuizAppService.Hubs;
using QuizAppService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IQuizService, QuizService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Remove the custom middleware for handling OPTIONS requests
// app.Use(async (context, next) =>
// {
//     if (context.Request.Method == HttpMethods.Options)
//     {
//         context.Response.StatusCode = 204;
//         return;
//     }
//     await next();
// });

// Configure CORS with the proper settings
app.UseCors(options =>
{
    options.WithOrigins("http://localhost:4200")  // Allow requests from the frontend
           .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
           .AllowAnyHeader()                    // Allow any headers
           .AllowCredentials();                 // Allow credentials (if needed, for cookies/tokens)
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<QuizHub>("/quizHub");

app.Run();
