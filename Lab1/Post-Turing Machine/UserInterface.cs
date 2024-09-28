namespace PostMachineNS
{
    public class UserInterface
    {
        private CommandImplementer commandImplementer;

        public UserInterface(CommandImplementer commandImplementer)
        {
            this.commandImplementer = commandImplementer;
        }

        public void TakeTheInput()
        {
            while (true)
            {

                if (CommandImplementer.exitWasRequested == true)
                    break;

                commandImplementer.ImplementCommand(Console.ReadLine());
            }
        }

        public void TakeTheInputInStepByStepMode(int lineToExecute)
        {
            string command = "";

            Console.WriteLine("This is step by step execution mode. 'next' for the next step and 'switch' for the switching execution mode");

            while (true)
            {

                if (CommandImplementer.exitWasRequested == true || command == "next" || command == "switch")
                    break;

                command = commandImplementer.ImplementCommand(Console.ReadLine());
            }
        }

        public string TerminateCurrentProcess(int lastExecutedLine, int errorIndex)
        {
            string errorLog = "";
            switch (errorIndex)
            {
                case 1:
                    errorLog = "Translation to the non-existing command from the {0} line. Stopping execution...";
                    break;
                case 2:
                    errorLog = "Wrong command form on the {0} line. Stopping execution...";
                    break;
                case 3:
                    errorLog = "Translation to the non-existing command from the {0} line. Stopping execution...";
                    break;
            }
            Console.WriteLine(errorLog, lastExecutedLine);
            Thread.Sleep(2000);

            return errorLog;
        }
    }
}