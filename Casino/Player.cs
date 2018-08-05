using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class Player
    {
        public Player(string name) : this(name, 100) // This constructor recycles the below constructor & assigns the values from below. 
        {
        }
        // Player Constructor
        // Always include at the top of a class!
        public Player(string name, int beginningBalance)
        {
            Hand = new List<Card>();
            Balance = beginningBalance;
            Name = name;
        }

        private List<Card> _hand = new List<Card>();
        public List<Card> Hand { get { return _hand; } set { _hand = value; } }
        public int Balance { get; set; }
        public string Name { get; set; }
        public bool isActivelyPlaying { get; set; }
        public bool Stay { get; set; }
        public Guid Id { get; set; }

        //"Bet" logic should be in the "Player" class due to it being specific to the player. Keeps the code making logically sense!
        public bool Bet(int amount)
        {// Verifies that bet amount is available from users "Balance" variable.
            if (Balance - amount < 0)
            {
                Console.WriteLine("\n\nYou do not have enough to place a bet that size.");
                return false;
            }
            else
            {
                //Balance = Balance - amount; same thing, but better to write in short hand!
                Balance -= amount;
                return true;
            }
        }

        public static Game operator+ (Game game, Player player)
        {
            game.Players.Add(player); // We first were unable to run the code because the list had not been abstantiated yet. Without it, the code would break and hang here.
            return game;
        }

        public static Game operator- (Game game, Player player)
        {
            game.Players.Remove(player);
            return game;
        }
    }
}
