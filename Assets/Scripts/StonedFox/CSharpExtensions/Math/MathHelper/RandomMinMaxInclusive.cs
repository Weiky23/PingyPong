using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathStuff
{
    public class RandomMinMaxInclusive
    {
        int min;
        int max;
        List<int> previousGenerated;

        public RandomMinMaxInclusive(int min, int max)
        {
            this.min = min;
            this.max = max;
            previousGenerated = new List<int>(max - min + 1);
        }

        public bool RollNewRandom(out int randomNumber)
        {
            if (max - min < previousGenerated.Count)
            {
                randomNumber = -1;
                return false;
            }
            for (;;)
            {
                randomNumber = UnityEngine.Random.Range(min, max + 1);
                if (!previousGenerated.Contains(randomNumber))
                {
                    previousGenerated.Add(randomNumber);
                    return true;
                }
            }
        }
    }
}
