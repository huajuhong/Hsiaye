using Hsiaye.Domain;
using Hsiaye.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
