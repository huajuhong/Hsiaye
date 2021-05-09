using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    //机构成员

    //添加系统成员->给该成员设置所属的组织机构
    //分配角色：会员管理系统
    //该系统成员登录后即可使用会员管理系统模块
    //主要功能：
    //  1.会员：列表/添加/编辑/删除
    //  2.消费项目（商品）：列表/添加/编辑/删除
    //todo  3.促销活动：列表/添加/编辑/删除
    //todo  4.会员充值：形成资金流水
    //todo  5.会员消费：选择消费项目进行消费，形成资金流水
    //todo  6.会员提现：形成资金流水
    public class Membership
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public string Name { get; set; }
        public Shared.Gender Gender { get; set; }
        public string Phone { get; set; }
        public string IDCard { get; set; }//身份证号
        public decimal Balance { get; set; }
        public MembershipState State { get; set; }
        public bool Deleted { get; set; }
    }
    public enum MembershipState
    {
        未知 = 0,
        正常 = 1,
        禁用 = 2
    }
}
