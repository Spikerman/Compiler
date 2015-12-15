namespace Compiler
{
    public class Symbol
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Data1 { get; set; }
        public float Data2 { get; set; }
        public Symbol Next { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }

        public Symbol()
        {
            Name = null;
            Type = null;
            Data1 = 0;
            Data2 = 0F;
            Next = null;
            LineNumber = -1;
            ColumnNumber = -1;
        }

        public Symbol(string name, int a, int b)
        {
            Name = name;
            Type = null;
            Data1 = 0;
            Data2 = 0F;
            Next = null;
            LineNumber = a;
            ColumnNumber = b;
        }
    }
}