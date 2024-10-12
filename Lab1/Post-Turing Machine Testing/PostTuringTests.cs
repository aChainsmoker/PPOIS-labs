using Microsoft.VisualStudio.TestPlatform.TestHost;
using PostMachineNS;
using System.Reflection.PortableExecutable;

namespace Post_Turing_Machine_Testing
{
    [TestClass]
    public  class PostTuringTests
    {
        Post_Machine machine;
        Editor editor;
        CommandImplementer commandImplementer;
        Interpreter interpreter;


        [TestInitialize]
        public void PostTuringInitialize()
        {

            machine = new Post_Machine();

            Reader reader = new Reader();

            reader.ReadTheTape("TestState.txt");

            interpreter = new Interpreter();

            
            interpreter.InterpretTheData(machine, reader.tapeData, reader.pointerPlace, reader.behaviourData);


            commandImplementer = new CommandImplementer(machine);

            UserInterface userInterface = new UserInterface(commandImplementer);

            editor = new Editor(machine);
        }

        [TestCleanup]
        public void PostTuringClean()
        {
            machine.SetCurrentPointerPosition(0);
            machine.tape.Clear();
            machine.behaviour.Clear();
        }

        [TestMethod]
        public void EditorTest()
        {
            string[] newRule = new string[] { "5", "v", "6" };
            editor.EditRules(newRule);
            CollectionAssert.Contains(machine.behaviour, newRule);

            newRule = new string[] { "10", "v", "11" };
            editor.EditRules(newRule);
            CollectionAssert.DoesNotContain(machine.behaviour, newRule);


            newRule = new string[] { "8", "x", "9" };
            editor.AddRule(newRule);
            CollectionAssert.Contains(machine.behaviour, newRule);

            newRule = new string[] { "8", "v", "9" };
            editor.AddRule(newRule);
            CollectionAssert.DoesNotContain(machine.behaviour, newRule);

            editor.DeleteRule(6);
            CollectionAssert.DoesNotContain(machine.behaviour, newRule);
        }


        [TestMethod]
        public void StateLoggingTest()
        {
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            commandImplementer.ImplementCommand("log");

            string expectedOutput = "0 0 0 0 0 1 0 0 0 ";

            StringAssert.Contains(stringWriter.ToString(), expectedOutput);
        }

        [TestMethod]
        public void CreateNewRuleTest()
        {
            commandImplementer.ImplementCommand("add 8 x 9");

            string[] expectedString = new string[] { "8", "x", "9", "" };

            CollectionAssert.AreEqual(expectedString, machine.behaviour[7], "Does not contain!");
        }


        [TestMethod]
        public void ExecutionTest()
        {
            commandImplementer.ImplementCommand("exe false");

            Assert.AreEqual(true, machine.tape[7]);
        }

        [TestMethod]
        public void writerTest()
        {
            Writer writer = new Writer();
            Reader reader = new Reader();
            List<bool> tapeCopy = machine.tape;


            writer.WriteTheTape("TestState.txt", interpreter.UnInterpretTheTape(machine.tape), machine.currentPosition, interpreter.UnInterpretTheBehaviour(machine.behaviour));

            reader.ReadTheTape("TestState.txt");

            Assert.AreEqual(tapeCopy, machine.tape);
        }

        [TestMethod]
        public void HelpDocumentDisplayingTest()
        {
            StringWriter stringWriter = new StringWriter();
            Reader reader = new Reader();
            Console.SetOut(stringWriter);

            Assert.AreEqual("help", commandImplementer.ImplementCommand("help ..\\..\\..\\help.txt"));

            string expectedOutput = "exit - выход";

            StringAssert.Contains(stringWriter.ToString(), expectedOutput);
            CollectionAssert.Contains(reader.ReadTheHelpDocument("..\\..\\..\\help.txt"), expectedOutput);
        }

        [TestMethod]
        public void ErrorTrowingTest()
        {
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            editor.EditRules(new string[] {"1", "v", "9"});

            Assert.AreEqual("exe", commandImplementer.ImplementCommand("exe false"));

            string expectedOutput = "Translation to the non-existing command from the 1 line. Stopping execution...";

            StringAssert.Contains(stringWriter.ToString(), expectedOutput);
        }


        [TestMethod]
        public void CommnandImplementationTest()
        {
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            Assert.AreEqual("exe", commandImplementer.ImplementCommand("exe false"));
            Assert.AreEqual("mark", commandImplementer.ImplementCommand("mark"));
            Assert.AreEqual("erase", commandImplementer.ImplementCommand("erase"));
            Assert.AreEqual("left", commandImplementer.ImplementCommand("left"));
            Assert.AreEqual("right", commandImplementer.ImplementCommand("right"));
            Assert.AreEqual("length", commandImplementer.ImplementCommand("length 9"));
            Assert.AreEqual("delete", commandImplementer.ImplementCommand("delete 7"));
            Assert.AreEqual("add", commandImplementer.ImplementCommand("add 7 !"));
            Assert.AreEqual("edit", commandImplementer.ImplementCommand("edit 1 x 2"));
            Assert.AreEqual("next", commandImplementer.ImplementCommand("next"));
            Assert.AreEqual("exit", commandImplementer.ImplementCommand("exit"));
            Assert.AreEqual("lol", commandImplementer.ImplementCommand("lol"));

            string expectedOutput = "Command does not exist!";
            StringAssert.Contains(stringWriter.ToString(), expectedOutput);
        }

        [TestMethod]
        public void TapeExpansionTest()
        {
            
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            commandImplementer.ImplementCommand("length 12");
            commandImplementer.ImplementCommand("log");

            string expectedOutput = "0 0 0 0 0 0 0 1 0 0 0 0 0";

            StringAssert.Contains(stringWriter.ToString(), expectedOutput);
        }

        [TestMethod]
        public void AskingForFileTest()
        {
            Reader reader = new Reader();
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);


            reader.AskForFilePath(new UserInterface(new CommandImplementer(new Post_Machine())));

            string expectedOutput = "Enter The File Path";

            StringAssert.Contains(stringWriter.ToString(), expectedOutput);
        }
        
    }
}



