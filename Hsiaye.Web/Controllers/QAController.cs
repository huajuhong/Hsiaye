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
                CategoryId = input.CategoryId,
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
            entity.ViewCount += 1;
            _database.Update(entity);
            return entity;
        }
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(PermissionNames.问答)]
        public PageResult<Question> ListQuestion(QuestionListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
            };

            predicateGroup.Predicates = new List<IPredicate>
            {
                Predicates.Field<Question>(f => f.Deleted, Operator.Eq, false),
                Predicates.Field<Question>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
            };

            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<Question>(f => f.Title, Operator.Like, input.Keywords));
                //predicateGroup.Predicates.Add(Predicates.Field<Question>(f => f.Description, Operator.Like, input.Keywords));
            }
            if (input.CategoryId > 0)
            {
                predicateGroup.Predicates.Add(Predicates.Field<Question>(f => f.CategoryId, Operator.Eq, input.CategoryId));
            }

            List<ISort> sort = new List<ISort>();
            switch (input.SortField)
            {
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
                    sort.Add(Predicates.Sort<Question>(f => f.Id, false));
                    break;
            }

            var list = _database.GetPage<Question>(predicateGroup, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<Question>(predicateGroup);
            return new PageResult<Question>(list, count);
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

            entity.Title = input.Title;
            entity.CategoryId = input.CategoryId;
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
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public PageResult<Answer> ListAnswer(AnswerListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<Answer>(f => f.QuestionId, Operator.Eq, input.QuestionId));
            predicateGroup.Predicates.Add(Predicates.Field<Answer>(f => f.Deleted, Operator.Eq, false));
            List<ISort> sort = new List<ISort>();
            switch (input.SortField)
            {
                case "LikeCount":
                    sort.Add(Predicates.Sort<Answer>(f => f.LikeCount, false));
                    break;
                default:
                    sort.Add(Predicates.Sort<Answer>(f => f.Id, false));
                    break;
            }

            var list = _database.GetPage<Answer>(predicateGroup, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<Answer>(predicateGroup);
            return new PageResult<Answer>(list, count);
        }

        /// <summary>
        /// 对问题投票，表示遇到该问题且支持
        /// </summary>
        /// <param name="id">问题Id</param>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public void VoteQuestion(int id)
        {
            var entity = _database.Get<Question>(id);
            entity.VoteCount += 1;
            _database.Update(entity);
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

        /// <summary>
        /// 对答案认可，点赞表时喜欢
        /// </summary>
        /// <param name="id">答案Id</param>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public void LikeAnswer(int id)
        {
            var entity = _database.Get<Answer>(id);
            entity.LikeCount += 1;
            _database.Update(entity);
        }

        /// <summary>
        /// 采纳答案
        /// </summary>
        /// <param name="id">答案Id</param>
        [HttpPost]
        [Authorize(PermissionNames.问答)]
        public void AcceptedAnswer(int id)
        {
            var answer = _database.Get<Answer>(id);
            if (answer.Accepted)
            {
                return;
            }
            answer.Accepted = true;
            _database.Update(answer);
            var question = _database.Get<Question>(answer.QuestionId);
            if (question.AnswerId < 1)
            {
                question.AnswerId = answer.Id;
                _database.Update(question);
            }
        }
    }
}
