using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    // 工单系统
    public class WorkOrder
    {
        public int Id { get; set; }
        public int OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public int Number { get; set; }//工单号
        public WorkOrderType Type { get; set; }//业务性质
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubmitMemberId { get; set; }//提交人员Id
        public int AcceptMemberId { get; set; }//受理人员Id
        public WorkOrderState State { get; set; }
    }
    public enum WorkOrderType : byte
    {
        公告 = 1,
        讨论 = 2,
        分享 = 3,
        提问 = 4,
    }
    public enum WorkOrderState : byte
    {
        激活 = 1,
        处理中 = 2,
        已处理 = 3,
    }
}
