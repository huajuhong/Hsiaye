using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //工时薪资
    public class WorkTimeSalary
    {
        public int Id { get; set; }
        public int CreateMemberId { get; set; }
        public DateTime CreateTime { get; set; }
        public int ProjectId { get; set; }//WorkTimeProject的Id
        public int MembershipId { get; set; }
        public string Name { get; set; }//薪资名称，例如：大工、小工、工程师等
        public decimal Amount { get; set; }//薪资数额
        public WorkTimeSalaryType Type { get; set; }
        public bool Deleted { get; set; }
    }
    public enum WorkTimeSalaryType
    {
        未知 = 0,
        年薪 = 1,
        月薪 = 2,
        日薪 = 3,
        时薪 = 4,
    }
}
