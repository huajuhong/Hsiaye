using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsiaye.Application.Contracts.Authorization;
using Hsiaye.Dapper;
using Microsoft.AspNetCore.Http;

namespace Hsiaye.Application.Authorization
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionChecker(IDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _database = database;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsGranted(string permissionName)
        {
            throw new NotImplementedException();
        }

        public bool IsGranted(long memberId, string permissionName)
        {
            throw new NotImplementedException();
        }
    }
}
