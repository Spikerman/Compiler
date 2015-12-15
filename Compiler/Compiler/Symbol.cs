namespace Compiler
{
    public class Symbol
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Data1 { get; set; }
        public float Data2 { get; set; }
        public Symbol Next { get; set; }

        public int Hang { get; set; }
        public int Lie { get; set; }

        public Symbol()
        {
            Data1 = 0;
            Data2 = 0F;
            Next = null;
            Hang = -1;
            Lie = -1;
        }

        public Symbol(string i, int a, int b, int c)
        {
            Name = i;
            Type = "int";
            Data1 = a;
            Data2 = 0F;
            Next = null;

            Hang = b;
            Lie = c;
        }

        public Symbol(string i, float a, int b, int c)
        {
            Name = i;
            Type = "real";
            Data1 = (int)a;
            Data2 = b;
            Next = null;

            Hang = b;
            Lie = c;
        }

        public Symbol(string i, int a, int b)
        {
            Name = i;
            Hang = a;
            Lie = b;
            Next = null;
        }
    }
}