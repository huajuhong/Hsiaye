
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public class RoleEditDto
    {
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; }

        //[Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsStatic { get; set; }
    }
}
