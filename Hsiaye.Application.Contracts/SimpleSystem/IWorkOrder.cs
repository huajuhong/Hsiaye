using Hsiaye.Domain;
using System.Collections.Generic;

namespace Hsiaye.Application.Contracts
{
    public interface IWorkOrder
    {
        void Create(WorkOrder input);
        void Update(int id, int submitMemberId, WorkOrderState state);
        List<WorkOrder> List(int submitMemberId, WorkOrderState? state);
    }
}
