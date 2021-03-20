using Hsiaye.Application.Contracts.Members.Dto;
using Hsiaye.Dapper;
using Hsiaye.Domain.Members;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IDatabase _database;

        public MemberController(IMemoryCache cache, IDatabase database)
        {
            _cache = cache;
            _database = database;
        }
        [HttpGet]
        public MemberToken Login(LoginDto input)
        {
            string value = _cache.Get<string>(input.VerifyKey);
            if (string.IsNullOrEmpty(value))
                throw new UserFriendlyException("图形验证码已过期");
            if (!value.Equals(input.VerifyCode, StringComparison.OrdinalIgnoreCase))
                throw new UserFriendlyException("图形验证码错误");

            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<Member>(f => f.UserName, Operator.Eq, input.UserName),
                Predicates.Field<Member>(f => f.Password, Operator.Eq, input.Password)
            };

            var list = _database.GetList<Member>(Predicates.Group(GroupOperator.And, predicates.ToArray())).ToList();
            if (!list.Any())
                throw new UserFriendlyException("账号或密码错误");
            var member = list[0];
            string providerKey = Guid.NewGuid().ToString("N");
            MemberToken memberToken = new MemberToken
            {
                TenantId = member.TenantId,
                MemberId = member.Id,
                LoginProvider = "PC",
                ProviderKey = providerKey,
                ExpireDate = DateTime.Now.AddHours(6),
            };
            _database.Insert(memberToken);
            _cache.Remove(input.VerifyKey);

            return memberToken;
        }

        //1.当前用户列表（包含拥有的角色数组）
        //  当前用户可管理的角色

        //2.当前角色列表（包含拥有的权限数组）
        //  
    }
}
