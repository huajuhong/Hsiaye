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

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public PageResult<WorkTimeSalary> List(WorkTimeSalaryListInput input)
        {
            var predicates = new List<IPredicate>();
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicates.Add(Predicates.Field<WorkTimeSalary>(f => f.Name, Operator.Like, input.Keywords));
            }
            if (input.ProjectId > 0)
            {
                predicates.Add(Predicates.Field<WorkTimeSalary>(f => f.ProjectId, Operator.Eq, input.ProjectId));
            }
            if (input.MembershipId > 0)
            {
                predicates.Add(Predicates.Field<WorkTimeSalary>(f => f.MembershipId, Operator.Eq, input.MembershipId));
            }
            var pageResult = _database.GetPaged<WorkTimeSalary>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<WorkTimeSalary>(f => f.Id, false) }, input.PageIndex, input.PageSize);
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
