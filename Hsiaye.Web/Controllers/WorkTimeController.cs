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
    /// 会员工时
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WorkTimeController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public WorkTimeController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Create(WorkTimeInput input)
        {
            WorkTime entity = new WorkTime
            {
                CreateMemberId = _accessor.MemberId,
                CreateTime = DateTime.Now,
                ProjectId = input.ProjectId,
                MembershipId = input.MembershipId,
                Date = input.Date,
                Duration = input.Duration,
                IsOvertime = input.IsOvertime,
                Description = input.Description,
            };

            _database.Insert(entity);

            return true;
        }

        [HttpGet]
        [Authorize(PermissionNames.工时)]
        public List<WorkTime> List(int projectId, int membershipId, bool? isOvertime, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            if (projectId > 0)
            {
                predicates.Add(Predicates.Field<WorkTime>(f => f.ProjectId, Operator.Eq, projectId));
            }
            if (membershipId > 0)
            {
                predicates.Add(Predicates.Field<WorkTime>(f => f.MembershipId, Operator.Eq, membershipId));
            }
            if (isOvertime.HasValue)
            {
                predicates.Add(Predicates.Field<WorkTime>(f => f.IsOvertime, Operator.Eq, isOvertime.Value));
            }
            var list = _database.GetPage<WorkTime>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<WorkTime>(f => f.Id, false) }, page, limit).ToList();
            return list;
        }

        [HttpGet]
        [Authorize(PermissionNames.工时)]
        public WorkTime Get(long id)
        {
            var entity = _database.GetList<WorkTime>(Predicates.Field<WorkTime>(f => f.Id, Operator.Eq, id)).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Update(WorkTimeEditInput input)
        {
            WorkTime entity = _database.Get<WorkTime>(input.Id);

            entity.CreateMemberId = _accessor.MemberId;
            entity.CreateTime = DateTime.Now;
            entity.ProjectId = input.ProjectId;
            entity.MembershipId = input.MembershipId;
            entity.Date = input.Date;
            entity.Duration = input.Duration;
            entity.IsOvertime = input.IsOvertime;
            entity.Description = input.Description;

            _database.Update(entity);
            return true;
        }
    }
}
