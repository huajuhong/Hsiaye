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
    /// 会员工时项目
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WorkTimeProjectController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public WorkTimeProjectController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Create(WorkTimeProjectInput input)
        {
            WorkTimeProject entity = new WorkTimeProject
            {
                CreateMemberId = _accessor.MemberId,
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                Name = input.Name,
                Description = input.Description,
                State = input.State,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
            };

            _database.Insert(entity);

            return true;
        }

        [HttpGet]
        [Authorize(PermissionNames.工时)]
        public List<WorkTimeProject> List(string keywords, TimesheetProjectState state, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            if (!string.IsNullOrEmpty(keywords))
            {
                predicates.Add(Predicates.Field<WorkTimeProject>(f => f.Name, Operator.Like, keywords));
            }
            if (state != TimesheetProjectState.未知)
            {
                predicates.Add(Predicates.Field<WorkTimeProject>(f => f.State, Operator.Eq, state));
            }
            var list = _database.GetPage<WorkTimeProject>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<WorkTimeProject>(f => f.Id, false) }, page, limit).ToList();
            return list;
        }

        [HttpGet]
        [Authorize(PermissionNames.工时)]
        public WorkTimeProject Get(long id)
        {
            var entity = _database.GetList<WorkTimeProject>(Predicates.Field<WorkTimeProject>(f => f.Id, Operator.Eq, id)).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Update(WorkTimeProjectEditInput input)
        {
            WorkTimeProject entity = _database.Get<WorkTimeProject>(input.Id);

            entity.CreateMemberId = _accessor.MemberId;
            entity.CreateTime = DateTime.Now;
            entity.Name = input.Name;
            entity.Description = input.Description;
            entity.State = input.State;
            entity.StartTime = input.StartTime;
            entity.EndTime = input.EndTime;
            var predicates = new IPredicate[]
            {
                Predicates.Field<WorkTimeProject>(f => f.Id, Operator.Eq, entity.Id, true),
            };

            int count = _database.Count<WorkTimeProject>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该项目已存在");
            }
            _database.Update(entity);
            return true;
        }
    }
}
