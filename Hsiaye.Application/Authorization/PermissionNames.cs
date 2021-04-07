using Hsiaye.Dapper;
using Hsiaye.Domain;
using System.Collections.Generic;

namespace Hsiaye.Application
{
    public static class PermissionNames
    {
        public const string 成员 = "成员";
        public const string 成员_新建 = "成员.新建";
        public const string 成员_列表 = "成员.列表";
        public const string 成员_详情 = "成员.详情";
        public const string 成员_编辑 = "成员.编辑";
        public const string 成员_重置密码 = "成员.重置密码";


        public const string 角色 = "角色";
        public const string 角色_新建 = "角色.新建";
        public const string 角色_列表 = "角色.列表";
        public const string 角色_详情 = "角色.详情";
        public const string 角色_编辑 = "角色.编辑";


        public static List<Permission> Permissions
        {
            get
            {
                var fieldInfos = typeof(PermissionNames).GetFields();
                List<Permission> permissions = new List<Permission>();
                foreach (var item in fieldInfos)
                {
                    var value = item.GetValue(item);
                    
                    permissions.Add(new Permission
                    {
                        CreatorMemberId = 0,
                        Name = value.ToString(),
                        MemberId = 0,
                        RoleId = 0,
                        TenantId = 0,
                        IsGranted = true,
                    });
                }
                return permissions;
            }
        }
    }
}
