using DapperExtensions;
using DapperExtensions.Predicate;
using Hsiaye.Application.Contracts;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Web.Controllers
{
    public class SeatSubject_ListInput : KeywordsListInput
    {
        public long SeatId { get; set; }
        public long SeatCategoryId { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
    /// <summary>
    /// 自习室
    /// 使用教程：
    /// 1.座位科目、类型管理
    /// 2.添加座位
    /// 3.座位预约，预约规则：不可在相同时间段内预约
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SelfStudyRoomController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly IAccessor _accessor;
        private readonly IMemoryCache _cache;

        public SelfStudyRoomController(IDatabase database, IAccessor accessor, IMemoryCache cache)
        {
            _database = database;
            _accessor = accessor;
            _cache = cache;
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
        public bool SeatSubject_Delete(long id)
        {
            var model = _database.Get<SeatSubject>(new { Id = id });
            //if (model.Deleted == true || model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatReservation>(f => f.SeatSubjectId, Operator.Eq, id),
                Predicates.Field<SeatReservation>(f => f.Normal, Operator.Eq, true),
            };
            int count = _database.Count<SeatReservation>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException("该学科已被现有座位预约使用，不可删除，只可编辑");
            }
            _database.Delete(model);
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
            //predicateGroup.Predicates.Add(Predicates.Field<SeatSubject>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
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
        public bool SeatCategory_Delete(long id)
        {
            var model = _database.Get<SeatCategory>(new { Id = id });
            //if (model.Deleted == true || model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<Seat>(f => f.SeatCategoryId, Operator.Eq, id),
                Predicates.Field<Seat>(f => f.Normal, Operator.Eq, true),
            };
            int count = _database.Count<Seat>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException("该分类已被现有座位使用，不可删除");
            }
            _database.Delete(model);
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
            //predicateGroup.Predicates.Add(Predicates.Field<SeatCategory>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
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
                Predicates.Field<Seat>(f => f.SeatCategoryId, Operator.Eq, input.SeatCategoryId),
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
        public bool Seat_Delete(long id)
        {
            var model = _database.Get<Seat>(new { Id = id });
            //if (model.Deleted == true || model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            List<IPredicate> predicates = new List<IPredicate>
            {
                Predicates.Field<SeatReservation>(f => f.SeatId, Operator.Eq, id),
                Predicates.Field<SeatReservation>(f => f.Normal, Operator.Eq, true),
                Predicates.Field<SeatReservation>(f => f.Reported, Operator.Eq, false),
            };
            int count = _database.Count<SeatReservation>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException("该座位已有预约记录，不可删除");
            }
            _database.Delete(model);
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
                Predicates.Field<Seat>(f => f.Id, Operator.Eq, input.Id, true),
                Predicates.Field<Seat>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<Seat>(f => f.Name, Operator.Eq, input.Name),
                Predicates.Field<Seat>(f => f.SeatCategoryId, Operator.Eq, input.SeatCategoryId),
            };
            int count = _database.Count<Seat>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (count > 0)
            {
                throw new UserFriendlyException($"已存在：{input.Name}");
            }

            model.SeatCategoryId = input.SeatCategoryId;
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

        /// <summary>
        /// 可预约的座位
        /// </summary>
        /// <param name="seatCategoryId">座位分类Id</param>
        /// <param name="begin">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Seat> Seat_Options(long seatCategoryId, DateTime? begin, DateTime? end)
        {
            //查询所有座位
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            //predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.SeatCategoryId, Operator.Eq, seatCategoryId));
            predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.Normal, Operator.Eq, true));
            var sort = new List<ISort> { Predicates.Sort<Seat>(x => x.CreateTime) };
            var list = _database.GetList<Seat>(predicateGroup, sort);
            foreach (var item in list)
            {
                MapToEntity(item);
            }

            if (begin.HasValue && end.HasValue)
            {
                //查询已被预约的座位
                predicateGroup = new PredicateGroup()
                {
                    Operator = GroupOperator.And,
                    Predicates = new List<IPredicate>()
                };
                predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.SeatCategoryId, Operator.Eq, seatCategoryId));
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(f => f.Normal, Operator.Eq, true));
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(f => f.Deleted, Operator.Eq, false));

                //时间条件：查找输入的开始时间和结束时间范围内是否有记录，包含临界值

                IPredicateGroup beginPredicate = new PredicateGroup()
                {
                    Operator = GroupOperator.And,
                    Predicates = new List<IPredicate>()
                };
                beginPredicate.Predicates.Add(Predicates.Field<SeatReservation>(f => f.Begin, Operator.Le, begin));
                beginPredicate.Predicates.Add(Predicates.Field<SeatReservation>(f => f.End, Operator.Ge, begin));
                IPredicateGroup endPredicate = new PredicateGroup()
                {
                    Operator = GroupOperator.And,
                    Predicates = new List<IPredicate>()
                };
                endPredicate.Predicates.Add(Predicates.Field<SeatReservation>(f => f.Begin, Operator.Le, end));
                endPredicate.Predicates.Add(Predicates.Field<SeatReservation>(f => f.End, Operator.Ge, end));
                IPredicateGroup betweenPredicate = Predicates.Group(GroupOperator.Or, beginPredicate, endPredicate);
                predicateGroup.Predicates.Add(betweenPredicate);

                var reservations = _database.GetList<SeatReservation>(predicateGroup);
                if (reservations.Any())
                {
                    var reservationSeatIds = reservations.Select(x => x.SeatId);
                    foreach (var item in list)
                    {
                        if (reservationSeatIds.Contains(item.Id))
                        {
                            //用作界面不可选标记
                            item.Description = "disabled";
                        }
                    }
                }
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
                //Predicates.Field<SeatReservation>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
                Predicates.Field<SeatReservation>(f => f.Deleted, Operator.Eq, false),
                Predicates.Field<SeatReservation>(f => f.SeatId, Operator.Eq, input.SeatId),
                Predicates.Field<SeatReservation>(f => f.Normal, Operator.Eq, true),
            };

            //时间条件：查找输入的开始时间和结束时间范围内是否有记录，包含临界值
            predicates.Add(Predicates.Field<SeatReservation>(f => f.Begin, Operator.Le, input.Begin));
            predicates.Add(Predicates.Field<SeatReservation>(f => f.End, Operator.Ge, input.Begin));
            predicates.Add(Predicates.Field<SeatReservation>(f => f.Begin, Operator.Le, input.End));
            predicates.Add(Predicates.Field<SeatReservation>(f => f.End, Operator.Ge, input.End));


            var list = _database.GetList<SeatReservation>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (list.Any())
            {
                foreach (var item in list)
                {
                    MapToEntity(item);
                }
                StringBuilder errorMessage = new StringBuilder();
                foreach (var item in list)
                {
                    //检查时间段是否冲突
                    //如果现有时间的开始时间在

                    if (seat.BeginTime >= item.Seat.BeginTime && seat.BeginTime <= item.Seat.EndTime)
                    {
                        //开始时间冲突
                        errorMessage.AppendLine($"开始时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.BeginTime}-{item.Seat.EndTime}时段。");
                    }
                    if (seat.EndTime >= item.Seat.BeginTime && seat.EndTime <= item.Seat.EndTime)
                    {
                        //结束时间冲突
                        errorMessage.AppendLine($"结束时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.BeginTime}-{item.Seat.EndTime}时段。");
                    }
                }
                if (errorMessage.Length > 0)
                {
                    throw new UserFriendlyException($"预约失败（该座位已被预约），预约信息：" + errorMessage.ToString());
                }
            }
            input.OrganizationUnitId = _accessor.OrganizationUnitId;
            input.CreateTime = DateTime.Now;
            input.OperatorId = _accessor.MemberId;
            input.SeatCategoryId = seat.SeatCategoryId;
            _database.Insert(input);
            return true;
        }

        [HttpPost]
        public bool SeatReservation_Update(SeatReservation input)
        {
            var model = _database.Get<SeatReservation>(new { input.Id });
            //if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            model.SeatId = input.SeatId;
            var seat = _database.Get<Seat>(new { Id = model.SeatId });
            model.SeatCategoryId = seat.SeatCategoryId;
            model.SeatSubjectId = input.SeatSubjectId;
            model.ArrivalTime = input.ArrivalTime;
            model.Begin = input.Begin;
            model.End = input.End;
            model.Description = input.Description;
            model.OperatorId = _accessor.MemberId;
            model.OperatorRemark = input.OperatorRemark;
            model.Normal = input.Normal;
            model.Reported = input.Reported;
            model.Paid = input.Paid;

            _database.Update(model);
            return true;
        }

        [HttpPost]
        public bool SeatReservation_Reported(long id, bool value)
        {
            var model = _database.Get<SeatReservation>(new { Id = id });
            //if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            model.Reported = value;
            _database.Update(model);
            return true;
        }

        [HttpPost]
        public bool SeatReservation_Paid(long id, bool value)
        {
            var model = _database.Get<SeatReservation>(new { Id = id });
            //if (model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            model.Paid = value;
            _database.Update(model);
            return true;
        }

        [HttpPost]
        public bool SeatReservation_Delete(long id)
        {
            var model = _database.Get<SeatReservation>(new { Id = id });
            //if (model.Deleted == true || model.OrganizationUnitId != _accessor.OrganizationUnitId)
            //{
            //    throw new UserFriendlyException("非法请求");
            //}
            model.Deleted = true;
            _database.Update(model);
            return true;
        }

        [HttpPost]
        public PageResult<SeatReservation> SeatReservation_List(SeatSubject_ListInput input)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            //predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
            predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Deleted, Operator.Eq, false));
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Name, Operator.Like, input.Keywords));
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Phone, Operator.Like, input.Keywords));
            }
            if (input.SeatId > 0)
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.SeatId, Operator.Eq, input.SeatId));
            }
            if (input.SeatCategoryId > 0)
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.SeatCategoryId, Operator.Eq, input.SeatCategoryId));
            }

            if (input.ReservationDate.HasValue)
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Begin, Operator.Le, input.ReservationDate.Value));
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.End, Operator.Ge, input.ReservationDate.Value));
            }
            var sort = new List<ISort> { Predicates.Sort<SeatReservation>(x => x.Id, false) };
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

        //座位预约接口
        [HttpPost]
        public bool SeatReservation(SeatReservation input, string smsCode)
        {
            //校验图形验证码
            string value = _cache.Get<string>(input.Phone);
            if (string.IsNullOrEmpty(value))
                throw new UserFriendlyException("短信验证码已过期");
            if (!value.Equals(smsCode, StringComparison.OrdinalIgnoreCase))
                throw new UserFriendlyException("短信验证码错误");
            _cache.Remove(input.Phone);

            var seat = _database.Get<Seat>(Predicates.Field<Seat>(f => f.Id, Operator.Eq, input.SeatId));

            var seatCategory = _database.Get<SeatCategory>(Predicates.Field<SeatCategory>(f => f.Id, Operator.Eq, seat.SeatCategoryId));

            //预约成功的条件：选中的座位在选择的时段内没有预约记录
            List<IPredicate> predicates = new List<IPredicate>
            {
                //Predicates.Field<SeatReservation>(f => f.OrganizationUnitId, Operator.Eq, input.OrganizationUnitId),
                Predicates.Field<SeatReservation>(f => f.Deleted, Operator.Eq, false),
                Predicates.Field<SeatReservation>(f => f.SeatId, Operator.Eq, input.SeatId),
                Predicates.Field<SeatReservation>(f => f.Normal, Operator.Eq, true),
            };


            //时间条件：查找输入的开始时间和结束时间范围内是否有记录，包含临界值
            IPredicate[] predicates1 = new IPredicate[]
            {
                Predicates.Field<SeatReservation>(f => f.Begin, Operator.Ge, input.Begin),
                Predicates.Field<SeatReservation>(f => f.Begin, Operator.Le, input.End),
            };
            IPredicate[] predicates2 = new IPredicate[]
            {
                Predicates.Field<SeatReservation>(f => f.End, Operator.Ge, input.Begin),
                Predicates.Field<SeatReservation>(f => f.End, Operator.Le, input.End),
            };
            IPredicate[] predicatesByBetween = new IPredicate[]
            {
                Predicates.Group(GroupOperator.And, predicates1),
                Predicates.Group(GroupOperator.And, predicates2)
            };

            predicates.Add(Predicates.Group(GroupOperator.Or, predicatesByBetween));

            var list = _database.GetList<SeatReservation>(Predicates.Group(GroupOperator.And, predicates.ToArray()));
            if (list.Any())
            {
                foreach (var item in list)
                {
                    MapToEntity(item);
                }
                StringBuilder errorMessage = new StringBuilder();
                foreach (var item in list)
                {
                    //检查时间段是否冲突
                    //如果现有时间的开始时间在

                    if (seat.BeginTime >= item.Seat.BeginTime && seat.BeginTime <= item.Seat.EndTime)
                    {
                        //开始时间冲突
                        errorMessage.AppendLine($"开始时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.BeginTime}-{item.Seat.EndTime}时段。");
                    }
                    if (seat.EndTime >= item.Seat.BeginTime && seat.EndTime <= item.Seat.EndTime)
                    {
                        //结束时间冲突
                        errorMessage.AppendLine($"结束时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.BeginTime}-{item.Seat.EndTime}时段。");
                    }
                }
                if (errorMessage.Length > 0)
                {
                    throw new UserFriendlyException($"预约失败（该座位已被预约），预约信息：" + errorMessage.ToString());
                }
            }
            //input.OrganizationUnitId = _accessor.OrganizationUnitId;
            input.CreateTime = DateTime.Now;
            input.OperatorId = 0;
            input.SeatCategoryId = seat.SeatCategoryId;
            //补充默认字段
            input.Description = "";
            input.OperatorRemark = "";
            input.Normal = true;
            input.Reported = false;
            _database.Insert(input);
            return true;
        }
        #endregion

        #region 设置
        [HttpPost]
        public bool SetSetting(SelfStudyRoomSetInput input)
        {
            Setting setting = new Setting
            {
                CreateTime = DateTime.Now,
                ControllerName = nameof(SelfStudyRoomController),
                Name = "座位分布图",
                Value = input.SeatMap,
            };

            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<Setting>(e => e.ControllerName, Operator.Eq, setting.ControllerName));
            predicateGroup.Predicates.Add(Predicates.Field<Setting>(e => e.Name, Operator.Eq, setting.Name));

            var list = _database.GetList<Setting>(predicateGroup);
            if (list.Any())
            {
                var entity = list.First();
                entity.Value = setting.Value;
                _database.Update(entity);
            }
            else
            {
                _database.Insert(setting);
            }
            return true;
        }

        [HttpPost]
        public SelfStudyRoomSetOutput GetSetting()
        {
            SelfStudyRoomSetOutput output = new SelfStudyRoomSetOutput();
            Setting setting = new Setting
            {
                CreateTime = DateTime.Now,
                ControllerName = nameof(SelfStudyRoomController),
                Name = "座位分布图",
            };

            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<Setting>(e => e.ControllerName, Operator.Eq, setting.ControllerName));
            predicateGroup.Predicates.Add(Predicates.Field<Setting>(e => e.Name, Operator.Eq, setting.Name));
            var list = _database.GetList<Setting>(predicateGroup);
            if (list.Any())
            {
                var entity = list.First();
                output.SeatMap = entity.Value;
            }
            return output;
        }

        /// <summary>
        /// 座位分布图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string SeatMap()
        {
            Setting setting = new Setting
            {
                CreateTime = DateTime.Now,
                ControllerName = nameof(SelfStudyRoomController),
                Name = "座位分布图",
            };

            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            predicateGroup.Predicates.Add(Predicates.Field<Setting>(e => e.ControllerName, Operator.Eq, setting.ControllerName));
            predicateGroup.Predicates.Add(Predicates.Field<Setting>(e => e.Name, Operator.Eq, setting.Name));

            var list = _database.GetList<Setting>(predicateGroup);
            if (list.Any())
            {
                return list.First().Value;
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
