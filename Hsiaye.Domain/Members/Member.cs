using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Hsiaye.Domain
{
    public class Member
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int AccessFailedCount { get; set; }//登录失败次数
        [StringLength(64)]
        public string AuthenticationSource { get; set; }//身份验证源，通过电脑登录就是PC、移动登录就是APP
        [StringLength(1024)]
        public string Avatar { get; set; }//头像 
        [StringLength(64)]
        public string UserName { get; set; }//用户名
        [StringLength(64)]
        public string Name { get; set; }//姓名
        public Shared.Gender Gender { get; set; }
        [StringLength(64)]
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        [StringLength(64)]
        public string Password { get; set; }
        [StringLength(64)]
        public string PasswordResetCode { get; set; }
        [StringLength(64)]
        public string EmailAddress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [StringLength(64)]
        public string EmailConfirmationCode { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}