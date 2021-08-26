using Hsiaye.Application.Contracts;
using DapperExtensions;
using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using Hsiaye.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Predicate;

namespace Hsiaye.Application
{
    public class RoleService : IRoleService
    {
        private readonly IDatabase _database;

        public RoleService(IDatabase database)
        {
            _database = database;
        }

        public List<Role> GetList(string permission)
        {
            var permissions = _database.GetList<Permission>(Predicates.Field<Permission>(f => f.Name, Operator.Eq, permission));
            if (!permissions.Any())
            {
                return null;
            }
            var roles = _database.GetList<Role>(Predicates.Field<Role>(f => f.Id, Operator.Eq, permissions.Select(x => x.RoleId))).ToList();
            return roles;
        }
    }
}
