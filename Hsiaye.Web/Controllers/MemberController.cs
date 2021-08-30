using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using DapperExtensions;
using DapperExtensions.Predicate;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    /// <summary>
    /// 系统成员
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;
        private readonly IMemberService _memberService;

        public MemberController(IMemoryCache cache, IAccessor accessor, IDatabase database, IMemberService memberService)
        {
            _cache = cache;
            _accessor = accessor;
            _database = database;
            _memberService = memberService;
        }

        [HttpPost]
        [Authorize(PermissionNames.成员_新建)]
        public MemberDto Create(CreateMemberDto input)
        {
            var dto = _memberService.Create(input);
            return dto;
        }

        [HttpPost]
        [Authorize(PermissionNames.成员_列表)]
        public PageResult<Member> List(MemberListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<Member>(f => f.UserName, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Member>(f => f.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Member>(f => f.Phone, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Member>(f => f.EmailAddress, Operator.Like, input.Keywords));
            }
            if (input.State != MemberState.未知)
            {
                predicateGroup.Predicates.Add(Predicates.Field<Member>(f => f.State, Operator.Eq, input.State));
            }
            IList<ISort> sort = new List<ISort> { Predicates.Sort<Member>(f => f.Id, false) };
            var list = _database.GetPage<Member>(predicateGroup, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<Member>(predicateGroup);
            return new PageResult<Member>(list, count);
        }

        [HttpGet]
        [Authorize(PermissionNames.成员_详情)]
        public MemberDto Get(long id)
        {
            return _memberService.Get(id);
        }

        [HttpPost]
        [Authorize(PermissionNames.成员_编辑)]
        public MemberDto Update(MemberDto input)
        {
            var dto = _memberService.Update(input);
            return dto;
        }
        /// <summary>
        /// 激活成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.成员_编辑)]
        public bool Activate(long id)
        {
            _memberService.Activate(id);
            return true;
        }
        /// <summary>
        /// 停用成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.成员_编辑)]
        public bool DeActivate(long id)
        {
            _memberService.DeActivate(id);
            return true;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MemberToken Login(LoginDto input)
        {
            string value = _cache.Get<string>(input.VerifyKey);
            if (string.IsNullOrEmpty(value))
                throw new UserFriendlyException("图形验证码已过期");
            if (!value.Equals(input.VerifyCode, StringComparison.OrdinalIgnoreCase))
                throw new UserFriendlyException("图形验证码错误");

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
            MemberToken memberToken = _database.Get<MemberToken>(Predicates.Field<MemberToken>(f => f.MemberId, Operator.Eq, member.Id));
            if (memberToken != null)
            {
                memberToken.ProviderKey = providerKey;
                memberToken.ExpireDate = DateTime.Now.AddHours(6);
                _database.Update(memberToken);
            }
            else
            {
                memberToken = new MemberToken
                {
                    MemberId = member.Id,
                    LoginProvider = "PC",
                    ProviderKey = providerKey,
                    ExpireDate = DateTime.Now.AddHours(6),
                };
                _database.Insert(memberToken);
            }
            _cache.Remove(input.VerifyKey);

            MemberDto memberDto = _memberService.Get(member.Id);
            _cache.Set(providerKey, memberDto, memberToken.ExpireDate);

            //记录最后一次登录时间
            member.LastLoginTime = DateTime.Now;
            //登录失败次数清零
            member.AccessFailedCount = 0;
            _database.Update(member);

            return memberToken;
        }

        [HttpPost]
        public bool Logout()
        {
            MemberToken memberToken = _database.Get<MemberToken>(Predicates.Field<MemberToken>(f => f.MemberId, Operator.Eq, _accessor.MemberId));
            memberToken.ProviderKey = string.Empty;
            memberToken.ExpireDate = DateTime.Now;
            _database.Update(memberToken);
            _cache.Remove(_accessor.ProviderKey);
            return true;
        }
        /// <summary>
        /// 当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MemberDto Current()
        {
            MemberDto dto = _cache.Get<MemberDto>(_accessor.ProviderKey);
            if (dto == null)
            {
               throw new UserFriendlyException(1001, "登录失效");
            }
            return dto;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ChangePassword(ChangePasswordDto input)
        {
            _memberService.ChangePassword(input);
            return Logout();
        }

        /// <summary>
        /// 管理员重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.成员_重置密码)]
        public bool ResetPassword(ResetPasswordDto input)
        {
            return _memberService.ResetPassword(input);
        }
    }
}