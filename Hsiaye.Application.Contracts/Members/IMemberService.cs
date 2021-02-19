using Hsiaye.Application.Contracts.Members.Dto;
using Hsiaye.Application.Contracts.Roles.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hsiaye.Application.Contracts.Members
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
        MemberDto GetAll(string Keyword, bool IsActive, int SkipCount, int MaxResultCount);

        bool ChangePassword(ChangePasswordDto input);
        bool ResetPassword(ResetPasswordDto input);

    }
}
