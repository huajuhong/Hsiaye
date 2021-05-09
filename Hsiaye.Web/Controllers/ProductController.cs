﻿using Hsiaye.Application;
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
    //组织机构商品管理（商品管理）
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public ProductController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        [HttpPost]
        [Authorize(PermissionNames.商品_新建)]
        public bool Create(ProductInput input)
        {
            Product entity = new Product
            {
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                Name = input.Name,
                Title = input.Title,
                Price = input.Price,
                Cover = input.Cover,
                Description = input.Description,
                InventoryQuantity = input.InventoryQuantity,
                State = input.State,
                PromotionDiscountsId = input.PromotionDiscountsId,
            };

            var entityPromotionDiscounts = _database.Get<PromotionDiscounts>(input.PromotionDiscountsId);
            if (entityPromotionDiscounts.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("该促销活动不存在");
            }
            if (!entityPromotionDiscounts.Approved)
            {
                throw new UserFriendlyException("该促销活动还未审核，请先审核");
            }

            var predicates = new IPredicate[]
            {
                Predicates.Field<Product>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<Product>(f => f.Title, Operator.Eq, entity.Title)
            };
            int count = _database.Count<Product>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该商品标题已存在");
            }

            _database.Insert(entity);

            return true;
        }

        [HttpGet]
        [Authorize(PermissionNames.商品_列表)]
        public List<Product> List(string keyword, ProductState state, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            if (_accessor.Member.UserName != PermissionNames.AdminUserName)
            {
                predicates.Add(Predicates.Field<Product>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                predicates.Add(Predicates.Field<Product>(f => f.Name, Operator.Like, keyword));
                predicates.Add(Predicates.Field<Product>(f => f.Title, Operator.Like, keyword));
            }
            if (state != ProductState.未知)
            {
                predicates.Add(Predicates.Field<Product>(f => f.State, Operator.Eq, state));
            }
            var list = _database.GetPage<Product>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<Product>(f => f.Id, false) }, page, limit).ToList();
            return list;
        }

        [HttpGet]
        [Authorize(PermissionNames.商品_详情)]
        public Product Get(long id)
        {
            var predicates = new IPredicate[]
            {
                Predicates.Field<Product>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<Product>(f => f.Id, Operator.Eq, id)
            };
            var entity = _database.GetList<Product>(Predicates.Group(GroupOperator.And, predicates.ToArray())).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.商品_编辑)]
        public bool Update(ProductEditInput input)
        {
            var entityPromotionDiscounts = _database.Get<PromotionDiscounts>(input.PromotionDiscountsId);
            if (entityPromotionDiscounts.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("该促销活动不存在");
            }
            if (!entityPromotionDiscounts.Approved)
            {
                throw new UserFriendlyException("该促销活动还未审核，请先审核");
            }

            Product entity = _database.Get<Product>(input.Id);

            entity.Name = input.Name;
            entity.Title = input.Title;
            entity.Price = input.Price;
            entity.Cover = input.Cover;
            entity.Description = input.Description;
            entity.InventoryQuantity = input.InventoryQuantity;
            entity.State = input.State;
            entity.PromotionDiscountsId = input.PromotionDiscountsId;

            var predicates = new IPredicate[]
            {
                Predicates.Field<Product>(f => f.Id, Operator.Eq, entity.Id,true),
                Predicates.Field<Product>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<Product>(f => f.Title, Operator.Eq, entity.Title),
            };

            int count = _database.Count<Product>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该商品标题已存在");
            }
            _database.Update(entity);
            return true;
        }
    }
}