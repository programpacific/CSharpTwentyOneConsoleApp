using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            string card = string.Format("\nxxxxxxxxxxxxxxxxxxxxxxxx\n" + Deck.Cards.First().ToString() + "\nxxxxxxxxxxxxxxxxxxxxxxxx\n");// The "/n" adds a new line to the end.
            Console.WriteLine(card);
            using (StreamWriter file = new StreamWriter(@"C:\Users\New\Desktop\TwentyOneLog.txt", true)) // The "using" keyword helps dispose of data at the end of operation to reduce memory consumption.
            {
                file.WriteLine(DateTime.Now);
                file.WriteLine(card);
            }
            Deck.Cards.RemoveAt(0);
        }
    }

}
