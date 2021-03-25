using System;
using System.Threading.Tasks;
using SimpleStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

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

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews().AddDataAnnotationsLocalization().AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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
            app.UseRequestLocalization();
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

            Initialization(serviceProvider); // Стартовая инициализация ролей и админа
        }

        private void Initialization(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            Task<IdentityResult> roleResult;

            string email = "admin@mail.ru";

            try
            {
                /* Проверяем наличие используемых ролей (Пользователь, Модератор, Админ)
                Если таких нет, то создаём их */

                Task<bool> hasUserRole = roleManager.RoleExistsAsync("User");
                hasUserRole.Wait(); // ждём завершения выполнения

                if (!hasUserRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole("User"));
                    roleResult.Wait();
                }

                Task<bool> hasModeratorRole = roleManager.RoleExistsAsync("Moderator");
                hasModeratorRole.Wait();

                if (!hasModeratorRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole("Moderator"));
                    roleResult.Wait();
                }

                Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Admin");
                hasAdminRole.Wait();

                if (!hasAdminRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole("Admin"));
                    roleResult.Wait();
                }

                // Проверяем есть ли пользователь администратор, если нет то создаём его        

                Task<User> testUser = userManager.FindByEmailAsync(email);
                testUser.Wait();

                if (testUser.Result == null)
                {
                    User administrator = new User {
                        Email = email,
                        UserName = email,
                        Address = "",
                        PhoneNumber = "",
                        DateBirth = DateTime.Now.ToString()
                    };

                    Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "123456Qwerty!");
                    newUser.Wait();

                    if (newUser.Result.Succeeded)
                    {
                        Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                        newUserRole.Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}