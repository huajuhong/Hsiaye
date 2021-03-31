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
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; }
        public int AccessFailedCount { get; set; }//登录失败次数
        [MaxLength(64)]
        public string AuthenticationSource { get; set; }//身份验证源，通过电脑登录就是PC、移动登录就是APP
        [MaxLength(1024)]
        public string Avatar { get; set; }//头像 
        [MaxLength(64)]
        public string UserName { get; set; }//用户名
        [MaxLength(64)]
        public string Name { get; set; }//姓名
        [MaxLength(64)]
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        [MaxLength(64)]
        public string Password { get; set; }
        [MaxLength(64)]
        public string PasswordResetCode { get; set; }
        [MaxLength(64)]
        public string EmailAddress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [MaxLength(64)]
        public string EmailConfirmationCode { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastLoginTime { get; set; }
    }
}