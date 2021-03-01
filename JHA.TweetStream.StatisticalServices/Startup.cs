using JHA.TweetStream.StatisticalServices.Interfaces.Statistics;
using JHA.TweetStream.StatisticalServices.Models.Settings;
using JHA.TweetStream.StatisticalServices.Services.Statistics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace JHA.TweetStream.StatisticalServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            AppSettingConfiguration = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
              .Build();
        }

        public IConfiguration AppSettingConfiguration { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AppSettings>(AppSettingConfiguration);
            services.AddHostedService<StreamService>();

            services.AddControllers();

            services.AddSingleton<StatisticsService>();
            services.AddSingleton<IEmojiService,EmojiService>();
            services.AddTransient<StaticticalAverageService>();
            services.AddTransient<AttributeStaticticsService>();         
            services.AddTransient<UrlStaticticsService>();
            services.AddTransient<StaticticalAverageService>();
            services.AddTransient<AverageAttributeStaticticsService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JHA.TweetStream.StatisticalServices", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JHA.TweetStream.StatisticalServices v1"));
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
