using System;
using System.Collections.Generic;

namespace Compiler
{
    public static class Semantic
    {
        public static string ErrorDisplay { get; } = "{0} ERROR: NO {1}.";
        private static string _outputString = string.Empty;

        private static void Output(string content)
        {
            _outputString += content;
        }

        public static string semantic_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            if (semanticList.Count > 0)
            {
                if (semanticList.First.Value.Data == ConstString.Program)
                {
                    semanticList.RemoveFirst();
                    program_go(semanticList, symbolTable);
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(semantic_go), ConstString.Program));
                    Output(Environment.NewLine);
                }
                if (semanticList.Count == 0)
                {
                    Output("NOTICE!!!:语义分析结束!!!");
                    Output(Environment.NewLine);
                }
            }
            else
            {
                Output("WARNING!!!--请先做语法分析--!!!");
                Output(Environment.NewLine);
                Output("NOTICE!!!语法分析命令:-ll !!!");
                Output(Environment.NewLine);
            }
            return _outputString;
        }

        private static void program_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            if (semanticList.First.Value.Data == ConstString.Compoundstmt)
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(program_go), ConstString.Compoundstmt));
                Output(Environment.NewLine);
            }
        }

        private static void comp_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            if (semanticList.First.Value.Data == ConstString.左大括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(comp_go), ConstString.左大括号));
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.Stmts)
            {
                semanticList.RemoveFirst();
                stmts_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(comp_go), ConstString.Stmts));
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.右大括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(comp_go), ConstString.右大括号));
                Output(Environment.NewLine);
            }
        }

        private static void stmts_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            if (semanticList.First.Value.Data == ConstString.Stmt)
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, symbolTable);
                if (semanticList.First.Value.Data == ConstString.Stmts)
                {
                    semanticList.RemoveFirst();
                    stmts_go(semanticList, symbolTable);
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(stmts_go), ConstString.Stmts));
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == ConstString.Stmts空)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("stmts_go ERROR!!! 后方错误 !!!");
                Output(Environment.NewLine);
            }
        }

        private static void stmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            LinkedList<Token> temp_list = new LinkedList<Token>();
            if (semanticList.First.Value.Data == ConstString.Ifstmt)
            {
                semanticList.RemoveFirst();
                ifstmt_go(semanticList, symbolTable);
            }
            else if (semanticList.First.Value.Data == ConstString.Whilestmt)
            {
                semanticList.RemoveFirst();
                temp_list = new LinkedList<Token>(semanticList);
                bool k = whilestmt_go(temp_list, symbolTable);

                while (k)
                {
                    temp_list = new LinkedList<Token>(semanticList);
                    k = whilestmt_go(temp_list, symbolTable);
                }
                if (!k)
                {
                    whilestmt_go(semanticList, symbolTable);
                }
            }
            else if (semanticList.First.Value.Data == ConstString.Assgstmt)
            {
                semanticList.RemoveFirst();
                assgstmt_go(semanticList, symbolTable);
            }
            else if (semanticList.First.Value.Data == ConstString.Compoundstmt)
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, symbolTable);
            }
            else
            {
                Output("stmt_go ERROR!!! 输入错误");
                Output(Environment.NewLine);
            }
        }

        private static void ifstmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            bool key = false;
            if (semanticList.First.Value.Data == ConstString.If)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.If));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.左括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.左括号));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Boolexpr)
            {
                semanticList.RemoveFirst();
                key = boolexpr_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.Boolexpr));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.右括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.右括号));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Then)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.Then));
                Output(Environment.NewLine);
            }

            if (key == false)
            {
                if (semanticList.First.Value.Data == ConstString.Stmt)
                {
                    semanticList.RemoveFirst();
                    pop_stmt(semanticList);
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.Stmt));
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == ConstString.Else)
                {
                    semanticList.RemoveFirst();
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.Else));
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == ConstString.Stmt)
                {
                    semanticList.RemoveFirst();
                    stmt_go(semanticList, symbolTable);
                }
                else
                {
                    Output("ifstmt_go else ERROR !!! NO stmt!!!");
                    Output(Environment.NewLine);
                }
            }
            else
            {
                if (semanticList.First.Value.Data == ConstString.Stmt)
                {
                    semanticList.RemoveFirst();
                    stmt_go(semanticList, symbolTable);
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.Stmt));
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == ConstString.Else)
                {
                    semanticList.RemoveFirst();
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(ifstmt_go), ConstString.Else));
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == ConstString.Stmt)
                {
                    semanticList.RemoveFirst();
                    pop_stmt(semanticList);
                }
                else
                {
                    Output("ifstmt else ERROR!!! NO stmt !!!");
                    Output(Environment.NewLine);
                }
            }
        }

        private static void assgstmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            string id = string.Empty;
            if (semanticList.First.Value.Type == ConstString.Id)
            {
                id = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(assgstmt_go), ConstString.Id));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.等号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt ERROR!!! NO = !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Arithexpr)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt ERROR!!! NO calexpr !!!");
                Output(Environment.NewLine);
            }

            SymbolTable.write_data(id, calexpr_go(semanticList, symbolTable), symbolTable);

            if (semanticList.First.Value.Data == ConstString.分号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(assgstmt_go), ConstString.分号));
                Output(Environment.NewLine);
            }
        }

        private static bool whilestmt_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            bool k = false;
            if (semanticList.First.Value.Data == ConstString.While)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(whilestmt_go), ConstString.While));
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.左括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(whilestmt_go), ConstString.左括号));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Boolexpr)
            {
                semanticList.RemoveFirst();
                k = boolexpr_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(whilestmt_go), ConstString.Boolexpr));
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.右括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(whilestmt_go), ConstString.右括号));
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.Stmt)
            {
                semanticList.RemoveFirst();
                if (k == true)
                {
                    stmt_go(semanticList, symbolTable);
                }
                else
                {
                    pop_stmt(semanticList);
                }
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(whilestmt_go), ConstString.Stmt));
                Output(Environment.NewLine);
            }

            return k;
        }

        private static bool boolexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            int a = 0;
            int b = 0;
            string cal = string.Empty;
            if (semanticList.First.Value.Data == ConstString.Arithexpr)
            {
                semanticList.RemoveFirst();
                a = calexpr_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(boolexpr_go), ConstString.Arithexpr));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Boolop)
            {
                semanticList.RemoveFirst();
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(boolexpr_go), ConstString.Boolop));
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Arithexpr)
            {
                semanticList.RemoveFirst();
                b = calexpr_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(boolexpr_go), ConstString.Arithexpr));
                Output(Environment.NewLine);
            }

            if (cal == ConstString.小于号)
            {
                return (a < b);
            }
            else if (cal == ConstString.大于号)
            {
                return (a > b);
            }
            else if (cal == ConstString.小于等于号)
            {
                return (a <= b);
            }
            else if (cal == ConstString.大于等于号)
            {
                return (a >= b);
            }
            else if (cal == ConstString.相等号)
            {
                return (a == b);
            }
            else
            {
                Output("boolexpr_go ERROR !!! boolop ERROR !!!");
                Output(Environment.NewLine);
                return false;
            }
        }

        private static int calexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            int a = 0;
            if (semanticList.First.Value.Data == ConstString.Multexpr)
            {
                semanticList.RemoveFirst();
                a = multexpr_go(semanticList, symbolTable);
            }
            else
            {
                Output("calexpr_ERROR!!! NO multexpr!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.Arithexprprime)
            {
                semanticList.RemoveFirst();
                a = a + calexprim_go(semanticList, symbolTable);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(calexpr_go), ConstString.Arithexprprime));
                Output(Environment.NewLine);
            }

            return a;
        }

        private static int calexprim_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            int a = 0;
            if (semanticList.First.Value.Data == "+")
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "multexpr")
                {
                    semanticList.RemoveFirst();
                    a = a + multexpr_go(semanticList, symbolTable);
                }
                else
                {
                    Output("calexprim ERROR!!!NO multexpr");
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == "arithexprprime")
                {
                    semanticList.RemoveFirst();
                    a = a + calexprim_go(semanticList, symbolTable);
                }
                else
                {
                    Output("calexprim_go ERROR!!! NO arithexprprime!!!");
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == "-")
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == ConstString.Multexpr)
                {
                    semanticList.RemoveFirst();
                    a = a - multexpr_go(semanticList, symbolTable);
                }
                else
                {
                    Output("calexprim ERROR!!!NO multexpr");
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == ConstString.Arithexprprime)
                {
                    semanticList.RemoveFirst();
                    a = a + calexprim_go(semanticList, symbolTable);
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(calexprim_go), ConstString.Arithexprprime));
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == "arithexprprime:ε")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("calexprim_go ERROR!!! NO calop!!!");
                Output(Environment.NewLine);
            }

            return a;
        }

        private static int multexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            string cal = string.Empty;
            int b = 0;
            int a = 0;
            if (semanticList.First.Value.Data == ConstString.Simpleexpr)
            {
                semanticList.RemoveFirst();
                a = simexpr_go(semanticList, symbolTable);
            }
            else
            {
                Output("multexpr ERROR!!!NO simpleexpr !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Multexprprime)
            {
                semanticList.RemoveFirst();
                b = multexprim_go(semanticList, symbolTable, ref cal);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(multexpr_go), ConstString.Multexprprime));
                Output(Environment.NewLine);
            }

            if (cal == "*")
            {
                a = a * b;
            }
            else if (cal == "/")
            {
                a = a / b;
            }
            else if (cal == string.Empty)
            {
                //a = a;
            }
            else
            {
                Output("multexpr_go ERROR!!! calop lack !!!");
                Output(Environment.NewLine);
            }
            return a;
        }

        private static int multexprim_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable, ref string cal)
        {
            int a = 0;
            int b;
            string m = string.Empty;
            if (semanticList.First.Value.Data == "*")
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == ConstString.Simpleexpr)
                {
                    semanticList.RemoveFirst();
                    a = simexpr_go(semanticList, symbolTable);
                    if (semanticList.First.Value.Data == ConstString.Multexprprime)
                    {
                        semanticList.RemoveFirst();
                        b = multexprim_go(semanticList, symbolTable, ref m);
                        if (m == "*")
                        {
                            a = a * b;
                        }
                        else if (m == "/")
                        {
                            a = a / b;
                        }
                        else if (m == string.Empty)
                        {
                            //a = a;
                        }
                        else
                        {
                            Output("multexprim_go ERROR!!! boolop lack !!!");
                            Output(Environment.NewLine);
                        }
                    }
                    else
                    {
                        Output("multexprim_go ERROR!!! NO multexprprime !!!");
                        Output(Environment.NewLine);
                    }
                }
                else
                {
                    Output("multexprim_go ERROR!!! NO simpleexpr !!!");
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == "/")
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "simpleexpr")
                {
                    semanticList.RemoveFirst();
                    a = simexpr_go(semanticList, symbolTable);
                    if (semanticList.First.Value.Data == "multexprprime")
                    {
                        semanticList.RemoveFirst();
                        b = multexprim_go(semanticList, symbolTable, ref m);
                        if (m == "*")
                        {
                            a = a * b;
                        }
                        else if (m == "/")
                        {
                            a = a / b;
                        }
                        else if (m == string.Empty)
                        {
                            //a = a;
                        }
                        else
                        {
                            Output("multexprim_go ERROR!!! boolop lack !!!");
                            Output(Environment.NewLine);
                        }
                    }
                    else
                    {
                        Output("multexprim_go ERROR!!! NO multexprprime !!!");
                        Output(Environment.NewLine);
                    }
                }
                else
                {
                    Output("multexprim_go ERROR!!! NO simpleexpr !!!");
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == ConstString.Multexprprime空)
            {
                semanticList.RemoveFirst();
                cal = string.Empty;
            }
            else
            {
                Output("multexprim_go ERROR!!!NO calop !!!");
                Output(Environment.NewLine);
            }

            return a;
        }

        private static int simexpr_go(LinkedList<Token> semanticList, LinkedList<Symbol> symbolTable)
        {
            int a = 0;
            if (semanticList.First.Value.Type == ConstString.Id)
            {
                string id = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                a = SymbolTable.get_data(id, symbolTable);
            }
            else if (semanticList.First.Value.Type == "integer" || semanticList.First.Value.Type == "real_number")
            {
                a = int.Parse(semanticList.First.Value.Data);
                semanticList.RemoveFirst();
            }
            else if (semanticList.First.Value.Data == ConstString.左括号)
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == ConstString.Arithexpr)
                {
                    semanticList.RemoveFirst();
                    a = calexpr_go(semanticList, symbolTable);
                    if (semanticList.First.Value.Data == ConstString.右括号)
                    {
                        semanticList.RemoveFirst();
                    }
                    else
                    {
                        Output(string.Format(ErrorDisplay, nameof(simexpr_go), ConstString.右括号));
                        Output(Environment.NewLine);
                    }
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(simexpr_go), ConstString.Arithexpr));
                    Output(Environment.NewLine);
                }
            }
            else
            {
                Output("simexpr_go ERROR!!! 输入错误 !!!");
                Output(Environment.NewLine);
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
                Output("pop_stmt ERROR !!! 输入错误!!!");
                Output(Environment.NewLine);
            }
        }

        private static void pop_assgstmt(LinkedList<Token> semanticList)
        {
            if (semanticList.First.Value.Type != "ID")
            {
                Output("pop_assgstmt ERROR NO ID!!!");
                Output(Environment.NewLine);
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
                Output(Environment.NewLine);
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
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "else")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_ifstmt ERROR !!! NO else!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                pop_stmt(semanticList);
            }
            else
            {
                Output("pop_ifstmt ERROR !!! NO stmt!!!");
                Output(Environment.NewLine);
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
                Output(Environment.NewLine);
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
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "stmts")
            {
                semanticList.RemoveFirst();
                pop_stmts(semanticList);
            }
            else
            {
                Output("pop_comp ERROR!!!NO stmts!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "}")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_comp ERROR!!!NO }!!!");
                Output(Environment.NewLine);
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
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == "stmts:ε")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("pop_stmts ERROR!!! NO End!!!");
                Output(Environment.NewLine);
            }
        }

        private static void print_semantic_list(LinkedList<Token> semanticList)
        {
            while (semanticList.Count > 0)
            {
                Output(semanticList.First.Value.Data);
                Output(Environment.NewLine);
                semanticList.RemoveFirst();
            }
        }
    }
}

