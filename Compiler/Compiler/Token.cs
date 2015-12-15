namespace Compiler
{
    public class Token
    {
        public string Type { get; set; }
        public string Data { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

        public Token()
        {
            Data = "";
            Type = "";
            LineNumber = 0;
            ColumnNumber = 0;
        }

        public Token(string type, string data)
        {
            Type = type;
            Data = data;
            LineNumber = -1;
            ColumnNumber = -1;
        }

        public Token(string data, int lineNumber, int columnNumber)
        {
            Data = data;
            Type = "";
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public Token(string data, int lineNumber, int columnNumber, string type)
        {
            Data = data;
            Type = type;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

    }
}
