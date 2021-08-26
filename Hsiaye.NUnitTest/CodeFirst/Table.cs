using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Hsiaye.Application.Contracts;
using Hsiaye.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Database = DapperExtensions.Database;
using IDatabase = DapperExtensions.IDatabase;

namespace Hsiaye.NUnitTest
{
    public class Table
    {
        public static readonly string connectionString = "Password=222222;Persist Security Info=True;User ID=sa;Initial Catalog=Hsiaye;Data Source=.";
        public static readonly IDbConnection connection = new SqlConnection(connectionString);
        public static readonly HsiayeContext hsiayeContext = new HsiayeContext(connectionString);
        public static IDatabase database
        {
            get
            {
                var config = new DapperExtensionsConfiguration();
                var sqlGenerator = new SqlGeneratorImpl(config);
                return new Database(Table.connection, sqlGenerator);
            }
        }

        [Test]
        public void Map()
        {
            //https://www.coder.work/article/1603792
            //https://dapper-tutorial.net/knowledge-base/33768919/how-do-you-register-dapperextension-classmapper-subclasses-so-they-are-used-
            //https://stackoverflow.com/questions/58659060/dapper-extensions-custom-classmapper-isnt-called-on-insert
            //DapperExtensions.DapperAsyncExtensions.DefaultMapper = typeof(PluralizedAutoClassMapper<>);
            //var list = database.GetList<RoleDto>();
        }
        [Test]
        public void Create()
        {
            //删除数据库
            //var result = hsiayeContext.Database.EnsureDeleted();

            //如果数据库不存在，EnsureCreated 将创建数据库并初始化数据库架构。 如果存在任何表(包括其他 DbContext 类) 的表，则不会初始化该架构。
            //hsiayeContext.Database.EnsureCreated();
            //若要获取 EnsureCreated 使用的 SQL，可以使用 GenerateCreateScript 方法。
            var sql = hsiayeContext.Database.GenerateCreateScript()
                .Replace("datetime2", "datetime");//修改时间精度

            //EnsureCreated 仅在数据库中不存在任何表时有效。 如果需要，您可以编写自己的检查来查看是否需要初始化架构，并使用基础 IRelationalDatabaseCreator 服务来初始化架构。
            //var databaseCreator = hsiayeContext.GetService<IRelationalDatabaseCreator>();
            //databaseCreator.CreateTables();
        }
        [Test]
        public void OneToMany()
        {
            //DapperExtensions.DapperExtensions.DefaultMapper = typeof(RoleMap);

            //// Tell Dapper Extension to scan this assembly for custom mappings
            //DapperExtensions.DapperExtensions.SetMappingAssemblies(new[]
            //{
            //    typeof (RoleMap).Assembly
            //});
            //DapperExtensions.DapperExtensions.SetMappingAssemblies(new[] { Assembly.GetExecutingAssembly() });
            var role = database.Get<Role>(1);
        }
    }
}
