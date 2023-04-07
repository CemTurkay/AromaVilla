using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Expections
{
    public class EmptyBasketExpection : Exception
    {
        public EmptyBasketExpection() : base("Basket cannot be empty.")
        {

        }
    }
}
