﻿using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 角色和用户权限表
    /// RoleId、MemberId不等于0时（写入一条记录时三者Id只能有一个不等于0），分别为他们自己的权限，例如角色权限和成员权限
    /// </summary>
    public class Permission
    {
        public int Id { get; set; }
        public long CreatorMemberId { get; set; }
        public bool IsGranted { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int RoleId { get; set; }
        public long MemberId { get; set; }
    }
}
