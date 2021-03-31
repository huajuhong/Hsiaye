﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.NUnitTest
{
    public class Table
    {
        public static readonly string connectionString = "Password=222222;Persist Security Info=True;User ID=sa;Initial Catalog=Hsiaye;Data Source=.";
        public static readonly IDbConnection connection = new SqlConnection(connectionString);
        [Test]
        public void Create()
        {
            HsiayeContext hsiayeContext = new HsiayeContext(connectionString);
            //如果数据库不存在，EnsureCreated 将创建数据库并初始化数据库架构。 如果存在任何表(包括其他 DbContext 类) 的表，则不会初始化该架构。
            //hsiayeContext.Database.EnsureCreated();
            //若要获取 EnsureCreated 使用的 SQL，可以使用 GenerateCreateScript 方法。
            //var sql = dbContext.Database.GenerateCreateScript();
            //EnsureCreated 仅在数据库中不存在任何表时有效。 如果需要，您可以编写自己的检查来查看是否需要初始化架构，并使用基础 IRelationalDatabaseCreator 服务来初始化架构。
            var databaseCreator = hsiayeContext.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }
    }
}
