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
            var model = _database.Get<SeatCategory>(input.Id);
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException($"非法请求");
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
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicate.Predicates.Add(Predicates.Field<SeatCategory>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate.Predicates.Add(Predicates.Field<SeatCategory>(e => e.Name, Operator.Like, input.Keywords));
                predicate.Predicates.Add(Predicates.Field<SeatCategory>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<SeatCategory>(x => x.CreateTime) };
            var list = _database.GetPage<SeatCategory>(Predicates.Group(GroupOperator.Or, predicate.Predicates.ToArray()), sort, input.PageIndex, input.PageSize);

            var count = _database.Count<SeatCategory>(predicate);
            return new PageResult<SeatCategory>(list, count);
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
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool Seat_Update(Seat input)
        {
            var model = _database.Get<Seat>(input.Id);
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException($"非法请求");
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
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicate.Predicates.Add(Predicates.Field<Seat>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate.Predicates.Add(Predicates.Field<Seat>(e => e.Name, Operator.Like, input.Keywords));
                predicate.Predicates.Add(Predicates.Field<Seat>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<Seat>(x => x.CreateTime) };
            var list = _database.GetPage<Seat>(Predicates.Group(GroupOperator.Or, predicate.Predicates.ToArray()), sort, input.PageIndex, input.PageSize);

            var count = _database.Count<Seat>(predicate);
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
            input.OperatorId = _accessor.MemberId;
            input.Normal = true;
            input.OrganizationUnitId = _accessor.OrganizationUnitId;
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool SeatReservation_Update(SeatReservation input)
        {
            var model = _database.Get<SeatReservation>(input.Id);
            if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            {
                throw new UserFriendlyException($"非法请求");
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
            IPredicateGroup predicate = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicate.Predicates.Add(Predicates.Field<SeatReservation>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Name, Operator.Like, input.Keywords));
                predicate.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Description, Operator.Like, input.Keywords));
            }
            var sort = new List<ISort> { Predicates.Sort<SeatReservation>(x => x.CreateTime) };
            var list = _database.GetPage<SeatReservation>(Predicates.Group(GroupOperator.Or, predicate.Predicates.ToArray()), sort, input.PageIndex, input.PageSize);
            foreach (var item in list)
            {
                MapToEntity(item);
            }
            var count = _database.Count<SeatReservation>(predicate);
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

            //座位类型
            MapToEntity(seat);
        }
        #endregion
    }
}
