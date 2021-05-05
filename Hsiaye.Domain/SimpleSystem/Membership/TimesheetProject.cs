using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //工时表项目
    public class TimesheetProject
    {
        public int Id { get; set; }
        public int CreateMemberId { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public string Name { get; set; }
    }
}
