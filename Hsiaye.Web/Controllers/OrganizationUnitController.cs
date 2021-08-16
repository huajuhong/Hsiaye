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
    /// <summary>
    /// 组织机构管理
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrganizationUnitController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public OrganizationUnitController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        /// <summary>
        /// 组织机构树结构（用途：树结构列表、添加、编辑时可用）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.组织机构)]
        public List<OrganizationUnitTree> Tree()
        {
            IPredicate predicate = null;
            if (PermissionNames.AdminUserName != _accessor.Member.UserName)
            {
                predicate = Predicates.Field<MemberOrganizationUnit>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId);

                var list = _database.GetList<MemberOrganizationUnit>(predicate);
                if (list.Any())
                {
                    predicate = Predicates.Field<OrganizationUnit>(f => f.Id, Operator.Eq, list.Select(x => x.OrganizationUnitId));
                }
            }
            else
            {
                predicate = Predicates.Field<OrganizationUnit>(f => f.ParentId, Operator.Eq, 0);
            }
            var organizationUnits = _database.GetList<OrganizationUnit>(predicate).ToList();
            var parents = ExpressionGenericMapper<OrganizationUnit, OrganizationUnitTree>.MapperTo(organizationUnits);
            var tree = GetTree(parents);
            return parents;
        }

        private List<OrganizationUnitTree> GetTree(List<OrganizationUnitTree> parents)
        {
            foreach (var parent in parents)
            {
                var organizationUnits = _database.GetList<OrganizationUnit>(Predicates.Field<OrganizationUnit>(f => f.ParentId, Operator.Eq, parent.Id)).ToList();
                if (organizationUnits.Count > 0)
                {
                    var child = ExpressionGenericMapper<OrganizationUnit, OrganizationUnitTree>.MapperTo(organizationUnits);
                    parent.Child = child;
                    GetTree(child);
                }
            }
            return parents;
        }

        [HttpPost]
        [Authorize(PermissionNames.组织机构)]
        public bool Create(OrganizationUnitInput input)
        {
            OrganizationUnit entity = new OrganizationUnit
            {
                CreateTime = DateTime.Now,
                CreateMemberId = _accessor.MemberId,
                Name = input.Name,
                Description = input.Description,
                ParentId = input.ParentId,
                ModifyMemberId = _accessor.MemberId,
                ModifyTime = DateTime.Now,
            };

            int count = _database.Count<OrganizationUnit>(Predicates.Field<OrganizationUnit>(f => f.Name, Operator.Eq, input.Name));
            if (count > 1)
            {
                throw new UserFriendlyException("该组织机构名称已存在");
            }

            _database.Insert(entity);

            return true;
        }
        /// <summary>
        /// 组织结构列表，用于后台管理列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.组织机构)]
        public PageResult<OrganizationUnit> List(KeywordsListInput input)
        {
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate.Predicates.Add( Predicates.Field<OrganizationUnit>(f => f.Name, Operator.Like, input.Keywords));
            }

            IList<ISort> sort = new List<ISort> { Predicates.Sort<OrganizationUnit>(f => f.Id, false) };
            var list = _database.GetPage<OrganizationUnit>(predicate, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<OrganizationUnit>(predicate);
            return new PageResult<OrganizationUnit>(list, count);
        }

        [HttpGet]
        [Authorize(PermissionNames.组织机构)]
        public OrganizationUnit Get(long id)
        {
            var entity = _database.GetList<OrganizationUnit>(Predicates.Field<OrganizationUnit>(f => f.Id, Operator.Eq, id)).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.组织机构)]
        public bool Update(OrganizationUnitEditInput input)
        {
            OrganizationUnit entity = _database.Get<OrganizationUnit>(input.Id);

            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.ParentId = input.ParentId;
            entity.ModifyMemberId = _accessor.MemberId;
            entity.ModifyTime = DateTime.Now;

            var predicates = new IPredicate[]
            {
                Predicates.Field<OrganizationUnit>(f => f.Id, Operator.Eq, entity.Id,true),
                Predicates.Field<OrganizationUnit>(f => f.Name, Operator.Eq, entity.Name),
            };

            int count = _database.Count<OrganizationUnit>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该组织机构名称已存在");
            }
            _database.Update(entity);
            return true;
        }
    }
}
