using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts
{
    public interface IMemberService
    {
        MemberDto Create(CreateMemberDto input);
        MemberDto Update(MemberDto input);
        //[AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        void Delete(long id);
        //[AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        void Activate(long id);
        //[AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        void DeActivate(long id);
        MemberDto Get(long id);
        bool ChangePassword(ChangePasswordDto input);
        bool ResetPassword(ResetPasswordDto input);

    }
}
