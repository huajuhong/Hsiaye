using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //机构成员
    public class Membership
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public string Number { get; set; }//编号，组织机构中成员数量+1，用于快捷查询。例：通过该编号查询工时、流水等。
        public string Name { get; set; }
        public Shared.Sex Sex { get; set; }
        public string Phone { get; set; }
        public string IDCard { get; set; }
        public decimal Balance { get; set; }
    }
}
