using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Member
{
    public class Member
    {
        public List<object> Permissions { get; set; }
        public List<object> Roles { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsPhoneNumberConfirmed { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordResetCode { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }
        public virtual string EmailConfirmationCode { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int TenantId { get; set; }
    }
}
