using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
 builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
// builder.Services.AddDbContext<Context>();
 builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IQuestion,SQuestion>();
builder.Services.AddScoped<IUser,SUser>();
builder.Services.AddScoped<IWalet,SWalet>();

builder.Services.AddSession();
builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/";
                options.LogoutPath = "/";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/home");
       //Handle 404 erros
       app.Use(async (ctx, next) =>
       {
           await next();
           if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
           {
               //Re-execute the request so the user gets the error page
               ctx.Request.Path = "/Home/home";
               await next();
           }
       });
}else
{
    //   app.UseExceptionHandler("/Home/load");
    //    //Handle 404 erros
    //    app.Use(async (ctx, next) =>
    //    {
    //        await next();
    //        if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    //        {
    //            //Re-execute the request so the user gets the error page
    //            ctx.Request.Path = "/Home/load";
    //            await next();
    //        }
    //    });
    app.UseDeveloperExceptionPage();


}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=login}/{id?}");
    
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=phone}/{action=login}/{id?}");


app.Run();
