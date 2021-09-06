﻿using DapperExtensions;
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

        /// <summary>
        /// 可预约的座位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<Seat> Seat_Options(DateTime begin, DateTime end)
        {
            IPredicateGroup predicateGroup = new PredicateGroup()
            {
                Operator = GroupOperator.And,
                Predicates = new List<IPredicate>()
            };
            //predicateGroup.Predicates.Add(Predicates.Field<Seat>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId));
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
                //Predicates.Field<SeatReservation>(f => f.OrganizationUnitId, Operator.Eq, _accessor.OrganizationUnitId),
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

                    if (seatCategory.BeginTime >= item.Seat.SeatCategory.BeginTime && seatCategory.BeginTime <= item.Seat.SeatCategory.EndTime)
                    {
                        //开始时间冲突
                        errorMessage.AppendLine($"开始时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.SeatCategory.BeginTime}-{item.Seat.SeatCategory.EndTime}时段。");
                    }
                    if (seatCategory.EndTime >= item.Seat.SeatCategory.BeginTime && seatCategory.EndTime <= item.Seat.SeatCategory.EndTime)
                    {
                        //结束时间冲突
                        errorMessage.AppendLine($"结束时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.SeatCategory.BeginTime}-{item.Seat.SeatCategory.EndTime}时段。");
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
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Description, Operator.Like, input.Keywords));
            }
            if (input.ReservationDate.HasValue)
            {
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.Begin, Operator.Le, input.ReservationDate.Value));
                predicateGroup.Predicates.Add(Predicates.Field<SeatReservation>(e => e.End, Operator.Ge, input.ReservationDate.Value));
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

                    if (seatCategory.BeginTime >= item.Seat.SeatCategory.BeginTime && seatCategory.BeginTime <= item.Seat.SeatCategory.EndTime)
                    {
                        //开始时间冲突
                        errorMessage.AppendLine($"开始时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.SeatCategory.BeginTime}-{item.Seat.SeatCategory.EndTime}时段。");
                    }
                    if (seatCategory.EndTime >= item.Seat.SeatCategory.BeginTime && seatCategory.EndTime <= item.Seat.SeatCategory.EndTime)
                    {
                        //结束时间冲突
                        errorMessage.AppendLine($"结束时间冲突：{item.Name}-{item.Phone}已预约{item.Begin:yyyyMMdd}至{item.End:yyyyMMdd}期间的{item.Seat.SeatCategory.BeginTime}-{item.Seat.SeatCategory.EndTime}时段。");
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
        #endregion
    }
}
