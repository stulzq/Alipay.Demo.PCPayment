using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.F2FPay.AspnetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Alipay.Demo.PCPayment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
			Console.WriteLine(Configuration["Alipay:AlipayPublicKey"]);
	        services.AddAlipay(options =>
	        {
		        options.AlipayPublicKey = Configuration["Alipay:AlipayPublicKey"];
		        options.AppId = Configuration["Alipay:AppId"];
		        options.CharSet = Configuration["Alipay:CharSet"];
		        options.Gatewayurl = Configuration["Alipay:Gatewayurl"];
		        options.PrivateKey = Configuration["Alipay:PrivateKey"];
		        options.SignType = Configuration["Alipay:SignType"];
		        options.Uid = Configuration["Alipay:Uid"];
	        }).AddAlipayF2F();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
