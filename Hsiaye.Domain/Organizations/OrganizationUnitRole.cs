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
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public int TenantId { get; set; }
        public int RoleId { get; set; }
        public int OrganizationUnitId { get; set; }
        public int IsDeleted { get; set; }
    }
}
