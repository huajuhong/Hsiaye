using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Roles.Dto
{
    public class RoleListDto
    {
        public DateTime CreationTime { get; set; }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }
    }
}
