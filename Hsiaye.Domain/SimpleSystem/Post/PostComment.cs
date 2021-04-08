using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    public class PostComment
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}
