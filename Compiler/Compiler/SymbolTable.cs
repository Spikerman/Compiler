using System;
using System.Collections.Generic;
using System.Globalization;

namespace Compiler
{
    public static class SymbolTable
    {
        private static string _outputString = string.Empty;

        private static void Output(string content)
        {
            _outputString += content;
        }

        public static void print_symbol_table(LinkedList<Symbol> symbolTable)
        {
            for (int s = 0; s < symbolTable.Count; s++)
            {
                Symbol tmp = symbolTable.First.Value;
                symbolTable.RemoveFirst();
                symbolTable.AddLast(tmp);

                Output("[");
                Output(tmp.LineNumber.ToString());
                Output(",");
                Output(tmp.ColumnNumber.ToString());
                Output("] ");
                Output(tmp.Name);
                Output(" : ");

                Symbol p = tmp;

                while (p != null)
                {
                    if (p.Type == "int")
                    {
                        Output(p.Type);
                        Output(" ");
                        Output(p.Data1.ToString());
                        Output(" ");
                    }
                    else if (p.Type == "real")
                    {
                        Output(p.Type);
                        Output(" ");
                        Output(p.Data2.ToString(CultureInfo.InvariantCulture));
                        Output(" ");
                    }
                    else if (p.Type == "")
                    {
                        Output("NULL ");
                    }
                    else
                    {
                        Output("Error ");
                    }
                    p = p.Next;
                }
                Output(Environment.NewLine);
            }

            Output("SymbolTable END");
            Output(Environment.NewLine);
        }

        public static void init_symbol_table(Symbol symbol, LinkedList<Symbol> symbolTable)
        {
            int i = symbolTable.Count;
            bool flag = true;
            for (int s = 0; s < i; s++)
            {
                Symbol temp = symbolTable.First.Value;
                string name1 = temp.Name;
                Symbol p = temp;
                if (symbol.Name == name1)
                {
                    while (p.Next != null)
                    {
                        p = p.Next;
                    }
                    p.Next = symbol;
                    flag = false;
                }
                symbolTable.RemoveFirst();
                symbolTable.AddLast(temp);
            }
            if (flag)
            {
                symbolTable.AddLast(symbol);
            }
        }

        public static bool write_data(string id, int num, LinkedList<Symbol> symbolTable)
        {
            int i = symbolTable.Count;
            bool flag = false;
            for (int s = 0; s < i; s++)
            {
                Symbol temp = symbolTable.First.Value;
                if (temp.Name == id)
                {
                    temp.Type = "int";
                    temp.Data1 = num;
                    flag = true;
                }
                symbolTable.RemoveFirst();
                symbolTable.AddLast(temp);
            }
            return flag;
        }

        public static int get_data(string id, LinkedList<Symbol> symbolTable)
        {
            int i = symbolTable.Count;
            int num = 0;
            for (int s = 0; s < i; s++)
            {
                Symbol temp = symbolTable.First.Value;
                if (temp.Name == id)
                {
                    temp.Type = "int";
                    num = temp.Data1;
                }
                symbolTable.RemoveFirst();
                symbolTable.AddLast(temp);
            }
            return num;
        }
    }
}