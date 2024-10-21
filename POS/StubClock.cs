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
            return new DateTime(2200, 5, 1);
        }
    }
}
