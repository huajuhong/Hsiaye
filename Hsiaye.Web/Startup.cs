using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using DapperExtensions;
using DapperExtensions.Predicate;
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
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

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
            })
            .AddNewtonsoftJson(options =>
            {
                //修改属性名称的序列化方式，首字母小写，即驼峰样式
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                //日期类型默认格式化处理 方式1

                //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy/MM/dd HH:mm:ss" });
                //日期类型默认格式化处理 方式2
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                //解决命名不一致问题 
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                //空值处理
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            })
            //    .AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;//默认大驼峰命名规则
            //    options.JsonSerializerOptions.WriteIndented = true;//缩进
            //    options.JsonSerializerOptions.Converters.Add(new JsonSerializerDateTimeConverter());//时间转换器
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;//字符串中有Unicode字符时需要此设置转义
            //})
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


            services.AddTransient(serviceProvider =>
            {
                var connection = new System.Data.SqlClient.SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
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
            services.AddSingleton<IStartupFilter, StartupFilter>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "道阻且长行则将至",
                    Version = "v1",
                    Description = "感到迷茫的时候，与其胡思乱想，不如行动起来，去做点什么。",
                });

                c.AddSecurityDefinition("ProviderKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "token",
                    Description = "登录成功后数据中的 ProviderKey",
                    In = ParameterLocation.Header,
                });

                //声明一个Scheme，注意下面的Id要和上面AddSecurityDefinition中的参数name一致
                var scheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "ProviderKey" }
                };
                //注册全局认证（所有的接口都可以使用认证）
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [scheme] = System.Array.Empty<string>()
                });
                c.IncludeXmlComments(System.IO.Path.Combine(System.AppContext.BaseDirectory, "Hsiaye.Web.xml"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();


            app.UseRouting();

            app.UseCors(c =>
            {
                c.WithHeaders("*");
                c.WithMethods("*");
                c.WithOrigins("*");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hsiaye.Web v1");
            });
        }
    }
}
