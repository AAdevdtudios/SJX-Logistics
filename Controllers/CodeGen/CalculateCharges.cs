using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers.CodeGen
{
    public class CalculateCharges
    {

         public int CalculateCharge(double distance)
        {
            int basePrice = 500;//1000
            if (distance == 0)
            {
                return 0;
            }
            int value = 0;

            if (distance > 25)
                value = basePrice + 6 * basePrice;
            else if (distance > 20)
                value = basePrice + 6 * basePrice;
            else if (distance > 15)
                value = basePrice + 4 * basePrice;
            else if (distance > 10)
                value = basePrice + 2 * basePrice;
            else if (distance > 8)
                value = (int)(basePrice + 1.5 * basePrice);
            else if (distance > 6)
                value = (int)(basePrice + 0.75 * basePrice);
            else if (distance > 0)
                value = (int)(basePrice + 0.5 * basePrice);

            return value;
        }
    }
}
