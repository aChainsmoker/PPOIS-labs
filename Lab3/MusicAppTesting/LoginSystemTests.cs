using MusicAppNS;

namespace MusicAppTesting
{
    [TestClass]
    public class LoginSystemTests
    {
        private LoginSystem loginSystem;
        private StateReader stateReader;
        private StateWriter stateWriter;

        [TestInitialize]
        public void TestInitialize()
        {
            stateReader = new StateReader();
            stateWriter = new StateWriter();
            loginSystem = new LoginSystem();
            stateWriter.WriteState(new List<string>(), "Profiles.txt");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            stateWriter.WriteState(new List<string>(), "Profiles.txt");
        }

        [TestMethod]
        public void LoginSystemInputTest()
        {
            string login = "validUser";
            string password = "password123";
            string rights = "artist";
            loginSystem.RegisterInSystem(login, password, rights);
            Console.SetIn(new StringReader("asd\nlogin validUser password123\nexit"));
            loginSystem.EnterTheLoginData();
        }

        [TestMethod]
        public void HandleLoginInputFailTest()
        {
            string command = "login invalidUser wrongPassword";
            bool result = loginSystem.HandleInput(command);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HandleLoginInputSuccessTest()
        {
            string command = "register newUser password123 artist";
            bool result = loginSystem.HandleInput(command);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HandleRegisterInputFailTest()
        {
            string login = "existingUser";
            string password = "password123";
            string command = $"register {login} {password} listener";
            loginSystem.RegisterInSystem(login, password, "listener");

            bool result = loginSystem.HandleInput(command);
            Assert.IsFalse(result);
        }
    }
}
