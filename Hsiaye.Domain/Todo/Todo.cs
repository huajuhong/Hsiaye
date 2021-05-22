using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //todo:重建表结构
    public class Todo
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public int CreateMemberId { get; set; }
        public string Title { get; set; }//标题
        public int CategoryId { get; set; }
        public string Tag { get; set; }//标签
        public DateTime ExpireTime { get; set; }//到期时间
        public int DistributeToMemberId { get; set; }//分配到
        public int DistributeFormMemberId { get; set; }//分配自
        public int ParentId { get; set; }
        public TodoState State { get; set; }
        public TimeSpan EstimatedTime { get; set; }//估算时间
        public DateTime StartTime { get; set; }//开始时间
        public int Priority { get; set; }//优先级，数值越小优先级越高
        public TodoRepeatType RepeatType { get; set; }//重复：每天/每周（某几星期）/每月（某几日）/每年（某月某日） 的某个时刻
        public bool Remind { get; set; }//提醒
        public TimeSpan ReminderTime { get; set; }//提醒时刻

        public DateTime LastModifyTime { get; set; }//上次修改时间
        public int LastModifyMemberId { get; set; }//上次修改者
        public TimeSpan TimeRemaining { get; set; }//剩余时间（作为计算列显示），值为：到期时间-开始时间
        public int DependTodoId { get; set; }//这个任务必须等该 依赖性 任务完成才可进行
        public TimeSpan SpendTime { get; set; }//所花费时间
        public string Icon { get; set; }//图标
        public byte CompletePercent { get; set; }//完成百分比
        public bool Completed { get; set; }//完成
        public string FileLinks { get; set; }//文件链接

        public DateTime NoteTime { get; set; }//注释时间 
        public string NoteContent { get; set; }//注释内容
        public int SubtaskCompletePercent { get; set; }//子任务完成百分比
    }

    public class TodoRepeat
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int TodoId { get; set; }
        public int RepeatDay { get; set; }//重复日
        public DayOfWeek RepeatWeek { get; set; }//重复周
        public int RepeatMonth { get; set; }//重复月
    }
    public enum TodoRepeatType
    {
        未知 = 0,
        不重复 = 1,
        每天 = 2,
        每周 = 3,
        每月 = 4,
        每年 = 5,
    }
    public enum TodoState
    {
        未知 = 0,
        已完成 = 1,
        待完成 = 2
    }
}
