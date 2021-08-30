using Hsiaye.Application;
using Hsiaye.Application.Contracts;
using Dapper;
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
    /// 组织机构会员管理（会员管理）
    /// </summary>
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

        [HttpPost]
        [Authorize(PermissionNames.会员_列表)]
        public PageResult<Membership> List(MembershipListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (_accessor.Member.UserName != PermissionNames.AdminUserName)
            {
                predicateGroup.Predicates.Add(Predicates.Field<Membership>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            }
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<Membership>(f => f.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Membership>(f => f.Phone, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Membership>(f => f.IDCard, Operator.Like, input.Keywords));
            }
            if (input.State != MembershipState.未知)
            {
                predicateGroup.Predicates.Add(Predicates.Field<Membership>(f => f.State, Operator.Eq, input.State));
            }

            IList<ISort> sort = new List<ISort> { Predicates.Sort<Membership>(f => f.Id, false) };
            var list = _database.GetPage<Membership>(predicateGroup, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<Membership>(predicateGroup);
            return new PageResult<Membership>(list, count);
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
        /// <summary>
        /// 会员充值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.会员_充值)]
        public bool Recharge(MembershipRechargeInput input)
        {
            if (input.Amount <= 0)
            {
                throw new UserFriendlyException("金额不正确");
            }
            string sql = "select * from Membership with (updlock) where Id=" + input.Id;
            var entity = _database.Connection.QueryFirst<Membership>(sql);
            entity.Balance += input.Amount;

            try
            {
                _database.BeginTransaction();
                DateTime now = DateTime.Now;
                MembershipFundsflow membershipFundsflow = new MembershipFundsflow
                {
                    CreateTime = now,
                    MembershipId = input.Id,
                    ProductId = 0,
                    Type = MembershipFundsflowType.充值,
                    Title = "充值-" + entity.Name,
                    Amount = input.Amount,
                    IncomeAmount = input.Amount,
                    DisburseAmount = 0,
                    Balance = entity.Balance,
                    PayState = PayState.支付成功,
                    PayType = input.PayType,
                    PayTime = now,
                    Description = $"充值：{input.Amount}元，客户：{entity.Name}（{entity.Phone}）。",
                    OrderNumber = now.ToString("yyyyMMddHHmmss") + input.Id.ToString().PadLeft(8, '0'),
                };
                _database.Insert(membershipFundsflow);

                _database.Update(entity);
                _database.Commit();
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
            finally
            {
                _database.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 会员提现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.会员_提现)]
        public bool Withdrawal(MembershipWithdrawalInput input)
        {
            if (input.Amount <= 0)
            {
                throw new UserFriendlyException("金额不正确");
            }
            string sql = "select * from Membership with (updlock) where Id=" + input.Id;
            var entity = _database.Connection.QueryFirst<Membership>(sql);
            if (entity.Balance < input.Amount)
            {
                throw new UserFriendlyException("余额不足");
            }
            entity.Balance -= input.Amount;

            try
            {
                _database.BeginTransaction();
                DateTime now = DateTime.Now;
                MembershipFundsflow membershipFundsflow = new MembershipFundsflow
                {
                    CreateTime = now,
                    MembershipId = input.Id,
                    ProductId = 0,
                    Type = MembershipFundsflowType.提现,
                    Title = "提现-" + entity.Name,
                    Amount = input.Amount,
                    IncomeAmount = 0,
                    DisburseAmount = input.Amount,
                    Balance = entity.Balance,
                    PayState = PayState.支付成功,
                    PayType = input.PayType,
                    PayTime = now,
                    Description = $"提现：{input.Amount}元，客户：{entity.Name}（{entity.Phone}）。",
                    OrderNumber = now.ToString("yyyyMMddHHmmss") + input.Id.ToString().PadLeft(8, '0'),
                };
                _database.Insert(membershipFundsflow);

                _database.Update(entity);
                _database.Commit();
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
            finally
            {
                _database.Dispose();
            }
            return true;
        }
        /// <summary>
        /// 会员消费
        /// </summary>
        /// <param name="input">PreviewAmount=true时返回消费详情</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.会员_消费)]
        public MembershipConsumeOutput Consume(MembershipConsumeInput input)
        {
            var product = _database.Get<Product>(input.ProductId);
            if (product == null || product.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("该商品不存在");
            }

            var promotionDiscounts = _database.Get<PromotionDiscounts>(product.PromotionDiscountsId);
            if (promotionDiscounts == null)
            {
                throw new UserFriendlyException("该商品的促销活动不存在");
            }
            if (!promotionDiscounts.Approved)
            {
                throw new UserFriendlyException("该商品的促销活动还未审核");
            }
            if (DateTime.Now < promotionDiscounts.StartTime)
            {
                throw new UserFriendlyException("该商品的促销活动还未开始");
            }
            if (DateTime.Now > promotionDiscounts.EndTime)
            {
                throw new UserFriendlyException("该商品的促销活动还已结束");
            }
            string sql = "select * from Membership with (updlock) where Id=" + input.Id;
            var entity = _database.Connection.QueryFirst<Membership>(sql);
            decimal discountAmount = 0;//优惠金额
            string ruleDescription = promotionDiscounts.Rule.ToString();
            switch (promotionDiscounts.Rule)
            {
                case PromotionDiscountsRule.满减:
                    if (product.Price >= promotionDiscounts.RuleAmount)
                    {
                        discountAmount = promotionDiscounts.RuleDiscountAmount;
                        ruleDescription += $"（满{promotionDiscounts.RuleAmount}减{promotionDiscounts.RuleDiscountAmount}）";
                    }
                    break;
                case PromotionDiscountsRule.满折:
                    if (product.Price >= promotionDiscounts.RuleAmount)
                    {
                        discountAmount = product.Price * (1 - promotionDiscounts.RuleDiscount / 10M);
                        ruleDescription += $"（满{promotionDiscounts.RuleAmount}打{promotionDiscounts.RuleDiscount}折）";
                    }
                    break;
                case PromotionDiscountsRule.直降:
                    discountAmount = product.Price - promotionDiscounts.RuleDiscountAmount;
                    ruleDescription += $"（{promotionDiscounts.RuleDiscountAmount}）";
                    break;
                case PromotionDiscountsRule.无折扣:
                    break;
                default:
                    throw new UserFriendlyException("该商品的促销活动规则无效");
            }
            decimal amount = product.Price - discountAmount;
            if (amount < 0)
            {
                throw new UserFriendlyException("商品/促销活动异常");
            }

            string description = $"消费：{amount}元，商品：{product.Name} 原价{product.Price}元，活动：{ruleDescription}，优惠金额：{discountAmount}元，客户：{entity.Name}（{entity.Phone}）。";

            if (input.PreviewAmount)
            {
                return new MembershipConsumeOutput
                {
                    Id = input.Id,
                    Amount = amount,
                    PayState = PayState.待支付,
                    Description = description,
                };
            }
            if (entity.Balance < amount)
            {
                throw new UserFriendlyException("余额不足");
            }
            entity.Balance -= amount;
            try
            {
                _database.BeginTransaction();
                DateTime now = DateTime.Now;
                MembershipFundsflow membershipFundsflow = new MembershipFundsflow
                {
                    CreateTime = now,
                    MembershipId = input.Id,
                    ProductId = input.ProductId,
                    Type = MembershipFundsflowType.消费,
                    Title = "消费-" + entity.Name,
                    Amount = amount,
                    IncomeAmount = 0,
                    DisburseAmount = amount,
                    Balance = entity.Balance,
                    PayState = PayState.支付成功,
                    PayType = input.PayType,
                    PayTime = now,
                    Description = description,
                    OrderNumber = now.ToString("yyyyMMddHHmmss") + input.Id.ToString().PadLeft(8, '0'),
                };
                _database.Insert(membershipFundsflow);

                _database.Update(entity);
                _database.Commit();
            }
            catch (Exception ex)
            {
                _database.Rollback();
                throw new UserFriendlyException(ex);
            }
            finally
            {
                _database.Dispose();
            }
            return new MembershipConsumeOutput
            {
                Id = input.Id,
                Amount = amount,
                PayState = PayState.支付成功,
            };
        }
    }
}
