using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class FirstFollowSet
    {
        public HashSet<string> First { get; } = new HashSet<string>();
        public HashSet<string> Follow { get; } = new HashSet<string>();
    }
}
