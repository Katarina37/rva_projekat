using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirQuality.Component1.Interfaces;

namespace AirQuality.Component1.Services
{
    public class UndoRedoService
    {
        private readonly Stack<IUndoableCommand> undoStack = new Stack<IUndoableCommand>();
        private readonly Stack<IUndoableCommand> redoStack = new Stack<IUndoableCommand>();

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public void ExecuteCommand(IUndoableCommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear(); 
        }

        public void Undo()
        {
            if (!CanUndo) return;
            var command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }

        public void Redo()
        {
            if (!CanRedo) return;
            var command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }
    }
}
