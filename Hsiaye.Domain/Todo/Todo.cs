using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    public class Todo
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateMemberId { get; set; }
        public string 标题 { get; set; }
        public string 标签 { get; set; }
        public DateTime 到期时间 { get; set; }
        public string 分配到 { get; set; }
        public string 分配自 { get; set; }
        public int ParentId { get; set; }
        public TimeSpan 估算时间 { get; set; }
        public DateTime 开始时间 { get; set; }
        public string Category { get; set; }
        public DateTime 上次修改时间 { get; set; }
        public string 上次修改者 { get; set; }
        public TimeSpan 剩余时间 { get; set; }
        public int 依赖TodoId { get; set; }//这个任务必须等该 依赖性 任务完成才可进行
        public TimeSpan 所花费时间 { get; set; }
        public bool 提醒 { get; set; }
        public string 图标 { get; set; }
        public byte 完成百分比 { get; set; }
        public bool 完成 { get; set; }
        public string 文件链接 { get; set; }
        public int 优先级 { get; set; }

        public TodoRepeat Repeat { get; set; }//重复：每天/每周（某星期）/每月（某日）/每年（某月某日） 的某个时刻
        public int RepeatDay { get; set; }//重复日
        public DayOfWeek RepeatWeek { get; set; }//重复周
        public int RepeatMonth { get; set; }//重复月
        public TimeSpan ReminderTime { get; set; }//提醒时刻

        public DateTime 注释时间 { get; set; }
        public string 注释内容 { get; set; }
        public int 状态 { get; set; }
        public int 子任务完成度 { get; set; }
    }
    public enum TodoRepeat
    {
        不重复 = 0,
        每天 = 1,
        每周 = 2,
        每月 = 3,
        每年 = 4,
    }
}
