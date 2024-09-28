namespace PostMachineNS
{
    class Program
    {
        public static void Main(string[] args)
        {

            Reader reader = new Reader();

            reader.ReadTheTape(Console.ReadLine());

            Interpreter interpreter = new Interpreter();

            Post_Machine machine = new Post_Machine();

            interpreter.InterpretTheData(machine, reader.tapeData, reader.pointerPlace, reader.behaviourData);

            CommandImplementer commandImplementer = new CommandImplementer(machine);

            UserInterface userInterface = new UserInterface(commandImplementer);

            userInterface.TakeTheInput();

            Writer writer = new Writer();

            writer.WriteTheTape("file.txt", interpreter.UnInterpretTheTape(machine.tape), interpreter.InterpreThePositionPointer(machine.currentPosition), interpreter.UnInterpretTheBehaviour(machine.behaviour));

        }
    }
}

