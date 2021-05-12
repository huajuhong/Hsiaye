using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class WorkTimeInput
    {
        public int ProjectId { get; set; }
        public int MembershipId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsOvertime { get; set; }
        public string Description { get; set; }
    }
    public class WorkTimeEditInput : WorkTimeInput
    {
        public int Id { get; set; }
    }
}
