﻿using System;
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
        public int Id { get; set; }
        public int OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public int SeatId { get; set; }//座位Id
        [StringLength(64)]
        public string Name { get; set; }//姓名
        [StringLength(64)]
        public string Phone { get; set; }//电话
        public DateTime Begin { get; set; }//预约开始时间
        public DateTime End { get; set; }//预约结束时间
        public string Subject { get; set; }//科目
        [StringLength(256)]
        public string Description { get; set; }
        public int OperatorId { get; set; }//操作者Id
        [StringLength(64)]
        public string OperatorRemark { get; set; }//操作者备注
        public bool Normal { get; set; }//默认true,取消false,预约true
        public bool Reported { get; set; }//默认false,未签到false,已签到true

        public Seat Seat { get; set; }
    }
}