namespace PostMachineNS
{
    public class Interpreter
    {
        public void InterpretTheData(Post_Machine machine, string tape, int currentPosPointer, List<string?> behaviour)
        {
            machine.SetCurrentPointerPosition(InterpreThePositionPointer(currentPosPointer));

            machine.SetTapeData(InterpretTheTape(tape));

            machine.SetBehaviourData(InterpretTheBehaviour(behaviour));
        }

        public int InterpreThePositionPointer(int currentPosPointer)
        {
            return currentPosPointer;
        }

        public List<bool> InterpretTheTape(string tape)
        {
            List<bool> tapeData = new List<bool>();

            for (int i = 0; i < tape.Length; i++)
            {
                if (tape[i] == '1')
                    tapeData.Add(true);
                else
                    tapeData.Add(false);
            }
            return tapeData;
        }


        public List<string[]> InterpretTheBehaviour(List<string?> behaviour)
        {
            List<string[]> behaviourData = new List<string[]>();
            string[] behaviourCommandsPerLine;

            for (int i = 0; i < behaviour.Count; i++)
            {
                behaviourCommandsPerLine = behaviour[i].Split();

                behaviourData.Add(behaviourCommandsPerLine);
            }
            return behaviourData;
        }

        public string UnInterpretTheTape(List<bool> tape)
        {
            string stringTape = "";

            for (int i = 0; i < tape.Count; i++)
                stringTape += Convert.ToInt32(tape[i]).ToString();

            return stringTape;
        }

        public string[] UnInterpretTheBehaviour(List<string[]> behaviour)
        {
            string[] stringTape = new string[behaviour.Count];

            for (int i = 0; i < behaviour.Count; i++)
                for (int j = 0; j < behaviour[i].Length; j++)
                {
                    stringTape[i] += behaviour[i][j];
                    stringTape[i] += " ";
                }
            return stringTape;
        }
    }
}