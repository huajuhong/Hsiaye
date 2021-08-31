using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    /// <summary>
    /// 座位科目
    /// </summary>
    public class SeatSubject
    {
        public long Id { get; set; }
        public long OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        public bool Normal { get; set; }
    }
}
