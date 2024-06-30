using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTSSDK
{
    public class OTSException : Exception
    {
        public OTSException() { }
        public OTSException(string message) : base(message) { }
    }
}
