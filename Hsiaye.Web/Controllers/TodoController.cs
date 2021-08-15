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
    /// 待办
    /// todo:未完
    /// 1.简单添加，详细编辑
    /// 2.详细列表
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public TodoController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }

        [HttpPost]
        [Authorize(PermissionNames.待办)]
        public bool Create(TodoInput input)
        {
            Todo entity = new Todo
            {
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                Title = input.Title,
                CategoryId = input.CategoryId,
                Tag = input.Tag,
                ExpireTime = input.ExpireTime,
                ParentId = input.ParentId,
                Priority = input.Priority,
                Remind = input.Remind,
                ReminderTime = input.ReminderTime,
                RepeatType = input.RepeatType,
            };

            var predicates = new IPredicate[]
            {
                Predicates.Field<Todo>(f => f.OrganizationUnitId, Operator.Eq, entity.OrganizationUnitId),
                Predicates.Field<Todo>(f => f.Title, Operator.Eq, entity.Title)
            };
            int count = _database.Count<Todo>(Predicates.Group(GroupOperator.And, predicates));
            if (count > 1)
            {
                throw new UserFriendlyException("该标题已存在");
            }

            _database.Insert(entity);

            return true;
        }

        [HttpPost]
        [Authorize(PermissionNames.待办)]
        public PageResult<Todo> List(TodoListInput input)
        {
            var predicates = new List<IPredicate>();
            if (_accessor.Member.UserName != PermissionNames.AdminUserName)
            {
                predicates.Add(Predicates.Field<Todo>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            }
            if (!string.IsNullOrEmpty(input.Title))
            {
                predicates.Add(Predicates.Field<Todo>(f => f.Title, Operator.Like, input.Title));
            }
            if (input.Priority > 0)
            {
                predicates.Add(Predicates.Field<Todo>(f => f.Priority, Operator.Eq, input.Priority));
            }
            if (input.DistributeToMemberId > 0)
            {
                predicates.Add(Predicates.Field<Todo>(f => f.DistributeToMemberId, Operator.Eq, input.DistributeToMemberId));
            }
            if (input.CategoryId > 0)
            {
                predicates.Add(Predicates.Field<Todo>(f => f.CategoryId, Operator.Eq, input.CategoryId));
            }
            if (!string.IsNullOrEmpty(input.Tag))
            {
                predicates.Add(Predicates.Field<Todo>(f => f.Tag, Operator.Eq, input.Tag));
            }
            if (input.State != TodoState.未知)
            {
                predicates.Add(Predicates.Field<Todo>(f => f.State, Operator.Eq, input.State));
            }
            var pageResult = _database.GetPaged<Todo>(Predicates.Group(GroupOperator.And, predicates.ToArray()),
                new List<ISort> { Predicates.Sort<Todo>(f => f.Id, false) }, input.PageIndex, input.PageSize);
            return pageResult;
        }

        [HttpGet]
        [Authorize(PermissionNames.待办)]
        public Todo Get(long id)
        {
            var predicates = new IPredicate[]
            {
                Predicates.Field<Todo>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<Todo>(f => f.Id, Operator.Eq, id)
            };
            var entity = _database.GetList<Todo>(Predicates.Group(GroupOperator.And, predicates.ToArray())).FirstOrDefault();
            return entity;
        }

        [HttpPost]
        [Authorize(PermissionNames.待办)]
        public bool Update(TodoEditInput input)
        {
            return true;
        }
    }
}
