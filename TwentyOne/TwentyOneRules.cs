using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne
{// Basic Helper Methods
    public class TwentyOneRules
    {
        // Private due to it only being used inside this class. Static to avoid having to create a "TwentyOneRules" object everywhere.
        // Naming convention for privates are to use the underscore first!
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {// Abstantiate with all of our objects.

        };
    }
}
