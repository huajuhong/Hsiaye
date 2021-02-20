using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsiaye.Application.Contracts.Authorization;
using Hsiaye.Dapper;
using Hsiaye.Domain.Authorization;
using Hsiaye.Domain.Members;

namespace Hsiaye.Application.Authorization
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly IDatabase _database;
        private readonly IAccessor _accessor;

        public PermissionChecker(IDatabase database, IAccessor accessor)
        {
            _database = database;
            _accessor = accessor;
        }

        public bool IsGranted(string permissionName)
        {
            return IsGranted(_accessor.MemberId, permissionName);
        }

        public bool IsGranted(long memberId, string permissionName)
        {
            var memberPermissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.MemberId, Operator.Eq, memberId));
            var member_Roles = _database.GetList<Member_Role>(Predicates.Field<Member_Role>(f => f.MemberId, Operator.Eq, memberId));

            List<IPredicate> predicates = new List<IPredicate>();
            foreach (var role in member_Roles)
            {
                predicates.Add(Predicates.Field<Permission>(f => f.RoleId, Operator.Eq, role.RoleId));
            }
            IPredicateGroup predicateGroup = Predicates.Group(GroupOperator.Or, predicates.ToArray());
            var rolePermissions = _database.GetList<Permission>(predicateGroup);

            bool result = memberPermissions != null && memberPermissions.Any(x => x.Name == permissionName && x.IsGranted) && rolePermissions != null && rolePermissions.Any(x => x.Name == permissionName && x.IsGranted);
            return result;
        }
    }
}
