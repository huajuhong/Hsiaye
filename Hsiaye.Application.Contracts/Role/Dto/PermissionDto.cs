﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Role.Dto
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
