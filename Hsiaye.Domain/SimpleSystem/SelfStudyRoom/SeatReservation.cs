using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 座位预约
    /// </summary>
    public class SeatReservation
    {
        public int Id { get; set; }
        public int OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public int SeatCategoryId { get; set; }
        public string Name { get; set; }//姓名
        public string Phone { get; set; }//电话
        public DateTime Time { get; set; }//预约时间
        public string Subject { get; set; }//科目
        public string Description { get; set; }
        public SeatReservationState State { get; set; }
        public int OperatorId { get; set; }//操作者Id
        public string OperatorRemark { get; set; }//操作者备注
    }
    public enum SeatReservationState
    {
        无效,
        预约,
        取消,
    }
}
