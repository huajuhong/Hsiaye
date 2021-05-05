using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //工时表
    public class Timesheet
    {
        public int Id { get; set; }
        public int CreateMemberId { get; set; }
        public DateTime CreateTime { get; set; }
        public int ProjectId { get; set; }
        public int MembershipId { get; set; }
        public DateTime Date { get; set; }//日期，精确到天
        public TimeSpan Duration { get; set; }//时长，精确到分
        public bool IsOvertime { get; set; }//是否为加班
        public string Description { get; set; }//说明
    }
}
