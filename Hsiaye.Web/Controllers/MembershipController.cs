using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using Hsiaye.Dapper;
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
    //组织机构会员管理（会员管理）
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MembershipController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public MembershipController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        [HttpPost]
        [Authorize(PermissionNames.会员_新建)]
        public bool Create(MembershipInput input)
        {
            Membership entity = new Membership
            {
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                Name = input.Name,
                Gender = input.Gender,
                Phone = input.Phone,
                IDCard = input.IDCard,
                State = input.State,
                Balance = 0,
                Deleted = false
            };
            var predicates = new IPredicate[]
            {
                Predicates.Field<Membership>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<Membership>(f => f.IDCard, Operator.Eq, entity.IDCard)
            };
            int count = _database.Count<Membership>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该身份证号已存在");
            }

            _database.Insert(entity);

            return true;
        }

        [HttpGet]
        [Authorize(PermissionNames.会员_列表)]
        public List<Membership> List(string keyword, MembershipState state, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            if (_accessor.Member.UserName != PermissionNames.AdminUserName)
            {
                predicates.Add(Predicates.Field<Membership>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                predicates.Add(Predicates.Field<Membership>(f => f.Name, Operator.Like, keyword));
                predicates.Add(Predicates.Field<Membership>(f => f.Phone, Operator.Like, keyword));
                predicates.Add(Predicates.Field<Membership>(f => f.IDCard, Operator.Like, keyword));
            }
            if (state != MembershipState.未知)
            {
                predicates.Add(Predicates.Field<Membership>(f => f.State, Operator.Eq, state));
            }
            var list = _database.GetPage<Membership>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<Membership>(f => f.Id, false) }, page, limit).ToList();
            return list;
        }

        [HttpGet]
        [Authorize(PermissionNames.会员_详情)]
        public Membership Get(long id)
        {
            var predicates = new IPredicate[]
            {
                Predicates.Field<Membership>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<Membership>(f => f.Id, Operator.Eq, id)
            };
            var entity = _database.GetList<Membership>(Predicates.Group(GroupOperator.And, predicates.ToArray())).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.会员_编辑)]
        public bool Update(MembershipEditInput input)
        {
            Membership entity = _database.Get<Membership>(input.Id);
            entity.Name = input.Name;
            entity.Gender = input.Gender;
            entity.Phone = input.Phone;
            entity.IDCard = input.IDCard;
            entity.State = input.State;
            var predicates = new IPredicate[]
            {
                Predicates.Field<Membership>(f => f.Id, Operator.Eq, entity.Id,true),
                Predicates.Field<Membership>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<Membership>(f => f.IDCard, Operator.Eq, entity.IDCard),
            };
            int count = _database.Count<Membership>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该身份证号已存在");
            }
            _database.Update(entity);
            return true;
        }
    }
}
