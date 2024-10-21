using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeSharedModels
{
    public interface IClock
    {
        public DateTime GetCurrentTime();
    }
}
