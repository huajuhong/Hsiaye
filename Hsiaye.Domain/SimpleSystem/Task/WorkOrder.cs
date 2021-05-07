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
        public byte ProgressPercent { get; set; }//进度百分比：0-100，100表示已完成
        public string ProgressContent { get; set; }//进度内容，每次提交会自动在提交内容后加换行符 \n 追加在该字段后面
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
        关闭 = 0,
        激活 = 1,
        处理中 = 2,
        已处理 = 3,
    }
}
