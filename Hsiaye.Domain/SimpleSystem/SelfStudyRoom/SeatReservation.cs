using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 座位预约
    /// 页面内容：
    /// 选择座位，不同座位有不同的时间段
    /// 姓名、电话、选择日期、选择科目
    /// </summary>
    public class SeatReservation
    {
        public long Id { get; set; }
        public long OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public long SeatId { get; set; }//座位Id
        public long SeatSubjectId { get; set; }//座位科目Id
        [StringLength(64)]
        public string Name { get; set; }//姓名
        [StringLength(64)]
        public string Phone { get; set; }//电话
        public DateTime Begin { get; set; }//预约开始时间
        public DateTime End { get; set; }//预约结束时间
        [StringLength(256)]
        public string Description { get; set; }
        public long OperatorId { get; set; }//操作者Id
        [StringLength(64)]
        public string OperatorRemark { get; set; }//操作者备注
        public bool Normal { get; set; }//默认true,取消false,预约true
        public bool Reported { get; set; }//默认false,未签到false,已签到true
        public bool Deleted { get; set; }

        public Seat Seat { get; set; }
        public SeatSubject SeatSubject { get; set; }
        
    }
    public class SeatReservationMap : ClassMapper<SeatReservation>
    {
        public SeatReservationMap()
        {
            Map(t => t.Seat).Ignore();
            Map(t => t.SeatSubject).Ignore();
            AutoMap();
        }
    }
}
