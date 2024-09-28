namespace PostMachineNS
{
    public class Writer
    {
        private StreamWriter streamWriter;

        public void WriteTheTape(string path, string tape, int pointerPosition, string[] behaviour)
        {
            streamWriter = new StreamWriter(path, false);

            for (int i = 0; i < pointerPosition; i++)
            {
                streamWriter.Write(" ");
            }
            streamWriter.WriteLine('↓');
            streamWriter.WriteLine(tape);
            for (int i = 0; i < behaviour.Length; i++)
            {
                streamWriter.WriteLine(behaviour[i]);
            }

            streamWriter.Close();
        }
    }
}