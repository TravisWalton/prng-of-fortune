using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prng_of_fortune
{
    class GameManager
    {
        // Total number of players
        private static int playerCount;

        // Word dictionary
        private readonly string[] wordList = { "constructor", "object", "loop", "initialization", "operator", "identifier", "syntax", "declaration", "overflow", "keyword" };

        // Chosen word
        private char[] wordLetters;

        // Number of rounds (all players complete 1 turn = 1 round)
        public int rounds;

        // Numbers of total turns 
        public int turns;

        // Chosen letter input
        private char choice;

        // Correct letters guessed list
        private List<char> correct = new List<char>();

        // Incorrect letters guessed list
        private List<char> incorrect = new List<char>();

        // Instantiated array of players
        Player[] players = new Player[4];

        // Current player
        private int currentPlayer;

        // Instantiated pseudo-random number generator
        Random rnd = new Random();

        // Array of cash values for the wheel
        int[] wheel = new int[] { 200, 200, 250, 250, 300, 300, 400, 500, 500, 700, 800, 800, 1000, 0, 0, 0 };

        public GameManager()
        {
            Title();
            Setup();
            TurnManager();
        }

        static void Title()
        {
            Console.WriteLine(@"                                  __   ______         _                    ");
            Console.WriteLine(@"                                 / _| |  ____|       | |                   ");
            Console.WriteLine(@"  _ __  _ __ _ __   __ _    ___ | |_  | |__ ___  _ __| |_ _   _ _ __   ___ ");
            Console.WriteLine(@" | '_ \| '__| '_ \ / _` |  / _ \|  _| |  __/ _ \| '__| __| | | | '_ \ / _ \");
            Console.WriteLine(@" | |_) | |  | | | | (_| | | (_) | |   | | | (_) | |  | |_| |_| | | | |  __/");
            Console.WriteLine(@" | .__/|_|  |_| |_|\__, |  \___/|_|   |_|  \___/|_|   \__|\__,_|_| |_|\___|");
            Console.WriteLine(@" | |                __/ |                                                  ");
            Console.WriteLine(@" |_|               |___/                      By Travis Walton, Version 2.0");
            Console.WriteLine(@"                                                                           ");
            Console.WriteLine("Press any key to Start");
            Console.ReadKey();
        }

        private void Setup()
        {
            Console.WriteLine("Player Setup\n");
            while (true)
            {
                try
                {
                    Console.WriteLine("How many will be playing? (1 to 4)");
                    playerCount = Int32.Parse(Console.ReadLine());

                    if (playerCount >= 1 && playerCount <= 4)
                        break;
                }
                catch
                {
                    Console.WriteLine("Please input 1, 2, 3, or 4");
                }
                

                // If the correct input is given, then break out of the while loop
                if (playerCount >= 1 && playerCount <= 4)
                    break;
            }
            Console.WriteLine("Enter player1 name:");
            players[0] = new Player();

            if (playerCount >= 2)
            {
                Console.WriteLine("Enter player2 name:");
                players[1] = new Player();
            }

            if (playerCount >= 3)
            {
                Console.WriteLine("Enter player3 name:");
                players[2] = new Player();
            }

            if (playerCount >= 4)
            {
                Console.WriteLine("Enter player4 name:");
                players[3] = new Player();
            }

            // Choose word
            ChooseWord();
        }

        private void TurnManager()
        {
            for (rounds = 1; rounds <= 6; rounds++)
            {
                for (currentPlayer = 0; currentPlayer <= playerCount - 1; ++currentPlayer)
                {
                    Turn();
                    turns++;
                }
            }
            Summary();
        }

        private void Turn()
        {

            // Display player info
            Console.Clear();
            Console.WriteLine("Round {0}/6 | Total Turns: {1} \n{2}'s Turn | Cash: {3:C}", rounds, turns, players[currentPlayer].name, players[currentPlayer].cash);

            SpinStart:

            // Get input to start the spin
            Console.WriteLine("\nPress a key to spin!");
            Console.ReadKey();

            // Call Random method to get a point value
            int mySpin = Random(0, wheel.Count());

            if (mySpin < 13)
            {
                // Spin normal point value
                Console.WriteLine("\nYou spun $" + wheel[mySpin] + "!");
                ShowWord();
                ChooseLetters(mySpin);
            }
            else if (mySpin == 13)
            {
                // Spin free spin
                Console.WriteLine("\nYou spun free spin!");
                ShowWord();
                ChooseLetters(mySpin);
                Console.WriteLine("\nNow for the 2nd spin.");
                goto SpinStart;
            }
            else if (mySpin == 14)
            {
                // Spin lose turn
                Console.WriteLine("\nYou spun lose a turn!");
            }
            else if (mySpin == 15)
            {
                // Spin bankrupt
                Console.WriteLine("\nYou spun bankrupt!");
                players[currentPlayer].cash = 0;
            }

        }
            
        public int Random(int min, int max)
        {
            return this.rnd.Next(min, max);
        }

        public void ShowWord()
        {
            //char[] displayWord = wordLetters;
            Console.WriteLine("\nThe word is...");
            //Display a (-) for unknown letters
            for (int i = 0; i < wordLetters.Length; i++) 
            {
                if (correct.Contains(wordLetters[i]))
                {
                    Console.Write(wordLetters[i]);
                }
                else
                    Console.Write("-");
            }
           
            //Console.WriteLine(displayWord);
        }

        public void ChooseLetters(int mySpin)
        {
            Console.WriteLine("\nChoose a Letter: ");

            // Get letter input 
            while(true)
            {
                try
                {
                    choice = Char.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine("\nInput a single letter.");
                }
            }
            

            // Make input lower case
            Char.ToLower(choice); 

            if (wordLetters.Contains(choice))
            {
                //Add correct letter to correct list
                correct.Add(choice);

                // Letter multiplier
                int multiplier = 0;

                foreach (char letter in wordLetters)
                {
                    if (letter == choice)
                        multiplier++;
                }
                // Cash value * same letters in word
                int finalCash = wheel[mySpin] * multiplier;

                //Add points to players cash
                players[currentPlayer].cash += finalCash; 
                Console.WriteLine("\nThat's correct! $" + wheel[mySpin] + " x " + multiplier + " = $" + finalCash + "cash added!");
            }
            else
            {
                //Add wrong letter to incorrect list
                incorrect.Add(choice); 
                Console.WriteLine("\nThat's incorrect!");
            }
            ShowWord();
            Console.WriteLine("\nPress a key to start next turn.");
            Console.ReadKey();
        }

        public void ChooseWord()
        {
            int word = Random(0, 9);
            this.wordLetters = wordList[word].ToCharArray();
        }

        public void Summary()
        {
            Console.Clear();

            Console.WriteLine("Game Complete!");
            Console.WriteLine("Player Summary");
            for(int i = 0; i <= playerCount - 1; i++)
            {
                Console.WriteLine("Player1: {0} | Cash: {1:C}", players[i].name, players[i].cash);
            }
            Console.WriteLine("Press any key to end game.");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
    }
}

