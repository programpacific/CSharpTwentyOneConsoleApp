using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Casino;
using Casino.TwentyOne;
using System.Data.SqlClient;
using System.Data;

namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            const string casinoName = "Grand Hotel and Casino";
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;            
            Console.WriteLine("Welcome to the {0}!\n\n", casinoName);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Let's start by telling me your name.");
            string playerName = Console.ReadLine();
            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = ReadException();
                foreach (var exception in Exceptions) // Looping through the list
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();
                }
                Console.Read();
                return;
            }





            Console.WriteLine("\nHello, {0}. Would you like to join a game of 21 right now?", playerName);
            // Converted to lower to assist in user input match.
            string answer = Console.ReadLine().ToLower();

            // Relocated this chunk, thought it made more logical sense that you'd first agree to a game? It was originall just above the "Hello fjdklj, would you....."

            // This is designed to prevent errors and to handle them appropriately when needed.
            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {          
                Console.WriteLine("How much money did you bring today?"); 
                validAnswer = int.TryParse(Console.ReadLine(), out bank); // This extracts the value inputted by user, attempts to convert it, logs a bool for comp. in "validAnswer", if successful outputs the converted int to "bank", otherwise continues below.
                if (!validAnswer) Console.WriteLine("Please enter whole dollars only, no change!.");  // This is an alternate method, but is not as adaptive for error handling - int bank = Convert.ToInt32(Console.ReadLine()); 
            }

            // Providing different possible user input matches to ease runability.
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                // Abstantiated Contstructor from "Player" Class.
                Player player = new Player(playerName, bank);
                // Special "guid" identifier.
                player.Id = Guid.NewGuid();

                using (StreamWriter file = new StreamWriter(@"C:\Users\New\Desktop\TwentyOneLog.txt", true))
                {
                    file.WriteLine(player.Name.ToUpper()); // Added myself, thought it made sense?
                    file.WriteLine(player.Id);
                }

                //Abstantiated Constructor & the use of Polymorphism to expose overloaded operators.
                Game game = new TwentyOneGame();
                // Adding player to "Game", += is the same as doing the whole equation written out c = a + b, for example a += b.
                game += player;
                player.isActivelyPlaying = true;


                // Told to use while loops with caution, even though we'll have multiple in this program, but for demonstation purposes I suppose!

                // While loop to check the status of "isActivelyPlaying & Balance variables to verify if program should be running or not.
                // While both are "true" the program will fire.
                while (player.isActivelyPlaying && player.Balance > 0)
                {
                    try
                    {
                        game.Play();
                    }
                    catch (FraudException ex)
                    {
                        Console.WriteLine(ex.Message); // Passes through custom exception message.
                        UpdateDbWithException(ex); // Updates database with exception message.
                        Console.ReadLine();
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred. Please contact your System Administrator.");
                        UpdateDbWithException(ex);
                        Console.ReadLine();
                        return; // While in a void method, return will kill the program. That can come in handy.
                    }
                }
                // Operates just after exiting the loop
                game -= player;
                Console.WriteLine("\nThank you for playing!");
            }
            // If user were to type something besides "yes, yah, etc", this will fire, I think generally this would be an "else" statement.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nFeel free to look around the casino. Bye for now!");
            Console.Read();
            // All other operations happen outside of this main method in other classes.
        }

        private static void UpdateDbWithException(Exception ex)
        {// connectionString is the address to the database acquired from the properties of the database in the connection string field.
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;Integrated Security=True;Connect Timeout=30;
                                        Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            // Provide strict values that may be used, this will prevent easy sql injection attacks.
            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) Values
                                  (@ExceptionType, @ExceptionMessage, @TimeStamp)";
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {// Makes a new instance and provides the paramaters to be filled into sql database.
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);

                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();
                command.Parameters["@ExceptionMessage"].Value = ex.Message;
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        } // Below returns a list of exceptions and displays them.
        private static List<ExceptionEntity> ReadException()
        {
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;Integrated Security=True;Connect Timeout=30;
                                        Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            string queryString = @"Select Id, ExceptionType, ExceptionMessage,TImeStamp From Exceptions";

            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {// What is inside the curly brackets is what happens while there is a sql connection.
                SqlCommand command = new SqlCommand(queryString, connection); // Passing in parameters.


                connection.Open(); // Opens sql connection

                SqlDataReader reader = command.ExecuteReader(); // Extracts information from database

                while (reader.Read()) // Reads objects in database.
                {
                    ExceptionEntity exception = new ExceptionEntity();
                    exception.Id = Convert.ToInt32(reader["Id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);
                    Exceptions.Add(exception);

                }
                connection.Close();
            }

            return Exceptions; // Returns a list of exceptions using ADO.NET Framework
        }
    }
}