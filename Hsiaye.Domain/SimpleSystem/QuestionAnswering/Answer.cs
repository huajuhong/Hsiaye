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
        public int OrganizationUnitId { get; set; }
        public DateTime CreateTime { get; set; }
        public int QuestionId { get; set; }
        public int AuthorId { get; set; }//MemberId
        public string AuthorName { get; set; }//MemberName
        public string Description { get; set; }//描述
    }
}
