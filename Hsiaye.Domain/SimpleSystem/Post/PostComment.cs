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
        /// <summary>
        /// 评论者Id
        /// </summary>
        public int MemberId { get; set; }
        public int PostId { get; set; }
        /// <summary>
        /// 父级评论Id
        /// </summary>
        public int ParentCommentId { get; set; }
        public string Content { get; set; }
    }
}
