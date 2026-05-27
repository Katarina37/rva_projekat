using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirQuality.Component1.Interfaces
{
    public interface IUndoableCommand
    {
        void Execute();
        void Undo();
    }
}
