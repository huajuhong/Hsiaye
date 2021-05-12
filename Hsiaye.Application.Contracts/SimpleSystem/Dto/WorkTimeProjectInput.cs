using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class WorkTimeProjectInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimesheetProjectState State { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class WorkTimeProjectEditInput : WorkTimeProjectInput
    {
        public int Id { get; set; }
    }
}
