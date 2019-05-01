using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SportsStore;Trusted_Connection=True;MultipleActiveResultSets=true"));
            //options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));

            //"Server=(localdb)\\MSSQLLocalDB;Database=SportsStore;Trusted_Connection=True;MultipleActiveResultSets=true"

            //services.AddTransient<IProductRepository, FakeProductRepository>();
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddMvc();

            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Этот расширяющий метод отображает детали исключения,
            // которое произошло в приложении, что полезно во время
            // процесса разработки. Он не должен быть включен в раз­
            // вернутых приложениях и при развертывании приложения в
            // главе 12 будет показано, как отключить данное средство
            app.UseDeveloperExceptionPage();

            // Этот расширяющий метод добавляет простое сообщение
            // в НТТР-ответы, которые иначе не имели бы тела, такие
            // как ответы 404 - Not Found(404 - не найдено)
            app.UseStatusCodePages();

            // Этот расширяющий метод включает по,одержку для обслу­
            // живания статического содержимого из папки wwwroot
            app.UseStaticFiles();

            // Этот расширяющий метод включает инфраструктуру
            // ASP.NEТ Core MVC
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );

                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Product", action = "List", productPage = 1 });

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });

            app.UseSession();

            SeedData.EnsurePopulated(app);

            //if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            //app.Run(async (context) =>{await context.Response.WriteAsync("Hello World!");});
        }
    }
}
