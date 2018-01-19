using Box.Security.Data;
using Box.Security.Services;
using Box.Security.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Steeltoe.Discovery.Client;
using Swashbuckle.AspNetCore.Swagger;

namespace Box.Security
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDataContext>();
            
            //Add Eureka Discovery
            services.AddDiscoveryClient(Configuration);
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Info {Title = "Box.Security", Version = "v1"});
            });
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetTestUsers())
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            
            services.AddCors(options => options.AddPolicy("LoginPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddScoped<ICaptchaService, CaptchaService>();
            services.AddScoped<IApiService, ApiService>();
            
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            

        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<UserDataContext>();
                context.Database.EnsureCreated();
                context.InitDb().GetAwaiter().GetResult();
            }

            app.UseIdentityServer();
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Box.Security V1");
            });
           
            app.UseCors("LoginPolicy");
            app.UseMvcWithDefaultRoute();
            app.UseDiscoveryClient();

        }
    }
}
