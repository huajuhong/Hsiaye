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
    //组织机构促销活动管理（促销活动管理）
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PromotionDiscountsController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public PromotionDiscountsController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        [HttpPost]
        [Authorize(PermissionNames.促销活动_新建)]
        public bool Create(PromotionDiscountsInput input)
        {
            PromotionDiscounts entity = new PromotionDiscounts
            {
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                Name = input.Name,
                State = input.State,
                Approved = input.Approved,
                Rule = input.Rule,
                RuleAmount = input.RuleAmount,
                Discount = input.Discount,
            };

            var predicates = new IPredicate[]
            {
                Predicates.Field<PromotionDiscounts>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<PromotionDiscounts>(f => f.Name, Operator.Eq, entity.Name)
            };
            int count = _database.Count<PromotionDiscounts>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该促销活动已存在");
            }

            _database.Insert(entity);

            return true;
        }

        [HttpGet]
        [Authorize(PermissionNames.促销活动_列表)]
        public List<PromotionDiscounts> List(string keyword, PromotionDiscountsState state, PromotionDiscountsRule rule, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            if (_accessor.Member.UserName != PermissionNames.AdminUserName)
            {
                predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.Name, Operator.Like, keyword));
            }
            if (state != PromotionDiscountsState.未知)
            {
                predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.State, Operator.Eq, state));
            }
            if (rule != PromotionDiscountsRule.未知)
            {
                predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.Rule, Operator.Eq, rule));
            }
            var list = _database.GetPage<PromotionDiscounts>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<PromotionDiscounts>(f => f.Id, false) }, page, limit).ToList();
            return list;
        }

        [HttpGet]
        [Authorize(PermissionNames.促销活动_详情)]
        public PromotionDiscounts Get(long id)
        {
            var predicates = new IPredicate[]
            {
                Predicates.Field<PromotionDiscounts>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<PromotionDiscounts>(f => f.Id, Operator.Eq, id)
            };
            var entity = _database.GetList<PromotionDiscounts>(Predicates.Group(GroupOperator.And, predicates.ToArray())).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.促销活动_编辑)]
        public bool Update(PromotionDiscountsEditInput input)
        {
            PromotionDiscounts entity = _database.Get<PromotionDiscounts>(input.Id);

            entity.Name = input.Name;
            entity.State = input.State;
            entity.Approved = input.Approved;
            entity.Rule = input.Rule;
            entity.RuleAmount = input.RuleAmount;
            entity.Discount = input.Discount;

            var predicates = new IPredicate[]
            {
                Predicates.Field<PromotionDiscounts>(f => f.Id, Operator.Eq, entity.Id,true),
                Predicates.Field<PromotionDiscounts>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<PromotionDiscounts>(f => f.Name, Operator.Eq, entity.Name),
            };

            int count = _database.Count<PromotionDiscounts>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该促销活动已存在");
            }
            _database.Update(entity);
            return true;
        }
    }
}
