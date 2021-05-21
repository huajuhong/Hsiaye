using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class WorkTimeListInput:PageInput
    {
        public int ProjectId { get; set; }
        public int MembershipId { get; set; }
        public WorkTimeOvertime Overtime { get; set; }
    }
}
