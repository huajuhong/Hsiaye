using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class MembershipInput
    {
        public string Name { get; set; }
        public Domain.Shared.Gender Gender { get; set; }
        public string Phone { get; set; }
        public string IDCard { get; set; }
        public MembershipState State { get; set; }
    }
    public class MembershipEditInput : MembershipInput
    {
        public int Id { get; set; }
    }
}
