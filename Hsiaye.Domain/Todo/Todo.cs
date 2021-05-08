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
        public string Title { get; set; }//标题
        public string Tag { get; set; }//标签
        public DateTime 到期时间 { get; set; }
        public string 分配到 { get; set; }
        public string 分配自 { get; set; }
        public int ParentId { get; set; }
        public TimeSpan 估算时间 { get; set; }
        public DateTime StartTime { get; set; }//开始时间
        public string Category { get; set; }
        public DateTime LastModifyTime { get; set; }//上次修改时间
        public int LastModifyMemberId { get; set; }//上次修改者
        public TimeSpan TimeRemaining { get; set; }//剩余时间（作为列显示），值为：到期时间-开始时间
        public int DependTodoId { get; set; }//这个任务必须等该 依赖性 任务完成才可进行
        public TimeSpan SpendTime { get; set; }//所花费时间
        public bool Remind { get; set; }//提醒
        public string Icon { get; set; }//图标
        public byte CompletePercent { get; set; }//完成百分比
        public bool Completed { get; set; }//完成
        public string FileLinks { get; set; }//文件链接
        public int Priority { get; set; }//优先级 

        public TodoRepeat Repeat { get; set; }//重复：每天/每周（某星期）/每月（某日）/每年（某月某日） 的某个时刻
        public int RepeatDay { get; set; }//重复日
        public DayOfWeek RepeatWeek { get; set; }//重复周
        public int RepeatMonth { get; set; }//重复月
        public TimeSpan ReminderTime { get; set; }//提醒时刻

        public DateTime NoteTime { get; set; }//注释时间 
        public string NoteContent { get; set; }//注释内容
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
