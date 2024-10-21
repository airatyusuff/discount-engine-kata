using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeSharedModels
{
    public class SystemClock : IClock
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
