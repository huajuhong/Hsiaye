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

            //��Ӧѹ��
            services.AddResponseCompression();
            services.AddControllers(options =>
            {
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<ActionFilter>();
                options.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;//Ĭ�ϴ��շ���������
                options.JsonSerializerOptions.WriteIndented = true;//����
                options.JsonSerializerOptions.Converters.Add(new JsonSerializerDateTimeConverter());//ʱ��ת����
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;//�ַ�������Unicode�ַ�ʱ��Ҫ������ת��
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


            services.AddTransient(serviceProvider =>
            {
                var connection = new System.Data.SqlClient.SqlConnection("Password=222222;Persist Security Info=True;User ID=sa;Initial Catalog=Hsiaye;Data Source=.");
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hsiaye.Web",
                    Version = "v1",
                    Description = "�µĿ�ʼ",
                });
                c.AddSecurityDefinition("ProviderKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "token",
                    Description = "��¼�ɹ��������е� ProviderKey",
                    In = ParameterLocation.Header,
                });

                //����һ��Scheme��ע�������IdҪ������AddSecurityDefinition�еĲ���nameһ��
                var scheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "ProviderKey" }
                };
                //ע��ȫ����֤�����еĽӿڶ�����ʹ����֤��
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [scheme] = System.Array.Empty<string>()
                });
            });
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
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
