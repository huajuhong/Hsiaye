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
    /// 座位
    /// </summary>
    public class Seat
    {
        public long Id { get; set; }
        public long OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public long SeatCategoryId { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        public bool Normal { get; set; }

        public SeatCategory SeatCategory { get; set; }
    }
    public class SeatMap : ClassMapper<Seat>
    {
        public SeatMap()
        {
            Map(t => t.SeatCategory).Ignore();
            AutoMap();
            //ReferenceMap(t => t.SeatCategory).Reference<SeatCategory>((seatCategory, seat) => seatCategory.Id == seat.SeatCategoryId);
        }
    }
}
