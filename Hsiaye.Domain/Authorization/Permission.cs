using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Authorization
{
    /// <summary>
    /// 角色和用户权限表
    /// TenantId、RoleId、MemberId不等于0时（写入一条记录时三者Id只能有一个不等于0），分别为他们自己的权限，例如租户权限、角色权限和成员权限
    /// </summary>
    public class Permission
    {
        public long CreatorMemberId { get; set; }
        public bool IsGranted { get; set; }
        public string Name { get; set; }
        public int TenantId { get; }
        public int RoleId { get; set; }
        public long MemberId { get; set; }
    }
}
