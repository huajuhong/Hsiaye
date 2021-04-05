using Hsiaye.Application;
using Hsiaye.Dapper;
using Hsiaye.Domain;
using Hsiaye.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.NUnitTest
{
    /// <summary>
    /// 初始化程序
    /// </summary>
    public class Startup
    {
        [Test]
        public void Configure()
        {
            //建管理员账号
            var member = Table.hsiayeContext.Member.Where(x => x.UserName == "admin").FirstOrDefault();
            if (member == null)
            {
                member = new Member
                {
                    CreateTime = DateTime.Now,
                    AccessFailedCount = 0,
                    AuthenticationSource = "系统初始创建",
                    Avatar = "帅",
                    UserName = "admin",
                    Name = "yuebole",
                    PhoneNumber = "18140340282",
                    IsPhoneNumberConfirmed = true,
                    Password = DESHelper.EncryptByGeneric("qwe123"),
                    PasswordResetCode = "",
                    EmailAddress = "891424065@qq.com",
                    IsEmailConfirmed = true,
                    EmailConfirmationCode = "",
                    IsActive = true,
                    TenantId = 1,
                    LastLoginTime = DateTime.Now,
                };
                Table.hsiayeContext.Member.Add(member);
                Table.hsiayeContext.SaveChanges();
            }

            //管理员权限写入
            var fieldInfos = typeof(PermissionNames).GetFields();
            List<Permission> permissions = new List<Permission>();
            foreach (var item in fieldInfos)
            {
                var value = item.GetValue(item);
                var predicate = Predicates.Group(GroupOperator.And,
                    Predicates.Field<Permission>(f => f.Name, Operator.Eq, value),
                    Predicates.Field<Permission>(f => f.MemberId, Operator.Eq, member.Id)
                    );
                int count = Table.database.Count<Permission>(predicate);
                if (count > 0)
                    continue;
                permissions.Add(new Permission
                {
                    CreatorMemberId = 0,
                    Name = value.ToString(),
                    MemberId = member.Id,
                    RoleId = 0,
                    TenantId = 0,
                    IsGranted = true,
                });
            }
            if (permissions.Any())
            {
                Table.database.Insert(permissions.AsEnumerable());
            }
        }
    }
}
