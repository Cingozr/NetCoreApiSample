using AutoMapper;
using BlogApi.Entities;
using BlogApi.Mapping;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace BlogApi
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
            services.AddMvc(cfg =>
            {
                cfg.ReturnHttpNotAcceptable = true;
                cfg.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                cfg.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:BlogApiDbConnection"]));
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();

            #region AutoMapper Config

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlogCustomMappingProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, BlogContext blogContext, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddNLog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }


            blogContext.EnsureSeedDataForContext();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
