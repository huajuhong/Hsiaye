using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //工时表
    // 1.工时项目：列表/添加/编辑/删除
    // 2.工时表：列表/添加/编辑/删除
    // 3.组织机构会员工时薪资：列表/添加/编辑/删除
    //todo:重建表结构
    public class WorkTime
    {
        public int Id { get; set; }
        public int CreateMemberId { get; set; }
        public DateTime CreateTime { get; set; }
        public int ProjectId { get; set; }//WorkTimeProject的Id
        public int MembershipId { get; set; }
        public DateTime Date { get; set; }//日期，精确到天
        public TimeSpan Duration { get; set; }//时长，精确到分
        public WorkTimeOvertime Overtime { get; set; }
        public decimal Salary { get; set; }//所得工资
        public string Description { get; set; }//说明
    }
    public enum WorkTimeOvertime
    {
        未知 = 0,
        正常 = 1,
        加班 = 2,
    }
}
