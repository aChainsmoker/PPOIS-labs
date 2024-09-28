namespace PostMachineNS
{
    public class Editor
    {
        Post_Machine machine;

        public Editor(Post_Machine machine)
        {
            this.machine = machine;
        }

        public void EditRules(string[] rule)
        {
            List<string[]> tapeCopy;
            tapeCopy = machine.behaviour;
            try
            {
                tapeCopy[Convert.ToInt32(rule[0]) - 1] = rule;
            }
            catch
            {
                return;
            }
            machine.FixTheSequence();
        }

        public void DeleteRule(int stringIndex)
        {
            List<string[]> tapeCopy;
            tapeCopy = machine.behaviour;
            for (int i = 0; i < tapeCopy.Count; i++)
            {
                if (Convert.ToInt32(tapeCopy[i][0]) == stringIndex)
                {
                    tapeCopy.RemoveAt(i);
                }
            }
        }

        public void AddRule(string[] rule)
        {
            List<string[]> tapeCopy;
            tapeCopy = machine.behaviour;

            for (int i = 0; i < tapeCopy.Count; i++)
            {
                if (tapeCopy[i][0] == rule[0])
                {
                    Console.WriteLine("The rule does already exist");
                    return;
                }
            }
            tapeCopy.Add(rule);
            machine.FixTheSequence();
        }
    }
}