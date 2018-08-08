using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class FraudException : Exception
    {
        public FraudException() // Makes new "exception", :base****** pulls from the inherit from "Exception" up top.
            : base() { }
        public FraudException(string message) // Overloaded constructor, more on exception handling pt2, step 163...
            : base(message) { }
    }
}
