using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Amazon.Extensions.CognitoAuthentication;
using Amazon;

namespace Project_I.Web
{
    public class Startup
    {
        private readonly string PoolId;
        private readonly string ClientId;
        private readonly string ClientSecret;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            PoolId = Configuration.GetSection("AWS:UserPoolId").Value;
            ClientId = Configuration.GetSection("AWS:UserPoolClientId").Value;
            ClientSecret = Configuration.GetSection("AWS:UserPoolClientSecret").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var provider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.APSouth1);
            //var CognitoUserPool = new CognitoUserPool(PoolId, ClientId, provider, ClientSecret);
            services.AddCognitoIdentity(config=> {
                config.Password = new Microsoft.AspNetCore.Identity.PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 6,
                    RequiredUniqueChars = 0,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };            
            });
            //services.AddSingleton<IAmazonCognitoIdentityProvider>(provider);
            //services.AddSingleton(CognitoUserPool);
            services.ConfigureApplicationCookie(options=> {
                options.LoginPath = "Accounts/Login";

            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
