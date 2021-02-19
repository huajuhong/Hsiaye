using Hsiaye.Application.Contracts.Authorization;
using Hsiaye.Dapper;
using Hsiaye.Domain.Members;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Hsiaye.Application.Authorization
{
    public class Accessor : IAccessor
    {
        private readonly IDatabase _database;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Accessor(IDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _database = database;
            _httpContextAccessor = httpContextAccessor;
        }

        public long MemberId => _database.GetList<MemberLogin>(Predicates.Field<MemberLogin>(f => f.ProviderKey, Operator.Eq, _httpContextAccessor.GetToken())).FirstOrDefault().MemberId;
        public Member Member => _database.Get<Member>(Id);
    }
}
