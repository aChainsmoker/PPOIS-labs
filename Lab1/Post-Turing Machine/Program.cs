namespace PostMachineNS
{
    class Program
    {
        public static void Main(string[] args)
        {
            Reader reader = new Reader();
            Post_Machine machine = new Post_Machine();
            CommandImplementer commandImplementer = new CommandImplementer(machine);
            UserInterface userInterface = new UserInterface(commandImplementer);
            Interpreter interpreter = new Interpreter();
            Writer writer = new Writer();
            string filePath;


            reader.AskForFilePath(userInterface);

            try
            {
                reader.ReadTheTape(filePath = Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("No File! You're in " + Directory.GetCurrentDirectory());
                return;
            }

            interpreter.InterpretTheData(machine, reader.tapeData, reader.pointerPlace, reader.behaviourData);

            userInterface.TakeTheInput();

            writer.WriteTheTape(filePath, interpreter.UnInterpretTheTape(machine.tape), interpreter.InterpreThePositionPointer(machine.currentPosition), interpreter.UnInterpretTheBehaviour(machine.behaviour));
        }
    }

}

