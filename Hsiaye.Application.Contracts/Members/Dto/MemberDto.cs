﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Members.Dto
{
    public class MemberDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }


        public DateTime? LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public string[] RoleNames { get; set; }
    }
}