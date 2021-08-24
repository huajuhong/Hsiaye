using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 座位种类
    /// </summary>
    public class SeatCategory
    {
        public int Id { get; set; }
        public int OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
