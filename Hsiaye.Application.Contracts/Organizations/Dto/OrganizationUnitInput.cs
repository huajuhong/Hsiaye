using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class OrganizationUnitInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
    }
    public class OrganizationUnitEditInput : OrganizationUnitInput
    {
        public int Id { get; set; }
    }
}
