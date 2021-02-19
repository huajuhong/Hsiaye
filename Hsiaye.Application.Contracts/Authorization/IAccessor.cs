using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsiaye.Domain;
using Hsiaye.Domain.Members;

namespace Hsiaye.Application.Contracts.Authorization
{
    public interface IAccessor
    {
        long MemberId { get; }
        Member Member { get; }
    }
}
