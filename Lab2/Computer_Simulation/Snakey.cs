using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Simulation
{
    public class Snakey : Application, ILoadable
    {
        private string appName = "Snakey";

        private const int FieldWidth = 15;
        private const int FieldHeight = 15;

        private List<(int X, int Y)> snakesParts;
        private (int X, int Y) foodPosition;
        private int score = 0;
        private int maxScore;
        private ConsoleKey direction;
        private bool isGameOver = false;

        public override string AppName { get => appName; set => appName = value; }

        public override void CloseApp()
        {
            WriteData();
            appStateUpdated -= UpdateAppRepresentation;
        }

        public override void LaunchApp()
        {
            ReadData();
            appStateUpdated += UpdateAppRepresentation;
        }

        public void ReadData()
        {
            if(stateReader.ReadData("Snakey.txt").Count != 0)
                maxScore = Convert.ToInt32(stateReader.ReadData("Snakey.txt")[0]);
        }
        public void WriteData()
        {
            List<string> stringData = new List<string>();
            stringData.Add(maxScore.ToString());
            stateWriter.WriteState(stringData, "Snakey.txt");
        }

        public override string HandleInput(string command)
        {
            switch (command.Trim())
            {
                case "play":
                    isGameOver = false;
                    Start();
                    break;
            }
            return command;
        }
        
        public void Start()
        {
            snakesParts = new List<(int, int)> { (7, 7) };
            direction = ConsoleKey.RightArrow;
            Console.CursorVisible = false;

            SpawnFood();
            TakeSnakesDirection();

            Console.SetCursorPosition(0, FieldHeight + 1);
            Console.WriteLine($"Game Over! Your score: {score}");
        }

        private void TakeSnakesDirection()
        {
            while (!isGameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape)
                        return;
                    if ((key == ConsoleKey.UpArrow && direction != ConsoleKey.DownArrow) ||
                        (key == ConsoleKey.DownArrow && direction != ConsoleKey.UpArrow) ||
                        (key == ConsoleKey.LeftArrow && direction != ConsoleKey.RightArrow) ||
                        (key == ConsoleKey.RightArrow && direction != ConsoleKey.LeftArrow))
                    {
                        direction = key;
                    }
                }

                MoveSnake();
                UpdateAppRepresentation();

                Thread.Sleep(300);
            }
        }

        private void DrawWalls()
        {
            for (int x = 0; x < FieldWidth + 2; x++)
            {
                Console.SetCursorPosition(x, 0);
                Console.Write("#");
                Console.SetCursorPosition(x, FieldHeight + 1);
                Console.Write("#");
            }
            for (int y = 0; y < FieldHeight + 1; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write("#");
                Console.SetCursorPosition(FieldWidth + 1, y);
                Console.Write("#");
            }
        }

        private void MoveSnake()
        {
            (int X, int Y) head = snakesParts[0];

            head = direction switch
            {
                ConsoleKey.UpArrow => (head.X, head.Y - 1),
                ConsoleKey.DownArrow => (head.X, head.Y + 1),
                ConsoleKey.LeftArrow => (head.X - 1, head.Y),   
                ConsoleKey.RightArrow => (head.X + 1, head.Y),
                _ => head
            };

            snakesParts.Insert(0, head);

            if (snakesParts.Skip(1).Any(part => part == head) || head.X <= 0 || head.X >= FieldWidth+1 || head.Y <= 0 || head.Y >= FieldHeight+1)
            {
                isGameOver = true;
                snakesParts.RemoveAt(snakesParts.Count - 1);
                return;
            }

            if (head == foodPosition)
            {
                score++;
                if(score > maxScore)
                    maxScore = score;
                SpawnFood();
            }
            else
            {
                snakesParts.RemoveAt(snakesParts.Count - 1);
            }
        }

        private void SpawnFood()
        {
            Random random = new Random();
            do
            {
                foodPosition = (random.Next(1, FieldWidth-1), random.Next(1, FieldHeight-1));
            } while (snakesParts.Contains(foodPosition));
        }

        protected override void UpdateAppRepresentation()
        {
            base.UpdateAppRepresentation();

            DrawWalls();

            if (snakesParts != null)
                foreach (var part in snakesParts)
                {
                    Console.SetCursorPosition(part.X, part.Y);
                    Console.Write("O");
                }

            Console.SetCursorPosition(foodPosition.X, foodPosition.Y);
            Console.Write("X");

            Console.SetCursorPosition(0, FieldHeight+3);
            Console.WriteLine($"Score: {score}");
            Console.SetCursorPosition(0, FieldHeight + 4);
            Console.WriteLine($"Max Score: {maxScore}");
        }

    }
}
