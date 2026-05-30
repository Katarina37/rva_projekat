using AirQuality.Component1.Interfaces;

namespace AirQuality.Component1.Commands
{
    public abstract class CommandDecorator : IUndoableCommand
    {
        protected IUndoableCommand wrappedCommand;

        protected CommandDecorator(IUndoableCommand command)
        {
            wrappedCommand = command;
        }

        public virtual void Execute()
        {
            wrappedCommand.Execute();
        }

        public virtual void Undo()
        {
            wrappedCommand.Undo();
        }
    }
}
