using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{
    public class Dealer
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }
        public int Balance { get; set; }


        // Card dealing method.
        public void Deal(List<Card> Hand)
        {
            // Writes what we are dealing.
            // Adding card to hand that is passed in from "Deck".
            Hand.Add(Deck.Cards.First());
            Console.WriteLine(Deck.Cards.First().ToString() + "/n"); // The "/n" adds a new line to the end.
            Deck.Cards.RemoveAt(0);
        }
    }

}
