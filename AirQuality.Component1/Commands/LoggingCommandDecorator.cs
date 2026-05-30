using AirQuality.Component1.Interfaces;
using AirQuality.Component1.Services;

namespace AirQuality.Component1.Commands
{
    public class LoggingCommandDecorator : CommandDecorator
    {
        private readonly LogService logService;
        private readonly string executeMessage;
        private readonly string undoMessage;

        public LoggingCommandDecorator(IUndoableCommand command, LogService logService, string executeMessage, string undoMessage)
            : base(command)
        {
            this.logService = logService;
            this.executeMessage = executeMessage;
            this.undoMessage = undoMessage;
        }

        public override void Execute()
        {
            logService.Log(executeMessage);
            wrappedCommand.Execute();
        }

        public override void Undo()
        {
            logService.Log(undoMessage);
            wrappedCommand.Undo();
        }
    }
}
