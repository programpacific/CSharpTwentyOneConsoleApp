using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{
    class TwentyOneGame : Game, IWalkAway
    {
        public TwentyOneDealer Dealer { get; set; }

        // This fires from the "Main" method.
        public override void Play()
        {// This is ran per each individual hand!
            // Abstantiated a new "dealer" specific to the twenty one type game.
            Dealer = new TwentyOneDealer();

            //Loops through players in "Player" list, as there could be multiple players.
            foreach (Player player in Players)
            {
                player.Hand = new List<Card>();
                player.Stay = false;
            }
            
            // Refreshes deck and hand per each round.
            Dealer.Hand = new List<Card>();
            Dealer.Stay = false;
            Dealer.Deck = new Deck();

            Console.WriteLine("Place your bet!");
            foreach (Player player in Players)
            {
                int bet = Convert.ToInt32(Console.ReadLine());
                // Passing in amount entered in "Player" bet method.
                bool successfullyBet = player.Bet(bet);

                //if (successfullyBet == false) the same as below, but better to use shorthand option!
                if (!successfullyBet)
                {
                    // Even though this is a void method this is telling the program to "end" this particular method & will go back to the top.
                    return;
                }
                // If true, instead of putting an "else" statement this will fire.
                // A data type "dictionary" holds a collection of "key value pairs".
                // Added dictionary entry!
                Bets[player] = bet;

            }

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Dealing...");
                foreach (Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);
                    Dealer.Deal(player.Hand);
                    if ( i == 1) // Means second turn.
                    {

                    }

                }
            }

        }
        public override void ListPlayers()
        {
            Console.WriteLine("21 Players:");
            base.ListPlayers();
        }

        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
