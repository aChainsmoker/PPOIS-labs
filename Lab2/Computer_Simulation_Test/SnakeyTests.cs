using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Computer_Simulation.Tests
{
    [TestClass]
    public class SnakeyTests
    {
        private Snakey snakey;
        private string tempFilePath;

        [TestInitialize]
        public void TestInitialize()
        {
            snakey = new Snakey();
            tempFilePath = "Snakey.txt";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }

        [TestMethod]
        public void LaunchApp_Test()
        {
            File.WriteAllText(tempFilePath, "10");
            snakey.LaunchApp();
            var maxScore = GetPrivateField<int>(snakey, "maxScore");
            Assert.AreEqual(10, maxScore);
        }

        [TestMethod]
        public void CloseApp_Test()
        {
            SetPrivateField(snakey, "maxScore", 20);
            snakey.CloseApp();
            string fileContent = File.ReadAllText(tempFilePath).Trim();
            Assert.AreEqual("20", fileContent);
        }

        [TestMethod]
        public void SpawnFood_Test()
        {
            SetPrivateField(snakey, "snakesParts", new List<(int X, int Y)> { (7, 7) });
            InvokePrivateMethod(snakey, "SpawnFood");
            var foodPosition = GetPrivateField<(int X, int Y)>(snakey, "foodPosition");
            var snakeParts = GetPrivateField<List<(int X, int Y)>>(snakey, "snakesParts");
            Assert.IsFalse(snakeParts.Contains(foodPosition));
        }

        [TestMethod]
        public void MoveSnake_Test()
        {
            var initialSnake = new List<(int X, int Y)> { (7, 7) };
            SetPrivateField(snakey, "snakesParts", initialSnake);
            SetPrivateField(snakey, "foodPosition", (8, 7));
            SetPrivateField(snakey, "direction", ConsoleKey.RightArrow);
            InvokePrivateMethod(snakey, "MoveSnake");
            var snakeParts = GetPrivateField<List<(int X, int Y)>>(snakey, "snakesParts");
            Assert.AreEqual(2, snakeParts.Count);
        }

        [TestMethod]
        public void MoveSnakeCollision_Test()
        {
            var snake = new List<(int X, int Y)> { (16, 16), (7, 7), (6, 7), (6, 8), (7, 8), (7, 7) };
            SetPrivateField(snakey, "snakesParts", snake);
            SetPrivateField(snakey, "direction", ConsoleKey.RightArrow);
            InvokePrivateMethod(snakey, "MoveSnake");
            var isGameOver = GetPrivateField<bool>(snakey, "isGameOver");
            Assert.IsTrue(isGameOver);
        }

        private T GetPrivateField<T>(object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(obj);
        }

        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(obj, value);
        }

        private void InvokePrivateMethod(object obj, string methodName, params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(obj, parameters);
        }
    }
}
