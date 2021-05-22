using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Domain
{
    // 简单问答
    // 参照实现：http://qasample.aspnetboilerplate.com
    public class Question
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrganizationUnitId { get; set; }
        public int CategoryId { get; set; }
        public int MemberId { get; set; }
        public string Title { get; set; }//标题
        public string Description { get; set; }//描述
        public int AnswerId { get; set; }//被采用的答案Id
        public int VoteCount { get; set; }//投票数（有多少人遇到这个问题）
        public int AnswerCount { get; set; }//答案数
        public int ViewCount { get; set; }//浏览量
        public bool Deleted { get; set; }
    }
}
