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
    /// 问答
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QAController : ControllerBase
    {
        private readonly IAccessor _accessor;
        private readonly IDatabase _database;

        public QAController(IAccessor accessor, IDatabase database)
        {
            _accessor = accessor;
            _database = database;
        }
        /// <summary>
        /// 创建问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public bool CreateQuestion(QuestionInput input)
        {
            Question entity = new Question
            {
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                MemberId = _accessor.MemberId,
                Title = input.Title,
                Description = input.Description,
                AnswerId = 0,
                VoteCount = 0,
                AnswerCount = 0,
                ViewCount = 0,
            };

            _database.Insert(entity);

            return true;
        }
        /// <summary>
        /// 获取问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionNames.问答)]
        public Question GetQuestion(long id)
        {
            var entity = _database.Get<Question>(id);
            return entity;
        }
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="sortField"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionNames.问答)]
        public PageResult<Question> ListQuestion(string keywords, string sortField, int page, int limit)
        {
            var predicates = new List<IPredicate>
            {
                Predicates.Field<Question>(f => f.Deleted, Operator.Eq, false),
                Predicates.Field<Question>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
            };

            if (!string.IsNullOrEmpty(keywords))
            {
                predicates.Add(Predicates.Field<Question>(f => f.Title, Operator.Like, keywords));
                //predicates.Add(Predicates.Field<Question>(f => f.Description, Operator.Like, keywords));
            }

            List<ISort> sort = new List<ISort>();
            switch (sortField)
            {
                case "Id":
                    sort.Add(Predicates.Sort<Question>(f => f.Id, false));
                    break;
                case "VoteCount":
                    sort.Add(Predicates.Sort<Question>(f => f.VoteCount, false));
                    break;
                case "AnswerCount":
                    sort.Add(Predicates.Sort<Question>(f => f.AnswerCount, false));
                    break;
                case "ViewCount":
                    sort.Add(Predicates.Sort<Question>(f => f.ViewCount, false));
                    break;
                default:
                    break;
            }

            var pageResult = _database.GetPaged<Question>(Predicates.Group(GroupOperator.And, predicates.ToArray()), sort, page, limit);
            return pageResult;
        }

        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public bool UpdateQuestion(QuestionEditInput input)
        {
            Question entity = _database.Get<Question>(input.Id);
            if (entity.AnswerId > 0)
            {
                throw new UserFriendlyException("问题已有采纳回答");
            }
            if (entity.MemberId != _accessor.MemberId)
            {
                throw new UserFriendlyException("非法操作");
            }

            entity.Description = input.Description;

            _database.Update(entity);
            return true;
        }
        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public bool DeleteQuestion(long id)
        {
            Question entity = _database.Get<Question>(id);

            if (entity.MemberId != _accessor.MemberId)
            {
                throw new UserFriendlyException("非法操作");
            }

            entity.Deleted = true;

            _database.Update(entity);

            return true;
        }
        /// <summary>
        /// 获取问题答案列表
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public List<Answer> ListAnswer(long questionId, int page, int limit)
        {
            var predicates = new List<IPredicate>();
            predicates.Add(Predicates.Field<Answer>(f => f.QuestionId, Operator.Eq, questionId));
            predicates.Add(Predicates.Field<Answer>(f => f.Deleted, Operator.Eq, false));

            var list = _database.GetPage<Answer>(Predicates.Group(GroupOperator.And, predicates.ToArray()), new List<ISort> { Predicates.Sort<Answer>(f => f.Id, false) }, page, limit).ToList();

            return list;
        }
        /// <summary>
        /// 创建答案
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public bool CreateAnswer(AnswerInput input)
        {
            Answer entity = new Answer
            {
                CreateTime = DateTime.Now,
                OrganizationUnitId = _accessor.OrganizationUnitId,
                MemberId = _accessor.MemberId,
                QuestionId = input.QuestionId,
                Description = input.Description,
                LikeCount = 0,
            };

            _database.Insert(entity);

            return true;
        }
        /// <summary>
        /// 获取答案
        /// </summary>
        /// <param name="id">答案id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionNames.问答)]
        public Answer GetAnswer(long id)
        {
            var entity = _database.Get<Answer>(id);
            return entity;
        }
        /// <summary>
        /// 修改答案
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public bool UpdateAnswer(AnswerEditInput input)
        {
            Answer entity = _database.Get<Answer>(input.Id);
            if (entity.Accepted)
            {
                throw new UserFriendlyException("回答已被采纳");
            }
            if (entity.MemberId != _accessor.MemberId)
            {
                throw new UserFriendlyException("非法操作");
            }

            entity.Description = input.Description;

            _database.Update(entity);
            return true;
        }
        /// <summary>
        /// 删除答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public bool DeleteAnswer(long id)
        {
            Answer entity = _database.Get<Answer>(id);

            if (entity.Accepted)
            {
                throw new UserFriendlyException("回答已被采纳");
            }
            if (entity.MemberId != _accessor.MemberId)
            {
                throw new UserFriendlyException("非法操作");
            }

            entity.Deleted = true;

            _database.Update(entity);

            return true;
        }
    }
}
