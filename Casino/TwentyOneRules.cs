using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.TwentyOne
{// Basic Helper Methods
    public class TwentyOneRules
    {
        // Private due to it only being used inside this class. Static to avoid having to create a "TwentyOneRules" object everywhere.
        // Naming convention for privates are to use the underscore first!
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {// Abstantiate with all of our objects.
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1 // Due to one value able to be set at once, we will add the logic to deteremine if the player would rather it count as 1 or 11....
        };

        // This is designed to determine the value of the users hand, including whether or not "bust" or "blackjack" has been achieved.
        // int[] is an integer array.
        private static int[] GetAllPossibleHandValues(List<Card> Hand)
        {// Lambda expression used to check list!
            int aceCount = Hand.Count(x => x.Face == Face.Ace);
            // Result
            int[] result = new int[aceCount + 1];
            // Takes each item and looks it up in the card value table (dictionary) and sums it!
            int value = Hand.Sum(x => _cardValues[x.Face]);
            // Takes first entry and assigns a value.
            result[0] = value;
            if (result.Length == 1) return result; // is the same as below comment! Just more compact!
            //{
            //return result;
            //}
            for (int i = 1; i < result.Length; i++)
            {// Figures the value of Aces
                value += (i * 10); // same as "value = value + (i * 10);"
                result[i] = value;
            }
            return result;
        }

        public static bool CheckForBlackJack(List<Card> Hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(Hand);
            int value = possibleValues.Max();
            if (value == 21) return true;
            else return false;
        }

        public static bool IsBusted(List<Card> Hand)
        {
            int value = GetAllPossibleHandValues(Hand).Min();
            if (value > 21) return true;
            else return false;
        }

        // Performs basic logic to determine whether dealer should remain in the game, as the name implies.
        public static bool ShouldDealerStay(List<Card> Hand)
        {
            int[] possibleHandValues = GetAllPossibleHandValues(Hand);
            foreach (int value in possibleHandValues)
            {
                if (value > 16 && value < 22)
                {
                    return true;
                }
            }
            return false;
        }
        // Use your noggin, guess what this one does..... However it is special due to its ability to be a 3 possible value nullable boolean.
        public static bool? CompareHand(List<Card> PlayerHand, List<Card> DealerHand)
        {
            int[] PlayerResults = GetAllPossibleHandValues(PlayerHand);
            int[] DealerResults = GetAllPossibleHandValues(DealerHand);

            int playerScore = PlayerResults.Where(x => x < 22).Max(); // Takes items from player results and filters them for less than 22, and then what is the largest of those...
            int dealerScore = DealerResults.Where(x => x < 22).Max(); // Same, but different. 

            if (playerScore > dealerScore) return true;
            else if (playerScore < dealerScore) return false;
            else return null; 

        }
    }
}
