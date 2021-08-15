using Hsiaye.Application.Contracts;
using DapperExtensions;using DapperExtensions.Predicate;
using Hsiaye.Domain;
using Hsiaye.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Extensions
{
    /// <summary>
    /// 程序启动后只执行一次
    /// </summary>
    public class StartupFilter : IStartupFilter
    {
        private readonly IMemoryCache _cache;
        private readonly IDatabase _database;

        public StartupFilter(IMemoryCache cache, IDatabase database)
        {
            _cache = cache;
            _database = database;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                var list = _database.GetList<MemberToken>();
                if (list.Any())
                {
                    foreach (var memberToken in list)
                    {
                        if (memberToken.ExpireDate < DateTime.Now)
                            continue;

                        var model = _database.Get<Member>(memberToken.MemberId);
                        var memberDto = ExpressionGenericMapper<Member, MemberDto>.MapperTo(model);
                        var roleIds = _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, memberToken.MemberId)).Select(r => r.RoleId);
                        if (roleIds.Any())
                            memberDto.RoleNames = _database.GetList<Role>(Predicates.Field<Role>(f => f.Id, Operator.Eq, roleIds)).Select(x => x.Name).ToArray();

                        _cache.Set(memberToken.ProviderKey, memberDto, memberToken.ExpireDate);
                    }
                }
                next(builder);
            };
        }
    }
}
