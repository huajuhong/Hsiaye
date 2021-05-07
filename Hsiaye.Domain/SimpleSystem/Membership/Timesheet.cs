using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //工时表

    //todo 1.工时项目：列表/添加/编辑/删除
    //todo 2.工时表：列表/添加/编辑/删除
    //工时添加：左侧显示组织下所有成员（名字+电话 含复选框）列表，右侧显示表单：选择项目/选择日期/选择时长/是否加班/说明
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
