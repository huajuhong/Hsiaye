﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public class RoleDto
    {
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; }

        //[Required]
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<string> GrantedPermissions { get; set; }
    }
}
