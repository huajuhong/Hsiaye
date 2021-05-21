using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class WorkTimeSalaryListInput : PageInput
    {
        public string Keywords { get; set; }
        public int ProjectId { get; set; }
        public int MembershipId { get; set; }
    }
}
