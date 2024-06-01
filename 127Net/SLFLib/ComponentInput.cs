using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLFLib
{
    public class ComponentInput : IComponentInput
    {
        int c = 0;
        public string TestValue() => $"Hello maug {++c}";
    }
}
