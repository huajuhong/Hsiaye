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
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public long DeleterUserId { get; set; }
        public DateTime DeletionTime { get; set; }
        public string DisplayName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModificationTime { get; set; }
        public long LastModifierUserId { get; set; }
        public int ParentId { get; set; }
        public int TenantId { get; set; }
        public decimal Balance { get; set; }
    }
}
