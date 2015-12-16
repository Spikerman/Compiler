using System;
using System.Collections.Generic;

namespace Compiler
{
    public static class Semantic
    {
        private static string _outputString = string.Empty;

        private static void Output(string content)
        {
            _outputString += content;
        }

        public static int getint(string Num)
        {
            int l = Num.Length;
            char ch;
            int counter = 1;
            int sum = 0;
            for (int i = l - 1; i >= 0; i--)
            {
                ch = Num[i];
                sum = sum + (((int)ch - 48)) * counter;
                counter = counter * 10;
            }
            return sum;
        }

        public static void semantic_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            if (semanticList.Count > 0)
            {
                if (semanticList.First.Value.Data == "program")
                {
                    semanticList.RemoveFirst();
                    program_go(semanticList, symbol_table);
                }
                else
                {
                    Output("ERROR!!!NO Program!!!");
                    Output("\n");
                }
                if (semanticList.Count == 0)
                {
                    Output("NOTICE!!!:�����������!!!");
                    Output("\n");
                }
            }
            else
            {
                Output("WARNING!!!--�������﷨����--!!!");
                Output("\n");
                Output("NOTICE!!!�﷨��������Ϊ:-ll !!!");
                Output("\n");
            }
        }

        private static void program_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            if (semanticList.First.Value.Data == "compoundstmt")
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, symbol_table);
            }
            else
            {
                Output("ERROR!!!NO compoundstmt!!!");
                Output("\n");
            }
        }

        private static void comp_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            if (semanticList.First.Value.Data == "{")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("comp_go ERROR!!! NO { !!!");
                Output("\n");
            }
            if (semanticList.First.Value.Data == "stmts")
            {
                semanticList.RemoveFirst();
                stmts_go(semanticList, symbol_table);
            }
            else
            {
                Output("comp_go ERROR!!! NO Stmts !!!");
                Output("\n");
            }
            if (semanticList.First.Value.Data == "}")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("comp_go ERROR!!! NO } !!!");
                Output("\n");
            }
        }

        private static void stmts_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, symbol_table);
                if (semanticList.First.Value.Data == "stmts")
                {
                    semanticList.RemoveFirst();
                    stmts_go(semanticList, symbol_table);
                }
                else
                {
                    Output("stmts_go ERROR!!! NO stmts!!!");
                    Output("\n");
                }
            }
            else if (semanticList.First.Value.Data == "stmts:ε")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("stmts_go ERROR!!! �󷽴��� !!!");
                Output("\n");
            }
        }

        private static void stmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            LinkedList<Token> temp_list = new LinkedList<Token>();
            bool k = false;
            if (semanticList.First.Value.Data == "ifstmt")
            {
                semanticList.RemoveFirst();
                ifstmt_go(semanticList, symbol_table);
            }
            else if (semanticList.First.Value.Data == "whilestmt")
            {
                semanticList.RemoveFirst();
                temp_list = new LinkedList<Token>(semanticList);
                k = whilestmt_go(temp_list, symbol_table);

                while (k)
                {
                    temp_list = new LinkedList<Token>(semanticList);
                    k = whilestmt_go(temp_list, symbol_table);
                }
                if (!k)
                {
                    whilestmt_go(semanticList, symbol_table);
                }
            }
            else if (semanticList.First.Value.Data == "assgstmt")
            {
                semanticList.RemoveFirst();
                assgstmt_go(semanticList, symbol_table);
            }
            else if (semanticList.First.Value.Data == "compoundstmt")
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, symbol_table);
            }
            else
            {
                Output("stmt_go ERROR!!! 输入错误");
                Output("\n");
            }
        }

        private static void ifstmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            bool flag = true;
            bool key = false;
            if (semanticList.First.Value.Data == "if")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO IF !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "(")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO ( !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "boolexpr")
            {
                semanticList.RemoveFirst();
                key = boolexpr_go(semanticList, symbol_table);
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO Boolexpr !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == ")")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO ) !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "then")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO then !!!");
                Output("\n");
            }

            if (key == false)
            {
                if (semanticList.First.Value.Data == "stmt")
                {
                    semanticList.RemoveFirst();
                    pop_stmt(semanticList);
                }
                else
                {
                    Output("ifstmt_go ERROR!!!NO stmt!!!");
                    Output("\n");
                }

                if (semanticList.First.Value.Data == "else")
                {
                    semanticList.RemoveFirst();
                }
                else
                {
                    Output("ifstmt_go ERROR!!! NO else !!!");
                    Output("\n");
                }

                if (semanticList.First.Value.Data == "stmt")
                {
                    semanticList.RemoveFirst();
                    stmt_go(semanticList, symbol_table);
                }
                else
                {
                    Output("ifstmt_go else ERROR !!! NO stmt!!!");
                    Output("\n");
                }
            }
            else if (key == true)
            {
                if (semanticList.First.Value.Data == "stmt")
                {
                    semanticList.RemoveFirst();
                    stmt_go(semanticList, symbol_table);
                }
                else
                {
                    Output("ifstmt_go ERROR!!! NO stmt!!!");
                    Output("\n");
                }
                if (semanticList.First.Value.Data == "else")
                {
                    semanticList.RemoveFirst();
                }
                else
                {
                    Output("ifstmt_go ERROR!!! NO else");
                    Output("\n");
                }
                if (semanticList.First.Value.Data == "stmt")
                {
                    semanticList.RemoveFirst();
                    pop_stmt(semanticList);
                }
                else
                {
                    Output("ifstmt else ERROR!!! NO stmt !!!");
                    Output("\n");
                }
            }
        }

        private static void assgstmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            string id = string.Empty;
            if (semanticList.First.Value.Type == "ID")
            {
                id = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt_go ERROR!!! NO ID !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "=")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt ERROR!!! NO = !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "arithexpr")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt ERROR!!! NO calexpr !!!");
                Output("\n");
            }

            SymbolTable.write_data(id, calexpr_go(semanticList, symbol_table), symbol_table);

            if (semanticList.First.Value.Data == ";")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt_go ERROR!!! NO ;!!!");
                Output("\n");
            }
        }

        private static bool whilestmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            bool k = false;
            if (semanticList.First.Value.Data == "while")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO while!!!");
                Output("\n");
            }
            if (semanticList.First.Value.Data == "(")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO (!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "boolexpr")
            {
                semanticList.RemoveFirst();
                k = boolexpr_go(semanticList, symbol_table);
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO boolexpr!!!");
                Output("\n");
            }
            if (semanticList.First.Value.Data == ")")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO )");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                if (k == true)
                {
                    stmt_go(semanticList, symbol_table);
                }
                else
                {
                    pop_stmt(semanticList);
                }
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO stmt !!!");
                Output("\n");
            }

            return k;
        }

        private static bool boolexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            int a = 0;
            int b = 0;
            string cal = string.Empty;
            if (semanticList.First.Value.Data == "arithexpr")
            {
                semanticList.RemoveFirst();
                a = calexpr_go(semanticList, symbol_table);
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO arithexpr!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "boolop")
            {
                semanticList.RemoveFirst();
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO boolop!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "arithexpr")
            {
                semanticList.RemoveFirst();
                b = calexpr_go(semanticList, symbol_table);
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO arithexpr!!!");
                Output("\n");
            }

            if (cal == "<")
            {
                return (a < b);
            }
            else if (cal == ">")
            {
                return (a > b);
            }
            else if (cal == "<=")
            {
                return (a <= b);
            }
            else if (cal == ">=")
            {
                return (a >= b);
            }
            else if (cal == "==")
            {
                return (a == b);
            }
            else
            {
                Output("boolexpr_go ERROR !!! boolop ERROR !!!");
                Output("\n");
                return false;
            }
        }

        private static int calexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            int a = 0;
            if (semanticList.First.Value.Data == "multexpr")
            {
                semanticList.RemoveFirst();
                a = multexpr_go(semanticList, symbol_table);
            }
            else
            {
                Output("calexpr_ERROR!!! NO multexpr!!!");
                Output("\n");
            }
            if (semanticList.First.Value.Data == "arithexprprime")
            {
                semanticList.RemoveFirst();
                a = a + calexprim_go(semanticList, symbol_table);
            }
            else
            {
                Output("calexpr_go ERROR!!! NO arithexprprime!!!");
                Output("\n");
            }

            return a;
        }

        private static int calexprim_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            int a = 0;
            if (semanticList.First.Value.Data == "+")
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "multexpr")
                {
                    semanticList.RemoveFirst();
                    a = a + multexpr_go(semanticList, symbol_table);
                }
                else
                {
                    Output("calexprim ERROR!!!NO multexpr");
                    Output("\n");
                }
                if (semanticList.First.Value.Data == "arithexprprime")
                {
                    semanticList.RemoveFirst();
                    a = a + calexprim_go(semanticList, symbol_table);
                }
                else
                {
                    Output("calexprim_go ERROR!!! NO arithexprprime!!!");
                    Output("\n");
                }
            }
            else if (semanticList.First.Value.Data == "-")
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "multexpr")
                {
                    semanticList.RemoveFirst();
                    a = a - multexpr_go(semanticList, symbol_table);
                }
                else
                {
                    Output("calexprim ERROR!!!NO multexpr");
                    Output("\n");
                }

                if (semanticList.First.Value.Data == "arithexprprime")
                {
                    semanticList.RemoveFirst();
                    a = a + calexprim_go(semanticList, symbol_table);
                }
                else
                {
                    Output("calexprim_go ERROR!!! NO arithexprprime!!!");
                    Output("\n");
                }
            }
            else if (semanticList.First.Value.Data == "arithexprprime:ε")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("calexprim_go ERROR!!! NO calop!!!");
                Output("\n");
            }

            return a;
        }

        private static int multexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            string cal = string.Empty;
            int b = 0;
            int a = 0;
            if (semanticList.First.Value.Data == "simpleexpr")
            {
                semanticList.RemoveFirst();
                a = simexpr_go(semanticList, symbol_table);
            }
            else
            {
                Output("multexpr ERROR!!!NO simpleexpr !!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "multexprprime")
            {
                semanticList.RemoveFirst();
                b = multexprim_go(semanticList, symbol_table, ref cal);
            }
            else
            {
                Output("multexpr_go ERROR!!! NO multexprprime !!!");
                Output("\n");
            }

            if (cal == "*")
            {
                a = a * b;
            }
            else if (cal == "/")
            {
                a = a / b;
            }
            else if (cal == "")
            {
                a = a;
            }
            else
            {
                Output("multexpr_go ERROR!!! calop lack !!!");
                Output("\n");
            }
            return a;
        }

        private static int multexprim_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table, ref string cal)
        {
            int a = 0;
            int b = 0;
            string m = string.Empty;
            if (semanticList.First.Value.Data == "*")
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "simpleexpr")
                {
                    semanticList.RemoveFirst();
                    a = simexpr_go(semanticList, symbol_table);
                    if (semanticList.First.Value.Data == "multexprprime")
                    {
                        semanticList.RemoveFirst();
                        b = multexprim_go(semanticList, symbol_table, ref m);
                        if (m == "*")
                        {
                            a = a * b;
                        }
                        else if (m == "/")
                        {
                            a = a / b;
                        }
                        else if (m == "")
                        {
                            a = a;
                        }
                        else
                        {
                            Output("multexprim_go ERROR!!! boolop lack !!!");
                            Output("\n");
                        }
                    }
                    else
                    {
                        Output("multexprim_go ERROR!!! NO multexprprime !!!");
                        Output("\n");
                    }
                }
                else
                {
                    Output("multexprim_go ERROR!!! NO simpleexpr !!!");
                    Output("\n");
                }
            }
            else if (semanticList.First.Value.Data == "/")
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "simpleexpr")
                {
                    semanticList.RemoveFirst();
                    a = simexpr_go(semanticList, symbol_table);
                    if (semanticList.First.Value.Data == "multexprprime")
                    {
                        semanticList.RemoveFirst();
                        b = multexprim_go(semanticList, symbol_table, ref m);
                        if (m == "*")
                        {
                            a = a * b;
                        }
                        else if (m == "/")
                        {
                            a = a / b;
                        }
                        else if (m == "")
                        {
                            a = a;
                        }
                        else
                        {
                            Output("multexprim_go ERROR!!! boolop lack !!!");
                            Output("\n");
                        }
                    }
                    else
                    {
                        Output("multexprim_go ERROR!!! NO multexprprime !!!");
                        Output("\n");
                    }
                }
                else
                {
                    Output("multexprim_go ERROR!!! NO simpleexpr !!!");
                    Output("\n");
                }
            }
            else if (semanticList.First.Value.Data == "multexprprime:ε")
            {
                semanticList.RemoveFirst();
                cal = "";
            }
            else
            {
                Output("multexprim_go ERROR!!!NO calop !!!");
                Output("\n");
            }

            return a;
        }

        private static int simexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbol_table)
        {
            int a = 0;
            if (semanticList.First.Value.Type == "ID")
            {
                string id = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: a=get_data(id,symbol_table);
                a = SymbolTable.get_data(id, symbol_table);
            }
            else if (semanticList.First.Value.Type == "integer" || semanticList.First.Value.Type == "real_number")
            {
                a = getint(semanticList.First.Value.Data);
                semanticList.RemoveFirst();
            }
            else if (semanticList.First.Value.Data == "(")
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "arithexpr")
                {
                    semanticList.RemoveFirst();
                    a = calexpr_go(semanticList, symbol_table);
                    if (semanticList.First.Value.Data == ")")
                    {
                        semanticList.RemoveFirst();
                    }
                    else
                    {
                        Output("simexpr_go ERROR!!!NO ) !!!");
                        Output("\n");
                    }
                }
                else
                {
                    Output("simexpr_go ERROR !!! NO arithexpr !!!");
                    Output("\n");
                }
            }
            else
            {
                Output("simexpr_go ERROR!!! 输入错误 !!!");
                Output("\n");
            }

            return a;

        }

        private static void pop_stmt(LinkedList<Token> semanticList)
        {
            if (semanticList.First.Value.Data == "ifstmt")
            {
                semanticList.RemoveFirst();
                pop_ifstmt(semanticList);
            }
            else if (semanticList.First.Value.Data == "whilestmt")
            {
                semanticList.RemoveFirst();
                pop_whilestmt(semanticList);
            }
            else if (semanticList.First.Value.Data == "assgstmt")
            {
                semanticList.RemoveFirst();
                pop_assgstmt(semanticList);
            }
            else if (semanticList.First.Value.Data == "compoundstmt")
            {
                semanticList.RemoveFirst();
                pop_comp(semanticList);
            }
            else
            {
                Output("pop_stmt ERROR !!! �������!!!");
                Output("\n");
            }
        }

        private static void pop_assgstmt(LinkedList<Token> semanticList)
        {
            if (semanticList.First.Value.Type != "ID")
            {
                Output("pop_assgstmt ERROR NO ID!!!");
                Output("\n");
            }
            while (semanticList.First.Value.Data != ";")
            {
                semanticList.RemoveFirst();
            }
            if (semanticList.First.Value.Data == ";")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_assgstmt ERRER !!! NO ; !!!");
                Output("\n");
            }
        }

        private static void pop_ifstmt(LinkedList<Token> semanticList)
        {
            while (semanticList.First.Value.Data != "stmt")
            {
                semanticList.RemoveFirst();
            }
            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                pop_stmt(semanticList);
            }
            else
            {
                Output("pop_ifstmt ERROR !!! NO stmt!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "else")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_ifstmt ERROR !!! NO else!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                pop_stmt(semanticList);
            }
            else
            {
                Output("pop_ifstmt ERROR !!! NO stmt!!!");
                Output("\n");
            }
        }

        private static void pop_whilestmt(LinkedList<Token> semanticList)
        {
            while (semanticList.First.Value.Data != "stmt")
            {
                semanticList.RemoveFirst();
            }
            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                pop_stmt(semanticList);
            }
            else
            {
                Output("pop_whilestmt ERROR!!!NO stmt!!!");
                Output("\n");
            }
        }

        private static void pop_comp(LinkedList<Token> semanticList)
        {
            if (semanticList.First.Value.Data == "{")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_comp ERROR!!!NO {!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "stmts")
            {
                semanticList.RemoveFirst();
                pop_stmts(semanticList);
            }
            else
            {
                Output("pop_comp ERROR!!!NO stmts!!!");
                Output("\n");
            }

            if (semanticList.First.Value.Data == "}")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_comp ERROR!!!NO }!!!");
                Output("\n");
            }
        }

        private static void pop_stmts(LinkedList<Token> semanticList)
        {
            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                pop_stmt(semanticList);
                if (semanticList.First.Value.Data == "stmts")
                {
                    semanticList.RemoveFirst();
                    pop_stmts(semanticList);
                }
                else
                {
                    Output("pop_stmts ERROR!!!NO stmts!!!");
                    Output("\n");
                }
            }
            else if (semanticList.First.Value.Data == "stmts:ε")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_stmts ERROR!!! NO End!!!");
                Output("\n");
            }
        }

        private static void print_semantic_list(LinkedList<Token> semanticList)
        {
            while (semanticList.Count > 0)
            {
                Output(semanticList.First.Value.Data);
                Output("\n");
                semanticList.RemoveFirst();
            }
        }
    }
}

