using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class WorkTimeSalaryInput
    {
        public int ProjectId { get; set; }
        public int MembershipId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public WorkTimeSalaryType Type { get; set; }
    }
    public class WorkTimeSalaryEditInput : WorkTimeSalaryInput
    {
        public int Id { get; set; }
    }
}
