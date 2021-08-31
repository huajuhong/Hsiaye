using DapperExtensions;
using DapperExtensions.Predicate;
using Hsiaye.Application.Contracts;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    /// <summary>
    /// 自习室
    /// 使用教程：
    /// 1.添加座位类型
    /// 2.添加座位
    /// 3.座位预约，预约规则：不可在相同时间段内预约
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SelfStudyRoomController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly IAccessor _accessor;

        public SelfStudyRoomController(IDatabase database, IAccessor accessor)
        {
            _database = database;
            _accessor = accessor;
        }

        #region 座位学科 管理
        [HttpPost]
        public bool SeatSubject_Create(SeatSubject input)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatSubject>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatSubject>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<SeatSubject>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            input.OrganizationUnitId = _accessor.OrganizationUnitId;
            input.CreateTime = DateTime.Now;
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool SeatSubject_Update(SeatSubject input)
        {
            var model = _database.Get<SeatSubject>(new { input.Id });
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("非法请求");
            }
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatSubject>(f => f.Id, Operator.Eq,input.Id,true),
                Predicates.Field<SeatSubject>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatSubject>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<SeatSubject>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            model.Name = input.Name;
            model.Description = input.Description;
            model.Normal = input.Normal;

            _database.Update(model);
            return true;
        }

        [HttpPost]
        public PageResult<SeatSubject> SeatSubject_List(KeywordsListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<SeatSubject>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatSubject>(e => e.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<SeatSubject>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<SeatSubject>(x => x.CreateTime) };
            var list = _database.GetPage<SeatSubject>(predicateGroup, sort, input.PageIndex, input.PageSize);

            var count = _database.Count<SeatSubject>(predicateGroup);
            return new PageResult<SeatSubject>(list, count);
        }

        [HttpPost]
        public IEnumerable<SeatSubject> SeatSubject_Options()
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<SeatSubject>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            predicateGroup.Predicates.Add(Predicates.Field<SeatSubject>(f => f.Normal, Operator.Eq, true));
            var sort = new List<ISort> { Predicates.Sort<SeatSubject>(x => x.CreateTime) };
            var list = _database.GetList<SeatSubject>(predicateGroup, sort);
            return list;
        }
        #endregion

        #region 座位类型 管理
        [HttpPost]
        public bool SeatCategory_Create(SeatCategory input)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatCategory>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatCategory>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<SeatCategory>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            input.OrganizationUnitId = _accessor.OrganizationUnitId;
            input.CreateTime = DateTime.Now;
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool SeatCategory_Update(SeatCategory input)
        {
            var model = _database.Get<SeatCategory>(new { input.Id });
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("非法请求");
            }
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatCategory>(f => f.Id, Operator.Eq,input.Id,true),
                Predicates.Field<SeatCategory>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatCategory>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<SeatCategory>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            model.Name = input.Name;
            model.BeginTime = input.BeginTime;
            model.EndTime = input.EndTime;
            model.Price = input.Price;
            model.Description = input.Description;
            model.Normal = input.Normal;

            _database.Update(model);
            return true;
        }

        [HttpPost]
        public PageResult<SeatCategory> SeatCategory_List(KeywordsListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<SeatCategory>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatCategory>(e => e.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<SeatCategory>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<SeatCategory>(x => x.CreateTime) };
            var list = _database.GetPage<SeatCategory>(predicateGroup, sort, input.PageIndex, input.PageSize);

            var count = _database.Count<SeatCategory>(predicateGroup);
            return new PageResult<SeatCategory>(list, count);
        }

        [HttpPost]
        public IEnumerable<SeatCategory> SeatCategory_Options()
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<SeatCategory>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            predicateGroup.Predicates.Add(Predicates.Field<SeatCategory>(f => f.Normal, Operator.Eq, true));
            var sort = new List<ISort> { Predicates.Sort<SeatCategory>(x => x.CreateTime) };
            var list = _database.GetList<SeatCategory>(predicateGroup, sort);
            return list;
        }
        #endregion

        #region 座位 管理
        [HttpPost]
        public bool Seat_Create(Seat input)
        {
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<Seat>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<Seat>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<Seat>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }
            input.OrganizationUnitId = _accessor.OrganizationUnitId;
            input.CreateTime = DateTime.Now;
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool Seat_Update(Seat input)
        {
            var model = _database.Get<Seat>(new { input.Id });
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("非法请求");
            }
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<Seat>(f => f.Id, Operator.Eq,input.Id,true),
                Predicates.Field<Seat>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<Seat>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<Seat>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            model.SeatCategoryId = input.SeatCategoryId;
            model.Name = input.Name;
            model.Description = input.Description;
            model.Normal = input.Normal;

            _database.Update(model);
            return true;
        }

        [HttpPost]
        public PageResult<Seat> Seat_List(KeywordsListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<Seat>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<Seat>(e => e.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<Seat>(e => e.Description, Operator.Like, input.Keywords));
            }

            var sort = new List<ISort> { Predicates.Sort<Seat>(x => x.CreateTime) };

            var list = _database.GetPage<Seat>(predicateGroup, sort, input.PageIndex, input.PageSize);
            //todo:1和2交换位置会报错
            //1
            var count = _database.Count<Seat>(predicateGroup);
            //2
            foreach (var item in list)
            {
                MapToEntity(item);
            }
            return new PageResult<Seat>(list, count);
        }

        private void MapToEntity(Seat model)
        {
            if (model == null)
            {
                return;
            }
            //座位类型
            var seatCategory = _database.Get<SeatCategory>(Predicates.Field<SeatCategory>(f => f.Id, Operator.Eq, model.SeatCategoryId));
            model.SeatCategory = seatCategory;
        }

        [HttpPost]
        public IEnumerable<Seat> Seat_Options()
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.Normal, Operator.Eq, true));
            var sort = new List<ISort> { Predicates.Sort<Seat>(x => x.CreateTime) };
            var list = _database.GetList<Seat>(predicateGroup, sort);
            foreach (var item in list)
            {
                MapToEntity(item);
            }
            return list;
        }
        #endregion

        #region 座位预约 管理

        [HttpPost]
        public bool SeatReservation_Create(SeatReservation input)
        {
            var seat = _database.Get<Seat>(Predicates.Field<Seat>(f => f.Id, Operator.Eq, input.SeatId));

            var seatCategory = _database.Get<SeatCategory>(Predicates.Field<SeatCategory>(f => f.Id, Operator.Eq, seat.SeatCategoryId));

            //预约成功的条件：选中的座位在选择的时段内没有预约记录
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatReservation>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatReservation>(f => f.SeatId, Operator.Eq, input.SeatId),
                Predicates.Field<SeatReservation>(f => f.Normal, Operator.Eq, true),
            };


            //时间条件，输入时间时段不可有记录才可预约成功
            List<IPredicate> predicates1 = new List<IPredicate>
            {
                Predicates.Field<SeatReservation>(f => f.Begin, Operator.Gt, input.Begin.Add(seatCategory.BeginTime)),
                Predicates.Field<SeatReservation>(f => f.End, Operator.Lt, input.End.Add(seatCategory.EndTime)),
            };
            predicates.Add(Predicates.Group(GroupOperator.Or, predicates1.ToArray()));

            var model = _database.Get<SeatReservation>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (model != null)
            {
                throw new UserFriendlyException($"预约失败（该座位已被预约【姓名：{model.Name}；电话：{model.Phone}；时段：{model.Begin:yyyyMMdd HHmm}至{model.End:yyyyMMdd HHmm}】）");
            }
            input.OrganizationUnitId = _accessor.OrganizationUnitId;
            input.CreateTime = DateTime.Now;
            input.OperatorId = _accessor.MemberId;
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool SeatReservation_Update(SeatReservation input)
        {
            var model = _database.Get<SeatReservation>(new { input.Id });
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException("非法请求");
            }
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatReservation>(f => f.Id, Operator.Eq,input.Id,true),
                Predicates.Field<SeatReservation>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatReservation>(f => f.Name, Operator.Eq, input.Name),
            };
            int count = _database.Count<SeatReservation>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            model.SeatId = input.SeatId;
            model.SeatSubjectId = input.SeatSubjectId;
            model.Begin = input.Begin;
            model.End = input.End;
            model.Description = input.Description;
            model.OperatorId = _accessor.MemberId;
            model.OperatorRemark = input.OperatorRemark;
            model.Normal = input.Normal;
            model.Reported = input.Reported;

            _database.Update(model);
            return true;
        }

        [HttpPost]
        public PageResult<SeatReservation> SeatReservation_List(KeywordsListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<SeatReservation>(x => x.CreateTime) };
            var list = _database.GetPage<SeatReservation>(predicateGroup, sort, input.PageIndex, input.PageSize);
            var count = _database.Count<SeatReservation>(predicateGroup);
            foreach (var item in list)
            {
                MapToEntity(item);
            }
            return new PageResult<SeatReservation>(list, count);
        }

        private void MapToEntity(SeatReservation model)
        {
            if (model == null)
            {
                return;
            }

            //座位
            var seat = _database.Get<Seat>(Predicates.Field<Seat>(f => f.Id, Operator.Eq, model.SeatId));
            model.Seat = seat;

            //座位科目
            var seatSubject = _database.Get<SeatSubject>(Predicates.Field<Seat>(f => f.Id, Operator.Eq, model.SeatSubjectId));
            model.SeatSubject = seatSubject;

            //座位类型
            MapToEntity(seat);
        }
        #endregion
    }
}
