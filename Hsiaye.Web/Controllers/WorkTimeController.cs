using Hsiaye.Application;
using Hsiaye.Application.Contracts;
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
        /// <summary>
        /// 工时添加（记工时）
        /// 左侧：穿梭框，获取当前组成成员列表（名字+电话）
        /// 右侧：表单，选择项目/选择日期（默认今天）/选择时长/是否加班/说明
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public bool Create(WorkTimeInput input)
        {
            WorkTimeSalary workTimeSalary = _database.GetList<WorkTimeSalary>(Predicates.Field<WorkTimeSalary>(f => f.MembershipId, Operator.Eq, input.MembershipId)).FirstOrDefault();

            WorkTime entity = new WorkTime
            {
                CreateMemberId = _accessor.MemberId,
                CreateTime = DateTime.Now,
                ProjectId = input.ProjectId,
                MembershipId = input.MembershipId,
                Date = input.Date,
                Duration = input.Duration,
                Overtime = input.Overtime,
                Salary = workTimeSalary.Amount,
                Description = input.Description,
            };

            _database.Insert(entity);

            return true;
        }

        [HttpPost]
        [Authorize(PermissionNames.工时)]
        public PageResult<WorkTime> List(WorkTimeListInput input)
        {
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            if (input.ProjectId > 0)
            {
                predicate.Predicates.Add(Predicates.Field<WorkTime>(f => f.ProjectId, Operator.Eq, input.ProjectId));
            }
            if (input.MembershipId > 0)
            {
                predicate.Predicates.Add(Predicates.Field<WorkTime>(f => f.MembershipId, Operator.Eq, input.MembershipId));
            }
            if (input.Overtime != WorkTimeOvertime.未知)
            {
                predicate.Predicates.Add(Predicates.Field<WorkTime>(f => f.Overtime, Operator.Eq, input.Overtime));
            }

            IList<ISort> sort = new List<ISort> { Predicates.Sort<WorkTime>(f => f.Id, false) };
            var list = _database.GetPage<WorkTime>(predicate, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<WorkTime>(predicate);
            return new PageResult<WorkTime>(list, count);
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
            entity.Overtime = input.Overtime;
            entity.Description = input.Description;

            _database.Update(entity);
            return true;
        }
    }
}
