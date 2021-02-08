using Hsiaye.Dapper;
using Hsiaye.Dapper.Mapper;
using Hsiaye.Dapper.Sql;
using Hsiaye.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace Hsiaye.NUnitTest
{
    public class Tests
    {
        protected IDatabase Db;
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var connection = new SqlConnection("Password=sn668;Persist Security Info=True;User ID=sa;Initial Catalog=Hsiaye;Data Source=.");
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
            var sqlGenerator = new SqlGeneratorImpl(config);
            Db = new Database(connection, sqlGenerator);
            //Db.Insert(new Demo
            //{
            //    Name = "Q",
            //    Code = "1"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "W",
            //    Code = "2"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "E",
            //    Code = "3"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "R",
            //    Code = "4"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "T",
            //    Code = "5"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "Y",
            //    Code = "6"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "U",
            //    Code = "7"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "I",
            //    Code = "8"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "O",
            //    Code = "9"
            //});
            //Db.Insert(new Demo
            //{
            //    Name = "P",
            //    Code = "0"
            //});

            //列表
            //IFieldPredicate predicate1 = Predicates.Field<Demo>(f => f.Code, Operator.Eq, "1");
            //IFieldPredicate predicate2 = Predicates.Field<Demo>(f => f.Name, Operator.Eq, "W");

            ISort sort = Predicates.Sort<Demo>(x => x.Id, false);

            //var group = Predicates.Group(GroupOperator.Or, new IFieldPredicate[] { predicate1, predicate2 });

            //var list = Db.GetList<Demo>(group, new List<ISort> { sort });

            //列表：页码：1，分页大小：2
            var page1 = Db.GetPage<Demo>(null, new List<ISort> { sort }, 0, 2);
            //列表：页码：2，分页大小：2
            var page2 = Db.GetPage<Demo>(null, new List<ISort> { sort }, 1, 2);

            //var page2 = Db.GetSet<Demo>(null, new List<ISort> { sort }, 2, 2, null, 1000, true);
        }
    }

    public class Demo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}