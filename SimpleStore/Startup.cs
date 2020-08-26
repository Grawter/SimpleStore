using SimpleStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SimpleStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => // Entity FrameWork
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options => options.Password.RequireNonAlphanumeric = false) // Identity
                .AddEntityFrameworkStores<ApplicationContext>();

            services.AddControllersWithViews();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(); // Подключение EndpointRoutingMiddleware

            app.UseAuthentication(); // подключение аутентификации
            app.UseAuthorization(); // подключение авторизации

            app.UseEndpoints(endpoints => // Подключение EndpointMiddleware
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");

                endpoints.MapFallbackToController("Index", "Home"); // если запрос не соответствует ни одному маршруту
            });
        }
    }
}
