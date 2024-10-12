namespace PostMachineNS
{
    public class CommandImplementer
    {
        private Post_Machine machine;
        private Interpreter interpreter = new Interpreter();
        private Editor editor;

        private int tapeLength = 9;
        private bool logState;

        public static bool exitWasRequested;

        public CommandImplementer(Post_Machine machine)
        {
            this.machine = machine;
            editor = new Editor(machine);
        }

        public string ImplementCommand(string? command)
        {
            string[] commandParts = command.Split();
            try
            {
                switch (commandParts[0])
                {
                    case "log":
                        if (logState == false)
                            machine.lineExecuted += ShowTheData;
                        else
                            machine.lineExecuted -= ShowTheData;
                        logState = !logState;
                        break;
                    case "exe":
                        if (machine.GetMachineState() == false)
                        {
                            machine.ExecuteTheProgram(Convert.ToBoolean(commandParts[1]));
                            machine.SetMachineState(false);
                        }
                        break;
                    case "edit":
                        editor.EditRules(createNewRule(commandParts)[0]);
                        break;
                    case "delete":
                        editor.DeleteRule(Convert.ToInt32(commandParts[1]));
                        break;
                    case "add":
                        editor.AddRule(createNewRule(commandParts)[0]);
                        break;
                    case "length":
                        tapeLength = Convert.ToInt32(commandParts[1]);
                        break;
                    case "mark":
                        machine.MarkTheCell();
                        break;
                    case "erase":
                        machine.EraseTheMark();
                        break;
                    case "right":
                        machine.MoveToTheRight();
                        break;
                    case "left":
                        machine.MoveToTheLeft();
                        break;
                    case "switch":
                        machine.SwitchTheExecutionMode();
                        break;
                    case "help":
                        DisplayHelpDocument(commandParts[1]);
                        return commandParts[0];
                    case "next":
                        break;
                    case "exit":
                        exitWasRequested = true;
                        break;
                    default:
                        Console.WriteLine("Command does not exist!");
                        Thread.Sleep(1000);
                        break;

                }
            }
            catch
            {
                Console.WriteLine("Wrong command form! Try again");
                Thread.Sleep(1000);
            }
            machine.lineExecuted?.Invoke();

            return commandParts[0];
        }

        private void DisplayHelpDocument(string pathToFile)
        {
            Reader reader = new Reader();

            List<string> helpLines = reader.ReadTheHelpDocument(pathToFile);

            for (int i = 0; i < helpLines.Count; i++)
            {
                Console.WriteLine(helpLines[i]);
            }
        }

        private void ShowTheData()
        {
            if (Console.IsOutputRedirected == false)
                Console.Clear();

            int pointerPosrition = ShowThePointerPosition();

            ShowTheTapeState(pointerPosrition);
            ShowTheBehaviourState();
        }

        private int ShowThePointerPosition()
        {
            int pointerPosition;

            if (tapeLength % 2 != 0)
                pointerPosition = tapeLength - 1;
            else
                pointerPosition = tapeLength;

            for (int i = 0; i < pointerPosition; i++)
            {
                Console.Write(" ");
            }
            Console.Write("↓ \n");

            return pointerPosition;
        }

        private void ShowTheTapeState(int pointerPosition)
        {
            for (int i = (machine.currentPosition - pointerPosition / 2); i <= machine.currentPosition + pointerPosition / 2; i++)
            {
                if (i < 0 && i == (machine.currentPosition - pointerPosition / 2))
                    machine.SetCurrentPointerPosition(machine.currentPosition - i);

                i = machine.MakeTapeWiderIfNeeded(i);

                Console.Write(Convert.ToInt32(machine.tape[i]) + " ");
            }
            Console.Write('\n');
        }

        private void ShowTheBehaviourState()
        {
            for (int i = 0; i < machine.behaviour.Count; i++)
            {

                for (int j = 0; j < machine.behaviour[i].Length; j++)
                {
                    Console.Write(machine.behaviour[i][j] + " ");
                }

                Console.Write("\n");
            }
        }

        private List<string[]> createNewRule(string[] commandParts)
        {
            string newRule = "";
            List<string?> newRules = new List<string?>();
            for (int i = 1; i < commandParts.Length; i++)
                newRule += commandParts[i] + " ";
            newRules.Add(newRule);
            List<string[]> newInterpretedRule = interpreter.InterpretTheBehaviour(newRules);

            return newInterpretedRule;
        }

    }
}

