using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class Token
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

        public Token(string type, string data)
        {
            Type = type;
            Data = data;
            LineNumber = -1;
            ColumnNumber = -1;
        }

    }
}
