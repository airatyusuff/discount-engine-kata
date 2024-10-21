using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeSharedModels
{
    public class StubClock : IClock
    {
        public DateTime GetCurrentTime()
        {
            return new DateTime(2200, 5, 1); // still need to update for HH MM SS
        }

        public DateTime GetToday()
        {
            return new DateTime(2200, 5, 1);
        }
    }
}
