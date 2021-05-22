using Hsiaye.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    public class TodoListInput : PageInput
    {
        public string Title { get; set; }
        public DateTime ExpireTime { get; set; }//到期时间
        public int Priority { get; set; }//优先级，数值越小优先级越高
        public int DistributeToMemberId { get; set; }//分配到
        public int CategoryId { get; set; }
        public string Tag { get; set; }//标签
        public TodoState State { get; set; }
    }
}
