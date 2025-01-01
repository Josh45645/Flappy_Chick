using Flappy_Chick;
using NAudio.Wave;

namespace Flappy_Chick
{
    internal class Program
    {
        static bool isMusicOn = true;  
        static float volume = 0.5f;  
        static WaveOutEvent waveOut;
        static AudioFileReader audioFileReader;

        static int highScore = 0;

        static void Main(string[] args)
        {
            SoundManager.PlayBackgroundMusic(@"Sounds\Background.mp3", loop: true);
            ShowMainMenu();
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.CursorVisible = false;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            string titleArt = @"
 ███████╗██╗      █████╗ ██████╗ ██████╗ ██╗   ██╗     ██████╗██╗  ██╗██╗ ██████╗██╗  ██╗
 ██╔════╝██║     ██╔══██╗██╔══██╗██╔══██╗██║   ██║    ██╔════╝██║  ██║██║██╔════╝██║ ██╔╝
 █████╗  ██║     ███████║██████╔╝██████╔╝ ██  ██═╝    ██║     ███████║██║██║     █████╔╝ 
 ██╔══╝  ██║     ██╔══██║██╔═══╝ ██╔═══╝   ║██║       ██║     ██╔══██║██║██║     ██╔═██╗ 
 ██║     ███████╗██║  ██║██║     ██║       ║██║       ╚██████╗██║  ██║██║╚██████╗██║  ██╗
 ╚═╝     ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝       ╚══╝        ╚═════╝╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
";

            string topBorder = GeneratePipeBorder(consoleWidth);
            string bottomBorder = GeneratePipeBorder(consoleWidth);

            string[] menuOptions = { "Start", "About Us", "Settings", "Exit" };
            int selectedOption = 0;

            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(topBorder);

                Console.ForegroundColor = ConsoleColor.Cyan;
                string[] artLines = titleArt.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (var line in artLines)
                {
                    Console.SetCursorPosition((consoleWidth - line.Length) / 2, Console.CursorTop);
                    Console.WriteLine(line);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(bottomBorder);

                Console.ResetColor();
                Console.WriteLine();

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (i == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition((consoleWidth - menuOptions[i].Length) / 2, Console.CursorTop);
                        Console.WriteLine($"> {menuOptions[i]} <");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition((consoleWidth - menuOptions[i].Length) / 2, Console.CursorTop);
                        Console.WriteLine(menuOptions[i]);
                    }
                }

                int consoleWidthInstuction = Console.WindowWidth;
                int consoleHeightInstuction = Console.WindowHeight;

                string instruction = "Press Enter to select and WASD/Arrow Keys to navigate...";
                int frameDelay = 500;
                DateTime lastUpdateTime = DateTime.Now;
                bool showPrompt = true;

                int promptPositionY = consoleHeightInstuction - 3;
                int promptPositionX = (consoleWidthInstuction - instruction.Length) / 2;

                while (!Console.KeyAvailable)
                {
                    if ((DateTime.Now - lastUpdateTime).TotalMilliseconds >= frameDelay)
                    {
                        lastUpdateTime = DateTime.Now;

                        Console.SetCursorPosition(promptPositionX, promptPositionY);
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        if (showPrompt)
                        {
                            Console.WriteLine(instruction);
                        }
                        else
                        {
                            Console.WriteLine(new string(' ', instruction.Length));
                        }

                        showPrompt = !showPrompt;
                    }
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Select.mp3");
                    selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
                }
                else if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Select.mp3");
                    selectedOption = (selectedOption + 1) % menuOptions.Length;
                }
                else if (key == ConsoleKey.Enter)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Push.mp3");
                    HandleMenuSelection(selectedOption);
                    break;
                }
            }
        }

        static string GeneratePipeBorder(int width)
        {
            string topBorder = "╔";
            for (int i = 0; i < width - 2; i++)
            {
                topBorder += "═";
            }
            topBorder += "╗";

            string bottomBorder = "╚";
            for (int i = 0; i < width - 2; i++)
            {
                bottomBorder += "═";
            }
            bottomBorder += "╝";

            return (topBorder + "\n" + bottomBorder);
        }

        static void HandleMenuSelection(int pick)
        {
            Console.Clear();
            switch (pick)
            {
                case 0:
                    StartFlappyBirdGame();
                    break;
                case 1:
                    SoundManager.PlaySoundEffect(@"Sounds\Push.mp3");
                    ShowAboutUs();
                    break;
                case 2:
                    SoundManager.PlaySoundEffect(@"Sounds\Push.mp3");
                    ShowSettings();
                    break;
                case 3:
                    SoundManager.PlaySoundEffect(@"Sounds\Push.mp3");
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(2000);
                    Console.WriteLine("Thank You For Playing!!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Exiting...");
                    Console.ResetColor();
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;
            }
        }

        static void ShowAboutUs()
        {
            Console.Clear();

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            string asciiArt = @"
  █████╗ ██████╗  █████╗ ██╗   ██╗████████╗  ██╗   ██╗ ██████╗
 ██╔══██╗██╔══██╗██╔══██╗██║   ██║╚══██╔══╝  ██║   ██║██╔════╝
███████║██████╦╝██║  ██║██║   ██║   ██║     ██║   ██║╚█████╗
 ██╔══██║██╔══██╗██║  ██║██║   ██║   ██║     ██║   ██║ ╚═══██╗
 ██║  ██║██████╦╝╚█████╔╝╚██████╔╝   ██║     ╚██████╔╝██████╔╝
 ╚═╝  ╚═╝╚═════╝  ╚════╝  ╚═════╝    ╚═╝      ╚═════╝ ╚═════╝ ";

            string flappyChick = "Flappy Chick";

            string[] developerNames =
            {
        "Joshua Bartolome",
        "John Christopher Pascual",
        "Cholo Duran"
    };

            string[] developerDetails =
            {
        "Hello, I'm Joshua Bartolome, we are from LSPU (Laguna State Polytechnic University). I'm the team leader for this project. I organized the team's workflow, ensured deadlines were met, and contributed to both the code and design aspects of the game.",
        "Hi, I am John Christopher Pascual. I came up with the initial idea for the project and started by implementing the basic functionalities of the game, setting the foundation for what it is today.",
        "Hello, I am Cholo Duran. I focused on preparing the necessary documentation and contributed design suggestions for the project, ensuring that both aesthetics and details were well-considered."
    };

            string purpose = "We created this game as a requirement for our Final Project and, of course, to build a fun and challenging game that helps players develop quick reflexes and coordination. Have FUN!";
            string instructions = "Press any key to return to the menu...";

            string[] asciiArtLines = asciiArt.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            bool toggleArtColor = true;
            bool showInstructions = true;
            DateTime lastUpdateTime = DateTime.Now;
            int frameDelay = 500; 
            while (!Console.KeyAvailable)
            {
                if ((DateTime.Now - lastUpdateTime).TotalMilliseconds >= frameDelay)
                {
                    lastUpdateTime = DateTime.Now;
                    Console.Clear();

                   
                    int contentHeight = asciiArtLines.Length + developerNames.Length + 8 + developerDetails.Length * 2;
                    int verticalPosition = (consoleHeight - contentHeight) / 2;

                    
                    Console.ForegroundColor = toggleArtColor ? ConsoleColor.Yellow : ConsoleColor.Gray;
                    foreach (var line in asciiArtLines)
                    {
                        int linePosition = (consoleWidth - line.Length) / 2;
                        Console.SetCursorPosition(linePosition, verticalPosition++);
                        Console.WriteLine(line);
                    }

                   
                    Console.ForegroundColor = ConsoleColor.Green;
                    int flappyPosition = (consoleWidth - flappyChick.Length) / 2;
                    Console.SetCursorPosition(flappyPosition, verticalPosition++);
                    Console.WriteLine(flappyChick);

                    verticalPosition++; 

                    
                    for (int i = 0; i < developerNames.Length; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        int namePosition = (consoleWidth - developerNames[i].Length) / 2;
                        Console.SetCursorPosition(namePosition, verticalPosition++);
                        Console.WriteLine(developerNames[i]);

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        string detail = developerDetails[i];
                        string[] detailLines = SplitTextToLines(detail, consoleWidth - 10);

                        foreach (var detailLine in detailLines)
                        {
                            int detailPosition = (consoleWidth - detailLine.Length) / 2;
                            Console.SetCursorPosition(detailPosition, verticalPosition++);
                            Console.WriteLine(detailLine);
                        }

                        verticalPosition++; 
                    }

                    
                    Console.ForegroundColor = ConsoleColor.Gray;
                    string[] purposeLines = SplitTextToLines(purpose, consoleWidth - 10);

                    foreach (var line in purposeLines)
                    {
                        int purposePosition = (consoleWidth - line.Length) / 2;
                        Console.SetCursorPosition(purposePosition, verticalPosition++);
                        Console.WriteLine(line);
                    }

                    verticalPosition++; 

                    
                    if (showInstructions)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        int instructionPosition = (consoleWidth - instructions.Length) / 2;
                        Console.SetCursorPosition(instructionPosition, verticalPosition);
                        Console.WriteLine(instructions);
                    }

                  
                    toggleArtColor = !toggleArtColor;
                    showInstructions = !showInstructions;
                }
            }

            Console.ReadKey(intercept: true);
            ShowMainMenu();
        }



        
        static string[] SplitTextToLines(string text, int maxLineWidth)
        {
            List<string> lines = new List<string>();
            string[] words = text.Split(' ');

            string currentLine = "";
            foreach (var word in words)
            {
                if ((currentLine + word).Length > maxLineWidth)
                {
                    lines.Add(currentLine.Trim());
                    currentLine = "";
                }
                currentLine += word + " ";
            }
            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine.Trim());

            return lines.ToArray();
        }






        static void StartFlappyBirdGame()
        {
            SoundManager.StopBackgroundMusic();
            SoundManager.PlaySoundEffect(@"Sounds\Start.mp3");

            Console.Clear();
            Console.WriteLine("Starting Flappy Chick Please Wait ;)...");
            Thread.Sleep(2000);
            Console.Clear();
            Console.CursorVisible = false;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int groundLevel = consoleHeight - 2;

            int birdX = 10;
            int birdY = consoleHeight / 2;
            int birdHeight = 2;
            int birdWidth = 3;

            int score = 0;
            string deathCause = "";
            bool isGameOver = false;
            bool isPaused = false;
            bool isCoinCollected = false;

            List<int> pipeX = new List<int>();
            List<int> pipeHeightTop = new List<int>();
            List<int> pipeHeightBottom = new List<int>();
            List<int> coinX = new List<int>();
            List<int> coinY = new List<int>();
            List<string> coinTypes = new List<string>();
            List<bool> scoredPipes = new List<bool>();

            int collectedPoints = 0;
            int indicatorTimer = 0;

            int gravity = 1;
            int flapStrength = -3;
            int minFlapStrength = -1; 
            int flapStrengthDecreaseRate = -1; 
            int nextFlapStrengthDecreaseScore = 100; 

            int initialHorizontalGap = 100;
            int horizontalGap = initialHorizontalGap;
            int minHorizontalGap = 25;

            int horizontalGapDecreaseRate = 10; 
            int nextHorizontalGapDecreaseScore = 10;

            int maxPipeTopHeight = consoleHeight - 12; 

            Random rand = new Random();
            int coinSpawnRate = 20;
            int coinTimer = 0;

            int frameDelay = 100;
            DateTime lastUpdateTime = DateTime.Now;

            ShowIntroScreen(birdX, birdY, birdWidth);

            bool gameStarted = false;

            void DrawGround()
            {
                Console.SetCursorPosition(0, groundLevel);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('=', consoleWidth));
            }

            while (!isGameOver)
            {
                TimeSpan elapsedTime = DateTime.Now - lastUpdateTime;
                if (elapsedTime.TotalMilliseconds >= frameDelay)
                {
                    lastUpdateTime = DateTime.Now;

                    if (isPaused)
                    {
                        Console.SetCursorPosition(consoleWidth / 2 - 7, consoleHeight / 2);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Game Paused! Press 'P' to Resume.");
                        if (Console.KeyAvailable && Console.ReadKey(intercept: true).Key == ConsoleKey.P)
                            isPaused = false;

                        continue;
                    }

                    Console.Clear();
                    DrawPipes(pipeX, pipeHeightTop, pipeHeightBottom, consoleHeight);
                    DrawBird(birdX, birdY, birdWidth);
                    DrawCoins(coinX, coinY, coinTypes);
                    DrawGround();

                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Score: {score}");
                    Console.WriteLine($"High Score: {highScore}");
                    Console.WriteLine("Press 'P' to Pause"); 

                    if (indicatorTimer > 0)
                    {
                        Console.SetCursorPosition(birdX + 2, birdY - 1);
                        Console.ForegroundColor = collectedPoints == 2 ? ConsoleColor.Yellow :
                                                  collectedPoints == 5 ? ConsoleColor.Blue :
                                                  ConsoleColor.Magenta;
                        Console.Write($"+{collectedPoints}");
                        indicatorTimer--;
                    }

                    if (isCoinCollected)
                    {
                        isCoinCollected = false;
                    }

                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(intercept: true).Key;
                        if (key == ConsoleKey.Spacebar)
                        {
                            Task.Run(() => SoundManager.PlaySoundEffect(@"Sounds\Flap.mp3"));
                            if (!gameStarted)
                            {
                                gameStarted = true;
                            }
                            birdY += flapStrength;
                        }
                        else if (key == ConsoleKey.P)
                        {
                            Task.Run(() => SoundManager.PlaySoundEffect(@"Sounds\Pause.mp3"));
                            isPaused = true;
                        }
                    }
                    else if (gameStarted)
                    {
                        birdY += gravity;
                    }

                    if (birdY < 0) birdY = 0;

                    if (birdY >= groundLevel - birdHeight)
                    {
                        isGameOver = true;
                        deathCause = "Fell to the ground";
                    }

                    for (int i = 0; i < pipeX.Count; i++)
                    {
                        pipeX[i]--;

                        if (!scoredPipes[i] && birdX > pipeX[i] + 2)
                        {
                            score++;
                            Task.Run(() => SoundManager.PlaySoundEffect(@"Sounds\Point.mp3"));
                            if (score > highScore) highScore = score;
                            scoredPipes[i] = true;
                        }
                    }

                    if (pipeX.Count > 0 && pipeX[0] < 0)
                    {
                        pipeX.RemoveAt(0);
                        pipeHeightTop.RemoveAt(0);
                        pipeHeightBottom.RemoveAt(0);
                        scoredPipes.RemoveAt(0);
                    }

                    if (score >= nextHorizontalGapDecreaseScore)
                    {
                        horizontalGap = Math.Max(minHorizontalGap, horizontalGap - horizontalGapDecreaseRate);
                        nextHorizontalGapDecreaseScore += 10; 

                     
                       
                            Console.SetCursorPosition(consoleWidth / 2 - 10, consoleHeight / 2 + 1);
                       
                        Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Pipes are getting closer!");
                        SoundManager.PlaySoundEffect(@"Sounds\Danger.mp3");
                        Thread.Sleep(1000);
                            Console.SetCursorPosition(consoleWidth / 2 - 10, consoleHeight / 2 + 1);
                            Console.Write(new string(' ', "Pipes are getting closer!".Length)); 
                    }

                    if (pipeX.Count == 0 || pipeX[^1] < consoleWidth - horizontalGap)
                    {
                        int pipeTop = rand.Next(3, maxPipeTopHeight);  
                        pipeX.Add(consoleWidth - 2);
                        pipeHeightTop.Add(pipeTop);
                        pipeHeightBottom.Add(pipeTop + 10);
                        scoredPipes.Add(false);
                    }

                    coinTimer++;
                    if (coinTimer >= coinSpawnRate)
                    {
                        coinTimer = 0;
                        int coinXPosition = consoleWidth - 2;
                        for (int i = 0; i < pipeX.Count; i++)
                        {
                            if (pipeX[i] > coinXPosition - 5 && pipeX[i] < coinXPosition + 5)
                            {
                                int coinYPosition = rand.Next(pipeHeightTop[i] + 1, pipeHeightBottom[i] - 1);
                                if (coinYPosition < groundLevel - 1)
                                {
                                    coinX.Add(coinXPosition);
                                    coinY.Add(coinYPosition);
                                    coinTypes.Add(GetRandomCoinType(rand));
                                }
                            }
                        }
                    }

                    for (int i = coinX.Count - 1; i >= 0; i--)
                    {
                        coinX[i]--;
                        if (coinX[i] < 0)
                        {
                            coinX.RemoveAt(i);
                            coinY.RemoveAt(i);
                            coinTypes.RemoveAt(i);
                        }
                    }

                    for (int i = coinX.Count - 1; i >= 0; i--)
                    {
                        bool isCoinHit = birdX < coinX[i] + 1 &&
                                         birdX + birdWidth > coinX[i] &&
                                         birdY < coinY[i] + 1 &&
                                         birdY + birdHeight > coinY[i];

                        if (isCoinHit)
                        {
                            int coinPoints = GetCoinScore(coinTypes[i]);
                            score += coinPoints;
                            Task.Run(() => SoundManager.PlaySoundEffect(@"Sounds\Coin.mp3"));
                            isCoinCollected = true;

                            collectedPoints = coinPoints;
                            indicatorTimer = 10;

                            coinX.RemoveAt(i);
                            coinY.RemoveAt(i);
                            coinTypes.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < pipeX.Count; i++)
                    {
                        if (birdX + birdWidth - 1 >= pipeX[i] && birdX <= pipeX[i] + 2)
                        {
                            if (birdY <= pipeHeightTop[i] || (birdY + birdHeight - 1) >= pipeHeightBottom[i])
                            {
                                isGameOver = true;
                                deathCause = "Collided with the pipe";
                            }
                        }
                    }

                   
                    if (score >= nextFlapStrengthDecreaseScore)
                    {
                        flapStrength = Math.Max(minFlapStrength, flapStrength + flapStrengthDecreaseRate);
                        nextFlapStrengthDecreaseScore += 100; 
                      


                        
                            Console.SetCursorPosition(consoleWidth / 2 - 10, consoleHeight / 2);
                            
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Flap Strength Decreased!");
                        SoundManager.PlaySoundEffect(@"Sounds\Danger.mp3");
                        Thread.Sleep(1000); 
                            Console.SetCursorPosition(consoleWidth / 2 - 10, consoleHeight / 2);
                            Console.Write(new string(' ', "Flap Strength Decreased!".Length)); 
                    }
                }
            }

            GameOver(score, highScore, deathCause);
        }




        static void DrawCoins(List<int> coinX, List<int> coinY, List<string> coinTypes)
        {
            for (int i = 0; i < coinX.Count; i++)
            {
                int safeX = Math.Clamp(coinX[i], 0, Console.BufferWidth - 1);
                int safeY = Math.Clamp(coinY[i], 0, Console.BufferHeight - 1);

                Console.SetCursorPosition(safeX, safeY);
                Console.ForegroundColor = coinTypes[i] == "yellow" ? ConsoleColor.Yellow :
                                          coinTypes[i] == "blue" ? ConsoleColor.Blue :
                                          ConsoleColor.Magenta;
                Console.Write("O");
            }
        }

        static string GetRandomCoinType(Random rand)
        {
            int chance = rand.Next(100);
            return chance < 60 ? "yellow" : chance < 85 ? "blue" : "purple";
        }

        static int GetCoinScore(string coinType)
        {
            return coinType == "yellow" ? 2 : coinType == "blue" ? 5 : 10;
        }

        static void ShowIntroScreen(int birdX, int birdY, int birdWidth)
        {
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            Console.Clear();
            DrawBird(birdX, birdY, birdWidth);

            Console.SetCursorPosition(consoleWidth / 2 - 9, consoleHeight / 2 - 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Flappy Chick");

            Console.SetCursorPosition(consoleWidth / 2 - 18, consoleHeight / 2 + 1);
            Console.WriteLine("Press Spacebar to Flap and Start");

            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(intercept: true).Key == ConsoleKey.Spacebar)
                {
                    break;
                }
            }
        }

        static void DrawPipes(List<int> pipeX, List<int> pipeHeightTop, List<int> pipeHeightBottom, int consoleHeight)
        {
            for (int i = 0; i < pipeX.Count; i++)
            {
                for (int j = 0; j < pipeHeightTop[i]; j++)
                {
                    Console.SetCursorPosition(pipeX[i], j);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("█");
                }

                for (int j = pipeHeightBottom[i]; j < consoleHeight - 1; j++)
                {
                    Console.SetCursorPosition(pipeX[i], j);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("█");
                }
            }
        }

        static void DrawBird(int x, int y, int birdWidth)
        {
            string birdTop = " ___( o)> ";
            string birdBody = @" \ <_. ) ";
            string birdBottom = "  ---'   ";

            for (int i = 0; i < birdWidth; i++)
            {
                Console.SetCursorPosition(x + i, y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(birdTop);
                Console.SetCursorPosition(x + i, y + 1);
                Console.Write(birdBody);
                Console.SetCursorPosition(x + i, y + 2);
                Console.Write(birdBottom);
            }
        }

        static void GameOver(int score, int highScore, string deathCause)
        {
            SoundManager.PlaySoundEffect(@"Sounds\Death.mp3");
            Console.Clear();
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            string gameOverArt = @"
  ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ 
 ██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗
 ██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝
 ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║██║   ██║██╔══╝  ██╔══██║
 ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝╚██████╔╝███████╗██║  ██║
  ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝  ╚═════╝ ╚══════╝╚═╝  ╚═╝ 
";
            string[] artLines = gameOverArt.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            Console.ForegroundColor = ConsoleColor.Red;

            foreach (var line in artLines)
            {
                Console.SetCursorPosition((consoleWidth - line.Length) / 2, Console.CursorTop);
                Console.WriteLine(line);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            string scoreText = $"\nYour Score: {score}";
            Console.SetCursorPosition((consoleWidth - scoreText.Length) / 2, Console.CursorTop);
            Console.WriteLine(scoreText);

            string highScoreText = $"\nHigh Score: {highScore}";
            Console.SetCursorPosition((consoleWidth - highScoreText.Length) / 2, Console.CursorTop);
            Console.WriteLine(highScoreText);

            string causeText = $"You {deathCause} :(";
            Console.SetCursorPosition((consoleWidth - causeText.Length) / 2, Console.CursorTop + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(causeText);

            string prompt = "Press Enter to go back to the menu...";
            int frameDelay = 500;
            DateTime lastUpdateTime = DateTime.Now;
            bool showPrompt = true;
            int promptPositionY = consoleHeight - 3;
            int promptPositionX = (consoleWidth - prompt.Length) / 2;
            SoundManager.PlaySoundEffect(@"Sounds\GameOver.mp3");

            while (!Console.KeyAvailable)
            {
                if ((DateTime.Now - lastUpdateTime).TotalMilliseconds >= frameDelay)
                {
                    lastUpdateTime = DateTime.Now;

                    Console.SetCursorPosition(promptPositionX, promptPositionY);
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    if (showPrompt)
                    {
                        Console.WriteLine(prompt);
                    }
                    else
                    {
                        Console.WriteLine(new string(' ', prompt.Length));
                    }

                    showPrompt = !showPrompt;
                }
            }

            Console.ResetColor();

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.Enter);

            Main(null);
           
        }

        static void ShowSettings()
        {
            int currentSelection = 0; 
            bool settingsActive = true;

            while (settingsActive)
            {
                Console.Clear();
              

                
               

                
                HighlightOption(currentSelection);

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow|| key==ConsoleKey.W)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Select.mp3");
                    currentSelection = (currentSelection == 0) ? 1 : 0; 
                }
                else if (key == ConsoleKey.DownArrow|| key == ConsoleKey.S)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Select.mp3");
                    currentSelection = (currentSelection == 1) ? 0 : 1; 
                }
                else if (key == ConsoleKey.Enter)
                {
                    if (currentSelection == 0)
                    {
                        SoundManager.PlaySoundEffect(@"Sounds\Push.mp3");   
                        isMusicOn = !isMusicOn;
                        ToggleMusic();
                    }
                    else if (currentSelection == 1)
                    {
                        
                        AdjustVolume();
                    }
                }
                else if (key == ConsoleKey.Escape)
                {
                    settingsActive = false; 
                    ShowMainMenu(); 
                }
            }
        }

    
        static void HighlightOption(int selectedOption)
{
    string[] menuOptions = 
    {
        $"1. Toggle Background Music: {(isMusicOn ? "On" : "Off")}",
        $"2. Volume: {Math.Round(SoundManager.GetVolume() * 100)}%"
    };

    int consoleWidth = Console.WindowWidth;

    for (int i = 0; i < menuOptions.Length; i++)
    {
        if (i == selectedOption)
        {
            
                    Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition((consoleWidth - menuOptions[i].Length) / 2, Console.CursorTop);
            Console.WriteLine($"> {menuOptions[i]} <");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((consoleWidth - menuOptions[i].Length) / 2, Console.CursorTop);
            Console.WriteLine(menuOptions[i]);
        }
    }

    Console.ResetColor();

           
            string prompt = "Use Up/Down arrow keys to navigate, Enter to select, ESC to exit.";
            int promptPositionX = (Console.WindowWidth - prompt.Length) / 2;
            int promptPositionY = Console.WindowHeight - 2;

            
            bool showPrompt = true;
            DateTime lastUpdateTime = DateTime.Now;
            int frameDelay = 500; 

            while (!Console.KeyAvailable) 
            {
                if ((DateTime.Now - lastUpdateTime).TotalMilliseconds >= frameDelay)
                {
                    lastUpdateTime = DateTime.Now;
                    Console.SetCursorPosition(promptPositionX, promptPositionY);
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    if (showPrompt)
                    {
                        Console.WriteLine(prompt);
                    }
                    else
                    {
                        Console.WriteLine(new string(' ', prompt.Length)); 
                    }

                    showPrompt = !showPrompt;
                }
            }
        }


        static void AdjustVolume()
        {
            bool volumeAdjusting = true;

            while (volumeAdjusting)
            {
                Console.Clear();

               
                string title = "Adjust Volume:";
                string volumeDisplay = $"Current Volume: {Math.Round(SoundManager.GetVolume() * 100)}%";
                string instructions = "Press Up/Down to adjust, Enter to save, ESC to cancel.";
                int consoleWidth = Console.WindowWidth;
                int titleX = (consoleWidth - title.Length) / 2;
                int volumeX = (consoleWidth - volumeDisplay.Length) / 2;
                int instructionsX = (consoleWidth - instructions.Length) / 2;

               
                Console.SetCursorPosition(titleX, Console.CursorTop);
                Console.WriteLine(title);

                Console.SetCursorPosition(volumeX, Console.CursorTop);
                Console.WriteLine(volumeDisplay);

                
                bool showInstructions = true;
                DateTime lastUpdateTime = DateTime.Now;
                int frameDelay = 500; 

                
                while (!Console.KeyAvailable)
                {
                    if ((DateTime.Now - lastUpdateTime).TotalMilliseconds >= frameDelay)
                    {
                        lastUpdateTime = DateTime.Now;
                        Console.SetCursorPosition(instructionsX, Console.WindowHeight - 3);
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        if (showInstructions)
                        {
                            Console.WriteLine(instructions);
                        }
                        else
                        {
                            Console.WriteLine(new string(' ', instructions.Length));
                        }

                        showInstructions = !showInstructions;
                    }
                }

               
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Select.mp3");
                    float newVolume = SoundManager.GetVolume() + 0.1f;
                    SoundManager.SetVolume(Math.Min(newVolume, 1.0f)); 
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    SoundManager.PlaySoundEffect(@"Sounds\Select.mp3");
                    float newVolume = SoundManager.GetVolume() - 0.1f;
                    SoundManager.SetVolume(Math.Max(newVolume, 0.0f));
                }
                else if (key == ConsoleKey.Enter || key == ConsoleKey.Escape)
                {
                    volumeAdjusting = false; 
                }
            }
        }



        static void ToggleMusic()
        {
            if (isMusicOn)
            {
                SoundManager.PlayBackgroundMusic(@"Sounds\Background.mp3", loop: true);
            }
            else
            {
                SoundManager.StopBackgroundMusic(); 
            }
        }
    }

}