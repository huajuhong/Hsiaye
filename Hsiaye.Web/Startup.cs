using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using Hsiaye.Dapper;
using Hsiaye.Dapper.Mapper;
using Hsiaye.Dapper.Sql;
using Hsiaye.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;

namespace Hsiaye.Web
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
            services.AddMemoryCache();

            //响应压缩
            services.AddResponseCompression();
            services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ActionFilter>();
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;//默认大驼峰命名规则
                options.JsonSerializerOptions.WriteIndented = true;//缩进
                options.JsonSerializerOptions.Converters.Add(new JsonSerializerDateTimeConverter());//时间转换器
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;//字符串中有Unicode字符时需要此设置转义
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hsiaye.Web",
                    Version = "v1",
                    Description = "新的开始",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "token",
                    Description = "登录后返回的token",
                    In = ParameterLocation.Header,
                });
            });

            services.AddTransient(serviceProvider =>
            {
                var connection = new System.Data.SqlClient.SqlConnection("Password=sn668;Persist Security Info=True;User ID=sa;Initial Catalog=Hsiaye;Data Source=.");
                var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
                var sqlGenerator = new SqlGeneratorImpl(config);
                IDatabase database = new Database(connection, sqlGenerator);
                return database;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IAccessor, Accessor>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionChecker, PermissionChecker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hsiaye.Web v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
