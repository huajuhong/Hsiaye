using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using DapperExtensions;
using DapperExtensions.Predicate;
using Hsiaye.Domain;
using Hsiaye.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hsiaye.Domain.Shared;

namespace Hsiaye.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProgramController : ControllerBase
    {
        private readonly IDatabase _database;

        public ProgramController(IDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public bool Run()
        {
            //建立组织机构

            //建管理员账号
            var member = _database.Get<Member>(Predicates.Field<Member>(f => f.UserName, Operator.Eq, "admin"));
            if (member == null)
            {
                member = new Member
                {
                    CreateTime = DateTime.Now,
                    AccessFailedCount = 0,
                    AuthenticationSource = "系统初始创建",
                    Avatar = "帅",
                    Gender = Domain.Shared.Gender.男,
                    UserName = "admin",
                    Name = "yuebole",
                    Phone = "18140340282",
                    IsPhoneConfirmed = true,
                    Password = DESHelper.EncryptByGeneric("qwe123"),
                    PasswordResetCode = "",
                    EmailAddress = "891424065@qq.com",
                    IsEmailConfirmed = true,
                    EmailConfirmationCode = "",
                    State = MemberState.正常,
                    LastLoginTime = DateTime.Now,
                };
                _database.Insert(member);
            }

            //管理员权限写入
            List<Permission> permissions = PermissionNames.Permissions;
            List<Permission> permissionsByAdmin = new List<Permission>();
            foreach (var item in permissions)
            {
                var predicate = Predicates.Group(GroupOperator.And,
                    Predicates.Field<Permission>(f => f.Name, Operator.Eq, item.Name),
                    Predicates.Field<Permission>(f => f.MemberId, Operator.Eq, member.Id)
                    );
                int count = _database.Count<Permission>(predicate);
                if (count > 0)
                    continue;
                permissionsByAdmin.Add(new Permission
                {
                    CreatorMemberId = member.Id,
                    Name = item.Name,
                    MemberId = member.Id,
                    RoleId = 0,
                    IsGranted = true,
                });
            }
            if (permissionsByAdmin.Any())
            {
                _database.Insert(permissionsByAdmin.AsEnumerable());
            }

            return true;
        }
    }
}