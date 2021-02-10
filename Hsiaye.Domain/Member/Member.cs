using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Domain.Member
{
    public class Member
    {
        public List<string> Permissions { get; set; }
        public List<string> Roles { get; set; }
        public string UserName { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordResetCode { get; set; }
        public int AccessFailedCount { get; set; }
        public string EmailAddress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string EmailConfirmationCode { get; set; }
        public bool IsActive { get; set; }
        public int TenantId { get; set; }
    }
}
