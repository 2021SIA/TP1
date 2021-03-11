using System;
using System.Collections.Generic;
using System.Text;

namespace TP1.Models
{
    public interface State
    {
        IDictionary<object, State> PosibleActions();
        bool IsGoal { get; }

        bool IsDead();
    }
}
