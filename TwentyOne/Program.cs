using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            //////////////////////////////////////////////
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine("Welcome to the Grand Hotel and Casino!\n\nLet's start by telling me your name.");
            string playerName = Console.ReadLine();



            Console.WriteLine("\nHello, {0}. Would you like to join a game of 21 right now?", playerName);
            // Converted to lower to assist in user input match.
            string answer = Console.ReadLine().ToLower();

            // Relocated this chunk, thought it made more logical sense that you'd first agree to a game? It was originall just above the "Hello fjdklj, would you....."
            Console.WriteLine("\nAnd how much money did you bring today?");
            // Converted string input into an integer.
            int bank = Convert.ToInt32(Console.ReadLine()); 

            // Providing different possible user input matches to ease runability.
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                // Abstantiated Contstructor from "Player" Class.
                Player player = new Player(playerName, bank);
                //Abstantiated Constructor & the use of Polymorphism to expose overloaded operators.
                Game game = new TwentyOneGame();
                // Adding player to "Game", += is the same as doing the whole equation written out c = a + b, for example a += b.
                game += player;
                player.isActivelyPlaying = true;


                // Told to use while loops with caution, even though we'll have multiple in this program, silly, but for demonstation purposes I suppose!

                // While loop to check the status of "isActivelyPlaying & Balance variables to verify if program should be running or not.
                // While both are "true" the program will fire.
                while (player.isActivelyPlaying && player.Balance > 0)
                {
                    game.Play();
                }
                // Operates just after exiting the loop
                game -= player;
                Console.WriteLine("\nThank you for playing!");
            }
            // If user were to type something besides "yes, yah, etc", this will fire, I think generally this would be an "else" statement.
            Console.WriteLine("\nFeel free to look around the casino. Bye for now!");
            Console.Read();
            // All other operations happen outside of this main method in other classes.
        }
    }
}