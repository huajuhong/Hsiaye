using Hsiaye.Domain;
using System.Collections.Generic;

namespace Hsiaye.Application.Contracts
{
    public interface ISimpleTask
    {
        void Create(int assignedMemberId, string description);
        void Update(int id, int assignedMemberId, SimpleTaskState state);
        List<SimpleTask> List(int assignedMemberId, SimpleTaskState? state);
    }
}
