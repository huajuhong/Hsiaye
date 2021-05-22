using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    public class OrganizationUnit
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateMemberId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public int ModifyMemberId { get; set; }
        public DateTime ModifyTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
