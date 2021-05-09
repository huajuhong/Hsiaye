using Hsiaye.Application.Contracts;
using Hsiaye.Dapper;
using Hsiaye.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Hsiaye.Application
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

        public bool IsGranted(int memberId, string permissionName)
        {
            var memberPermissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.MemberId, Operator.Eq, memberId));
            var memberRoles = _database.GetList<MemberRole>(Predicates.Field<MemberRole>(f => f.MemberId, Operator.Eq, memberId));

            List<IPredicate> predicates = new List<IPredicate>();
            foreach (var role in memberRoles)
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
