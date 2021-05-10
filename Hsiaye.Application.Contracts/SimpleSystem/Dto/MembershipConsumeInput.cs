using Hsiaye.Domain;
using Hsiaye.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsiaye.Application.Contracts
{
    //消费
    public class MembershipConsumeInput
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public PayType PayType { get; set; }
        public bool PreviewAmount{ get; set; }//预览消费金额
    }
}
