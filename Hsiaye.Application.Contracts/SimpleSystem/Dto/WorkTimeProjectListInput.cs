using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class WorkTimeProjectListInput : PageInput
    {
        public string Keywords { get; set; }
        public TimesheetProjectState State { get; set; }
    }
}
