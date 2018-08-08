using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Interfaces;

namespace Casino.TwentyOne
{
    public class TwentyOneGame : Game, IWalkAway
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
            Dealer.Deck.Shuffle();

            foreach (Player player in Players)
            {

                bool validAnswer = false;
                int bet = 0;
                    while (!validAnswer)
                {
                    Console.WriteLine("Place your bet!");
                    validAnswer = int.TryParse(Console.ReadLine(), out bet);
                    if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals.");
                }

                    if (bet < 0)
                {
                    throw new FraudException("Security! Kick this person out!");
                }

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
                Console.WriteLine("\n\nDealing...");
                foreach (Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);
                    Dealer.Deal(player.Hand);
                    if ( i == 1) // Means second turn.
                    {
                        bool blackJack = TwentyOneRules.CheckForBlackJack(player.Hand);
                        if (blackJack)
                        {
                            Console.WriteLine("\n\n\nBlackjack! {0} wins {1}", player.Name, Bets[player]); // Bets[player] retrieves the original amount bet.
                            player.Balance += Convert.ToInt32((Bets[player] * 1.5) + Bets[player]); // Adds 1.5x + original bet back!
                            return;
                        }
                    }

                }
                Console.Write("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool blackJack = TwentyOneRules.CheckForBlackJack(Dealer.Hand);
                    if (blackJack)
                    {
                        Console.WriteLine("\n\n\nDealer has BlackJack! Everyone loses!");
                        foreach (KeyValuePair<Player, int> entry in Bets)
                        {
                            Dealer.Balance += entry.Value; // Adds entered bet values from everyone and gives it to the dealers balance.
                        }
                        return;
                    }
                }
            }
            // Goes through each player in the list, and ask hit or stay, until they say "stay".
            foreach (Player player in Players)
            {
                while (!player.Stay)// While player "is not" staying
                {
                    Console.WriteLine("\nYour cards are: ");
                    foreach (Card card in player.Hand)
                    {
                        Console.Write("{0} ",card.ToString());
                    }
                    Console.WriteLine("\n \nHit or stay?\n");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "stay")
                    {
                        player.Stay = true;// Ends "while" loop.
                        break;
                    }
                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                    }
                    // This portion will check to see whether or not player has "busted".
                    bool busted = TwentyOneRules.IsBusted(player.Hand); // Custom method to check "busted" status.
                    if (busted) // Boolean value, if true the code below is fired off!
                    {
                        Dealer.Balance += Bets[player]; // Adds players money to the dealers balance.
                        Console.WriteLine("\n\n{0} Busted! You lose your bet of {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance);
                        Console.WriteLine("\nDo you want to play again?");
                        answer = Console.ReadLine().ToLower();
                        if (answer == "yes" || answer == "yeah") // Just in case player were to answer with "yeah", could be implemented above also.
                        {
                            player.isActivelyPlaying = true;
                            return; // Ends void function, must have it or it wont end program, same thing on the below.
                        }
                        else
                        {
                            player.isActivelyPlaying = false; // In the "main" program it will only operate with a "true" value for "isActivelyPlaying".
                            return;                          //Extra side note, I think it would be advantagous to add a more defined option here, as a player would be upset if they 
                                                            //accidently terminated their game due to an invalid response.
                        }
                    }
                }
            }
            Dealer.isBusted = TwentyOneRules.IsBusted(Dealer.Hand);
            Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand); // Sends dealers hand to "rules" class to verify to continue. Returns a bool value.
            while (!Dealer.Stay && !Dealer.isBusted)// As long as dealer is not busted or not staying the below code will fire.
            {
                Console.WriteLine("\nDealer is hitting...");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = TwentyOneRules.IsBusted(Dealer.Hand);
                Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
            }
            if (Dealer.Stay)
            {
                Console.WriteLine("\nDealer is staying.");
            }
            if (Dealer.isBusted)
            {
                Console.WriteLine("\nDealer Busted!");
                foreach (KeyValuePair<Player, int> entry in Bets)// Pays out users winnings due to dealer bust. For every pair it will print to console.
                {
                    Console.WriteLine("\n{0} won {1}!", entry.Key.Name, entry.Value); // Accessess "bets" table by using the "entry" keyword.
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2); // Lambda expression... For some reason threw an error at first, rewritten exactly the same & fine.
                                                                                                       // Loops through each key value pair, "where" produces a list, and pays out winner.
                    Dealer.Balance -= entry.Value;
                }
                return;
            }
            foreach (Player player in Players)
            {
                bool? playerWon = TwentyOneRules.CompareHand(player.Hand, Dealer.Hand); //Creates a nullable boolean value, allowing 3 assignable values.
                if (playerWon == null)
                {
                    Console.WriteLine("\nPush! No one wins.");
                    player.Balance += Bets[player];
                }
                else if (playerWon == true)
                {
                    Console.WriteLine("\n{0} won {1}!", player.Name, Bets[player]);
                    player.Balance += (Bets[player] * 2);
                    player.Balance -= Bets[player];
                }
                else
                {
                    Console.WriteLine("\n\nDealer wins {0}!", Bets[player]);
                    Dealer.Balance += Bets[player];
                }
                Console.WriteLine("\n\nPlay again?");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "yeah")
                {
                    player.isActivelyPlaying = true;
                }
                else
                {
                    player.isActivelyPlaying = false;
                }

            }


        }
        public override void ListPlayers()
        {
            Console.WriteLine("\n\n21 Players:");
            base.ListPlayers();
        }

        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
