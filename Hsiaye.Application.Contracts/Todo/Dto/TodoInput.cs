using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class TodoInput
    {
        public string Title { get; set; }//标题
        public int CategoryId { get; set; }
        public string Tag { get; set; }//标签
        public DateTime ExpireTime { get; set; }//到期时间
        public int ParentId { get; set; }
        public int Priority { get; set; }//优先级，数值越小优先级越高
        public TodoRepeatType RepeatType { get; set; }//重复：每天/每周（某几星期）/每月（某几日）/每年（某月某日） 的某个时刻
        public bool Remind { get; set; }//提醒
        public TimeSpan ReminderTime { get; set; }//提醒时刻
    }
    public class TodoEditInput : TodoInput
    {
        public int Id { get; set; }
    }
}
