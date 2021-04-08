using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    // 简单任务系统
    public class SimpleTask
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int AssignedMemberId { get; set; }//指定人员Id
        public string Description { get; set; }
        public SimpleTaskState State { get; set; }
    }
    public enum SimpleTaskState : byte
    {
        激活 = 1,
        完成 = 2
    }
}
