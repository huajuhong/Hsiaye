using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class MemberListInput : PageInput
    {
        public string Keywords { get; set; }
        public MemberState State { get; set; }
    }
}
