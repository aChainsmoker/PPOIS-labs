namespace PostMachineNS
{
    public class Post_Machine
    {
        public List<bool> tape { get; private set; }
        public int currentPosition { get; private set; }
        public List<string[]> behaviour { get; private set; }

        public Action lineExecuted;

        private bool isMachineWorking;
        private bool stepByStepExecutionMode;
        private int lastExecutedLine;

        private UserInterface userInterface;

        public Post_Machine()
        {
            tape = new List<bool>();
            behaviour = new List<string[]>();
            userInterface = new UserInterface(new CommandImplementer(this));
        }

        public void SetCurrentPointerPosition(int Pos)
        {
            currentPosition = Pos;
        }

        public void SetTapeData(List<bool> tapeData)
        {
            tape = tapeData;
        }

        public void SetBehaviourData(List<string[]> behaviour)
        {
            this.behaviour = behaviour;
        }

        public void SetMachineState(bool state)
        {
            isMachineWorking = state;
        }

        public bool GetMachineState()
        {
            return isMachineWorking;
        }

        public void SwitchTheExecutionMode()
        {
            stepByStepExecutionMode = !stepByStepExecutionMode;
        }

        public void ExecuteTheProgram(bool executionMode)
        {
            stepByStepExecutionMode = executionMode;
            isMachineWorking = true;

            for (int i = 0; i < behaviour.Count; i++)
            {
                lastExecutedLine = i + 1;

                if (i != Convert.ToInt32(behaviour[i][0]) - 1)
                {
                    userInterface.TerminateCurrentProcess(lastExecutedLine, 1);
                    return;
                }

                if (stepByStepExecutionMode == true)
                    userInterface.TakeTheInputInStepByStepMode(lastExecutedLine);

                switch (behaviour[i][1])
                {
                    case "v":
                        tape[currentPosition] = true;
                        break;
                    case "x":
                        tape[currentPosition] = false;
                        break;
                    case "->":
                        currentPosition++;
                        currentPosition = MakeTapeWiderIfNeeded(currentPosition);
                        break;
                    case "<-":
                        currentPosition--;
                        currentPosition = MakeTapeWiderIfNeeded(currentPosition);
                        break;
                    case "!":
                        return;
                    case "?":
                        if (tape[currentPosition] == false)
                            i = Convert.ToInt32(behaviour[i][2]) - 2;
                        else
                            i = Convert.ToInt32(behaviour[i][3]) - 2;
                        continue;
                    default:
                        userInterface.TerminateCurrentProcess(lastExecutedLine, 2);
                        return;
                }
                i = Convert.ToInt32(behaviour[i][2]) - 2;

                lineExecuted?.Invoke();
            }
            userInterface.TerminateCurrentProcess(lastExecutedLine, 3);
            return;
        }

        public int MakeTapeWiderIfNeeded(int indexToCheck)
        {
            if (indexToCheck >= tape.Count)
            {
                while (tape.Count <= indexToCheck)
                    tape.Add(false);
                return indexToCheck;
            }
            else if (indexToCheck < 0)
            {
                while (indexToCheck < 0)
                {
                    tape.Insert(0, false);
                    indexToCheck++;
                }
                return indexToCheck;
            }
            else
            {
                return indexToCheck;
            }
        }

        public void MarkTheCell()
        {
            tape[currentPosition] = true;
        }
        public void EraseTheMark()
        {
            tape[currentPosition] = false;
        }
        public void MoveToTheRight()
        {
            currentPosition++;
            currentPosition = MakeTapeWiderIfNeeded(currentPosition);
        }
        public void MoveToTheLeft()
        {
            currentPosition--;
            currentPosition = MakeTapeWiderIfNeeded(currentPosition);
        }

        public void FixTheSequence()
        {
            for (int i = 0; i < behaviour.Count - 1; i++)
            {
                for (int j = i + 1; j < behaviour.Count; j++)
                {
                    if (Convert.ToInt32(behaviour[i][0]) > Convert.ToInt32(behaviour[j][0]))
                    {
                        string[] buff;
                        buff = behaviour[i];
                        behaviour[i] = behaviour[j];
                        behaviour[j] = buff;
                    }
                }
            }
        }
    }
}