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
        List<RoleDto> GetRoles();
        MemberDto Get(long id);
        List<MemberDto> GetPaged(string keyword, bool isActive, int page, int limit);
        bool ChangePassword(ChangePasswordDto input);
        bool ResetPassword(ResetPasswordDto input);
    }
}
