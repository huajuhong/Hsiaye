using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using DapperExtensions;using DapperExtensions.Predicate;
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
                Approved = input.Approved,
                Rule = input.Rule,
                RuleAmount = input.RuleAmount,
                RuleDiscount = input.RuleDiscount,
                RuleDiscountAmount = input.RuleDiscountAmount,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
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

        [HttpPost]
        [Authorize(PermissionNames.促销活动_列表)]
        public PageResult<PromotionDiscounts> List(PromotionDiscountsListInput input)
        {
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (_accessor.Member.UserName != PermissionNames.AdminUserName)
            {
                predicate.Predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            }
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate.Predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.Name, Operator.Like, input.Keywords));
            }
            if (input.Rule != PromotionDiscountsRule.未知)
            {
                predicate.Predicates.Add(Predicates.Field<PromotionDiscounts>(f => f.Rule, Operator.Eq, input.Rule));
            }

            IList<ISort> sort = new List<ISort> { Predicates.Sort<PromotionDiscounts>(f => f.Id, false) };
            var list = _database.GetPage<PromotionDiscounts>(predicate, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<PromotionDiscounts>(predicate);
            return new PageResult<PromotionDiscounts>(list, count);
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
            entity.Approved = input.Approved;
            entity.Rule = input.Rule;
            entity.RuleAmount = input.RuleAmount;
            entity.RuleDiscount = input.RuleDiscount;
            entity.RuleDiscountAmount = input.RuleDiscountAmount;
            entity.StartTime = input.StartTime;
            entity.EndTime = input.EndTime;

            var predicates = new IPredicate[]
            {
                Predicates.Field<PromotionDiscounts>(f => f.Id, Operator.Eq, entity.Id, true),
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
