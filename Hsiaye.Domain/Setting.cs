using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    public class Setting
    {
        public long Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string ControllerName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
