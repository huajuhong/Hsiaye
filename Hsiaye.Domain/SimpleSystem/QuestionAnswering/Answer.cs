using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    // 答案
    public class Answer
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public int MemberId { get; set; }
        public int QuestionId { get; set; }
        public string Description { get; set; }//描述
        public int LikeCount { get; set; }//点赞数（有多少人认可这个答案）
        public bool Accepted { get; set; }
        public bool Deleted { get; set; }
    }
}
