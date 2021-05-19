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
    /// <summary>
    /// 工时薪资单价
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WorkTimeSalaryController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public WorkTimeSalaryController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }
        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Create(WorkTimeSalaryInput input)
        {
            WorkTimeSalary entity = new WorkTimeSalary
            {
                CreateMemberId = _accessor.MemberId,
                CreateTime = DateTime.Now,
                ProjectId = input.ProjectId,
                MembershipId = input.MembershipId,
                Name = input.Name,
                Amount = input.Amount,
                Type = input.Type
            };
            int count = _database.Count<WorkTimeSalary>(Predicates.Field<WorkTimeSalary>(f => f.Name, Operator.Eq, input.Name));
            if (count > 1)
            {
                throw new UserFriendlyException("该项目已存在");
            }
            _database.Insert(entity);

            return true;
        }

        [HttpGet]
        [Authorize(PermissionNames.工时)]
        public PageResult<WorkTimeSalary> List(string keywords, int projectId, int membershipId, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            if (!string.IsNullOrEmpty(keywords))
            {
                predicates.Add(Predicates.Field<WorkTimeSalary>(f => f.Name, Operator.Like, keywords));
            }
            if (projectId > 0)
            {
                predicates.Add(Predicates.Field<WorkTimeSalary>(f => f.ProjectId, Operator.Eq, projectId));
            }
            if (membershipId > 0)
            {
                predicates.Add(Predicates.Field<WorkTimeSalary>(f => f.MembershipId, Operator.Eq, membershipId));
            }
            var pageResult = _database.GetPaged<WorkTimeSalary>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<WorkTimeSalary>(f => f.Id, false) }, page, limit);
            return pageResult;
        }

        [HttpGet]
        [Authorize(PermissionNames.工时)]
        public WorkTimeSalary Get(long id)
        {
            var entity = _database.GetList<WorkTimeSalary>(Predicates.Field<WorkTimeSalary>(f => f.Id, Operator.Eq, id)).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Update(WorkTimeSalaryEditInput input)
        {
            WorkTimeSalary entity = _database.Get<WorkTimeSalary>(input.Id);

            entity.CreateMemberId = _accessor.MemberId;
            entity.CreateTime = DateTime.Now;
            entity.ProjectId = input.ProjectId;
            entity.MembershipId = input.MembershipId;
            entity.Name = input.Name;
            entity.Amount = input.Amount;
            entity.Type = input.Type;

            var predicates = new IPredicate[]
            {
                Predicates.Field<WorkTimeSalary>(f => f.Id, Operator.Eq, entity.Id, true),
                Predicates.Field<WorkTimeSalary>(f => f.Name, Operator.Eq, entity.Name),
            };

            int count = _database.Count<WorkTimeSalary>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该项目已存在");
            }
            _database.Update(entity);
            return true;
        }
    }
}
