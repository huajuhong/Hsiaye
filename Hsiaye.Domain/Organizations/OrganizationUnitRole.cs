using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    public class OrganizationUnitRole
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateMemberId { get; set; }
        public int RoleId { get; set; }
        public int OrganizationUnitId { get; set; }
        public int IsDeleted { get; set; }
    }
}
