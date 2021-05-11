using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //机构成员工资
    public class MembershipWage
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public MembershipWageType Type { get; set; }
        public bool Deleted { get; set; }
    }
    public enum MembershipWageType
    {
        未知 = 0,
        年薪 = 1,
        月薪 = 2,
        日薪 = 3,
        时薪 = 4,
    }
}
