using Hsiaye.Application.Contracts.Members;
using Hsiaye.Application.Contracts.Members.Dto;
using Hsiaye.Dapper;
using Hsiaye.Domain.Members;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions.Crypto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IMemoryCache _cache;
        private readonly IDatabase _database;

        public MemberController(IMemoryCache cache, IDatabase database, IMemberService memberService)
        {
            _cache = cache;
            _database = database;
            _memberService = memberService;
            //创建管理员账户
            if (_database.Count<Member>(Predicates.Field<Member>(f => f.UserName, Operator.Eq, "admin")) < 1)
            {
                Member member = new Member
                {
                    CreateTime = DateTime.Now,
                    AccessFailedCount = 0,
                    AuthenticationSource = "系统初始创建",
                    Avatar = "",
                    UserName = "admin",
                    Name = "yuebole",
                    PhoneNumber = "18140340282",
                    IsPhoneNumberConfirmed = true,
                    Password = DESHelper.EncryptByGeneric("101010"),
                    PasswordResetCode = "",
                    EmailAddress = "891424065@qq.com",
                    IsEmailConfirmed = true,
                    EmailConfirmationCode = "",
                    IsActive = true,
                    TenantId = 1,
                    LastLoginTime = DateTime.Now,
                };
                _database.Insert(member);
            }
        }
        [HttpPost]
        public MemberToken Login(LoginDto input)
        {
            //string value = _cache.Get<string>(input.VerifyKey);
            //if (string.IsNullOrEmpty(value))
            //    throw new UserFriendlyException("图形验证码已过期");
            //if (!value.Equals(input.VerifyCode, StringComparison.OrdinalIgnoreCase))
            //    throw new UserFriendlyException("图形验证码错误");

            var list = _database.GetList<Member>(Predicates.Field<Member>(f => f.UserName, Operator.Eq, input.UserName)).ToList();
            if (!list.Any())
                throw new UserFriendlyException("账号或密码错误");
            var member = list[0];

            if (member.AccessFailedCount >= 5)
                throw new UserFriendlyException("密码连续5次错误，账户已被锁定");

            if (member.Password != DESHelper.EncryptByGeneric(input.Password))
            {
                //登录失败次数累加1
                member.AccessFailedCount += 1;
                _database.Update(member);
                throw new UserFriendlyException("密码错误");
            }
            string providerKey = Guid.NewGuid().ToString("N");
            List<MemberToken> memberTokens = _database.GetList<MemberToken>(Predicates.Field<MemberToken>(f => f.MemberId, Operator.Eq, member.Id)).ToList();
            MemberToken memberToken;
            if (memberTokens.Any())
            {
                memberToken = memberTokens[0];
                memberToken.TenantId = member.TenantId;
                memberToken.ProviderKey = providerKey;
                memberToken.ExpireDate = DateTime.Now.AddHours(6);
                _database.Update(memberToken);
            }
            else
            {
                memberToken = new MemberToken
                {
                    TenantId = member.TenantId,
                    MemberId = member.Id,
                    LoginProvider = "PC",
                    ProviderKey = providerKey,
                    ExpireDate = DateTime.Now.AddHours(6),
                };
                _database.Insert(memberToken);
            }
            //_cache.Remove(input.VerifyKey);

            MemberDto memberDto = _memberService.Get(member.Id);
            _cache.Set(providerKey, memberDto, memberToken.ExpireDate);

            //记录最后一次登录时间
            member.LastLoginTime = DateTime.Now;
            //登录失败次数清零
            member.AccessFailedCount = 0;
            _database.Update(member);

            return memberToken;
        }

        //1.当前用户列表（包含拥有的角色数组）
        //  当前用户可管理的角色

        //2.当前角色列表（包含拥有的权限数组）
        //  
    }
}
