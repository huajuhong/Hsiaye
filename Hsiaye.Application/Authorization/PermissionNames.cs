using Hsiaye.Dapper;
using Hsiaye.Domain;
using System.Collections.Generic;

namespace Hsiaye.Application
{
    public static class PermissionNames
    {
        public const string AdminUserName = "hsiaye";

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


        public const string 会员 = "会员";
        public const string 会员_新建 = "会员.新建";
        public const string 会员_列表 = "会员.列表";
        public const string 会员_详情 = "会员.详情";
        public const string 会员_编辑 = "会员.编辑";
        public const string 会员_充值 = "会员.充值";
        public const string 会员_消费 = "会员.消费";
        public const string 会员_提现 = "会员.提现";


        public const string 商品 = "商品";
        public const string 商品_新建 = "商品.新建";
        public const string 商品_列表 = "商品.列表";
        public const string 商品_详情 = "商品.详情";
        public const string 商品_编辑 = "商品.编辑";

        public const string 促销活动 = "促销活动";
        public const string 促销活动_新建 = "促销活动.新建";
        public const string 促销活动_列表 = "促销活动.列表";
        public const string 促销活动_详情 = "促销活动.详情";
        public const string 促销活动_编辑 = "促销活动.编辑";

        public const string 工时 = "工时";


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
                        IsGranted = true,
                    });
                }
                return permissions;
            }
        }
    }
}
