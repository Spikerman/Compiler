using System;
using System.Collections.Generic;

namespace Compiler
{
    public static class ThreeAddress
    {
        private static string _outputString = string.Empty;

        private static void Output(string content)
        {
            _outputString += content;
        }

        public static string semantic_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.Count > 0)
            {
                if (semanticList.First.Value.Data == Const.program)
                {
                    semanticList.RemoveFirst();
                    program_go(semanticList, ref counter);
                }
                else
                {
                    Output("ERROR!!!NO Program!!!");
                    Output(Environment.NewLine);
                }
                if (semanticList.Count == 0)
                {
                    Output("NOTICE!!!:三地址语言生成结束!!!");
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

        private static void program_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.First.Value.Data == Const.compoundstmt)
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, ref counter);
            }
            else
            {
                Output("ERROR!!!NO compoundstmt!!!");
                Output(Environment.NewLine);
            }
        }

        private static void comp_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.First.Value.Data == Const.左大括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("comp_go ERROR!!! NO { !!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == Const.stmts)
            {
                semanticList.RemoveFirst();
                stmts_go(semanticList, ref counter);
            }
            else
            {
                Output("comp_go ERROR!!! NO Stmts !!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == Const.右大括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("comp_go ERROR!!! NO } !!!");
                Output(Environment.NewLine);
            }
        }

        private static void stmts_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, ref counter);
                if (semanticList.First.Value.Data == Const.stmts)
                {
                    semanticList.RemoveFirst();
                    stmts_go(semanticList, ref counter);
                }
                else
                {
                    Output("stmts_go ERROR!!! NO stmts!!!");
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == "stmts:ε")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("stmts_go ERROR!!! 后方错误 !!!");
                Output(Environment.NewLine);
            }
        }

        private static void stmt_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.First.Value.Data == "ifstmt")
            {
                semanticList.RemoveFirst();
                ifstmt_go(semanticList, ref counter);
            }
            else if (semanticList.First.Value.Data == "whilestmt")
            {
                semanticList.RemoveFirst();
                whilestmt_go(semanticList, ref counter);
            }
            else if (semanticList.First.Value.Data == "assgstmt")
            {
                semanticList.RemoveFirst();
                assgstmt_go(semanticList, ref counter);
            }
            else if (semanticList.First.Value.Data == "compoundstmt")
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, ref counter);
            }
            else
            {
                Output("stmt_go ERROR!!! 输入错误");
                Output(Environment.NewLine);
            }
        }

        private static void ifstmt_go(LinkedList<Token> semanticList, ref int counter)
        {
            int itemp = 0;
            if (semanticList.First.Value.Data == "if")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO IF !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "(")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO ( !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "boolexpr")
            {
                semanticList.RemoveFirst();
                string boolreg = boolexpr_go(semanticList, ref counter);
                itemp = counter;
                Output(counter.ToString());
                counter++;
                Output(" jmpf ");
                Output(boolreg);
                Output(" , ");
                Output(itemp.ToString());
                Output("else");
                Output(Environment.NewLine);
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO Boolexpr !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ")")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO ) !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "then")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO then !!!");
                Output(Environment.NewLine);
            }



            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, ref counter);
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO stmt!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "else")
            {
                semanticList.RemoveFirst();
                Output(itemp.ToString());
                Output("else");
                Output(Environment.NewLine);
            }
            else
            {
                Output("ifstmt_go ERROR!!! NO else !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, ref counter);
            }
            else
            {
                Output("ifstmt_go else ERROR !!! NO stmt!!!");
                Output(Environment.NewLine);
            }

        }

        private static void assgstmt_go(LinkedList<Token> semanticList, ref int counter)
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
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "=")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt ERROR!!! NO = !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "arithexpr")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt ERROR!!! NO calexpr !!!");
                Output(Environment.NewLine);
            }

            string reg = calexpr_go(semanticList, ref counter);
            Output(counter.ToString());
            counter++;
            Output(" MOV ");
            Output(id);
            Output(" , ");
            Output(reg);
            Output(Environment.NewLine);

            if (semanticList.First.Value.Data == ";")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("assgstmt_go ERROR!!! NO ;!!!");
                Output(Environment.NewLine);
            }
        }

        private static void whilestmt_go(LinkedList<Token> semanticList, ref int counter)
        {
            int itemp = 0;
            if (semanticList.First.Value.Data == "while")
            {
                semanticList.RemoveFirst();
                itemp = counter;
                Output(itemp.ToString());
                Output("while");
                Output(Environment.NewLine);
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO while!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == "(")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO (!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "boolexpr")
            {
                semanticList.RemoveFirst();
                string boolreg = boolexpr_go(semanticList, ref counter);
                Output(counter.ToString());
                counter++;
                Output(" jmpf ");
                Output(boolreg);
                Output(" , ");
                Output(itemp.ToString());
                Output("whileend");
                Output(Environment.NewLine);
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO boolexpr!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ")")
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO )");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "stmt")
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, ref counter);
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO stmt !!!");
                Output(Environment.NewLine);
            }

            Output(counter.ToString());
            counter++;
            Output(" jmp ");
            Output(itemp.ToString());
            Output("while");
            Output(Environment.NewLine);
            Output(itemp.ToString());
            Output("whileend");
            Output(Environment.NewLine);
        }

        private static string boolexpr_go(LinkedList<Token> semanticList, ref int counter)
        {
            string result = "t1";
            string a = string.Empty;
            string b = string.Empty;
            string cal = string.Empty;
            string s = string.Empty;
            if (semanticList.First.Value.Data == "arithexpr")
            {
                semanticList.RemoveFirst();
                a = calexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO arithexpr!!!");
                Output(Environment.NewLine);
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
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "arithexpr")
            {
                semanticList.RemoveFirst();
                b = calexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO arithexpr!!!");
                Output(Environment.NewLine);
            }

            if (cal == "<")
            {
                s = "lt";
            }
            else if (cal == ">")
            {
                s = "gt";
            }
            else if (cal == "<=")
            {
                s = "le";
            }
            else if (cal == ">=")
            {
                s = "ge";
            }
            else if (cal == "==")
            {
                s = "eq";
            }
            else
            {
                a = "";
                Output("boolexpr_go ERROR !!! boolop ERROR !!!");
                Output(Environment.NewLine);
            }

            Output(counter.ToString());
            counter++;
            Output(" ");
            Output(s);
            Output(" ");
            Output(result);
            Output(" , ");
            Output(a);
            Output(" , ");
            Output(b);
            Output(Environment.NewLine);
            return result;
        }

        private static string calexpr_go(LinkedList<Token> semanticList, ref int counter)
        {
            string a = "";
            string b = "";
            string cal = "";
            string s = "";
            string result = "t1";
            if (semanticList.First.Value.Data == "multexpr")
            {
                semanticList.RemoveFirst();
                a = multexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("calexpr_ERROR!!! NO multexpr!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == "arithexprprime")
            {
                semanticList.RemoveFirst();
                b = calexprim_go(semanticList, ref counter, ref cal);
            }
            else
            {
                Output("calexpr_go ERROR!!! NO arithexprprime!!!");
                Output(Environment.NewLine);
            }
            if (cal != "")
            {
                if (cal == "+")
                {
                    s = "ADD";
                }
                else if (cal == "-")
                {
                    s = "SUB";
                }
                Output(counter.ToString());
                counter++;
                Output(" ");
                Output(s);
                Output(" ");
                Output(result);
                Output(" , ");
                Output(a);
                Output(" , ");
                Output(b);
                Output(" , ");
                Output(Environment.NewLine);
            }
            else if (cal == "")
            {
                result = a;
            }
            return result;
        }

        private static string calexprim_go(LinkedList<Token> semanticList, ref int counter, ref string cal)
        {
            string cal1 = "";
            string result = "t1";
            string m1 = "";
            string act = "";
            if (semanticList.First.Value.Data == "+" || semanticList.First.Value.Data == "-")
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "multexpr")
                {
                    semanticList.RemoveFirst();
                    m1 = multexpr_go(semanticList, ref counter);
                }
                else
                {
                    Output("calexprim ERROR!!!NO multexpr");
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == "arithexprprime")
                {
                    semanticList.RemoveFirst();
                    string c1 = calexprim_go(semanticList, ref counter, ref cal1);
                    if (cal1 != "")
                    {
                        if (cal1 == "+")
                        {
                            act = "ADD";
                        }
                        else if (cal1 == "-")
                        {
                            act = "SUB";
                        }
                        Output(counter.ToString());
                        counter++;
                        Output(" ");
                        Output(act);
                        Output(" ");
                        Output(result);
                        Output(" , ");
                        Output(m1);
                        Output(" , ");
                        Output(c1);
                        Output(Environment.NewLine);
                    }
                    else if (cal1 == "")
                    {
                        result = m1;
                    }
                }
                else
                {
                    Output("calexprim_go ERROR!!! NO arithexprprime!!!");
                    Output(Environment.NewLine);
                }
            }
            else if (semanticList.First.Value.Data == "arithexprprime:ε")
            {
                cal = "";
                result = "";
                semanticList.RemoveFirst();
            }
            else
            {
                Output("calexprim_go ERROR!!! NO calop!!!");
                Output(Environment.NewLine);
            }

            return result;
        }

        private static string multexpr_go(LinkedList<Token> semanticList, ref int counter)
        {
            string result = "t1";
            string cal = "";
            string b = "";
            string a = "";
            string act = "";
            if (semanticList.First.Value.Data == "simpleexpr")
            {
                semanticList.RemoveFirst();
                a = simexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("multexpr ERROR!!!NO simpleexpr !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == "multexprprime")
            {
                semanticList.RemoveFirst();
                b = multexprim_go(semanticList, ref counter, ref cal);
            }
            else
            {
                Output("multexpr_go ERROR!!! NO multexprprime !!!");
                Output(Environment.NewLine);
            }

            if (cal != "")
            {
                if (cal == "*")
                {
                    act = "MUL";
                }
                else if (cal == "/")
                {
                    act = "DIV";
                }
                Output(counter.ToString());
                counter++;
                Output(" ");
                Output(act);
                Output(" ");
                Output(result);
                Output(" , ");
                Output(a);
                Output(" , ");
                Output(b);
                Output(Environment.NewLine);
            }
            else if (cal == "")
            {
                result = a;
            }
            else
            {
                Output("multexpr_go ERROR!!! calop lack !!!");
                Output(Environment.NewLine);
            }
            return result;
        }

        private static string multexprim_go(LinkedList<Token> semanticList, ref int counter, ref string cal)
        {
            string a = "";
            string b = "";
            string result = "t1";
            string cal1 = "";
            string act = "";
            if (semanticList.First.Value.Data == "*" || semanticList.First.Value.Data == "/")
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "simpleexpr")
                {
                    semanticList.RemoveFirst();
                    a = simexpr_go(semanticList, ref counter);
                    if (semanticList.First.Value.Data == "multexprprime")
                    {
                        semanticList.RemoveFirst();
                        b = multexprim_go(semanticList, ref counter, ref cal1);
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
                if (cal1 != "")
                {
                    if (cal1 == "*")
                    {
                        act = "MUL";
                    }
                    else if (cal1 == "/")
                    {
                        act = "DIV";
                    }
                    Output(counter.ToString());
                    counter++;
                    Output(" ");
                    Output(act);
                    Output(" ");
                    Output(result);
                    Output(" , ");
                    Output(a);
                    Output(" , ");
                    Output(b);
                    Output(Environment.NewLine);
                }
                else if (cal1 == "")
                {
                    result = a;
                }
                else
                {
                    Output("multexprim_go ERROR!!! boolop lack !!!");
                    Output(Environment.NewLine);
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
                Output(Environment.NewLine);
            }

            return result;
        }

        private static string simexpr_go(LinkedList<Token> semanticList, ref int counter)
        {
            string result = "";
            if (semanticList.First.Value.Type == "ID")
            {
                result = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else if (semanticList.First.Value.Type == "integer" || semanticList.First.Value.Type == "real_number")
            {
                result = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else if (semanticList.First.Value.Data == "(")
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == "arithexpr")
                {
                    semanticList.RemoveFirst();
                    result = calexpr_go(semanticList, ref counter);
                    if (semanticList.First.Value.Data == ")")
                    {
                        semanticList.RemoveFirst();
                    }
                    else
                    {
                        Output("simexpr_go ERROR!!!NO ) !!!");
                        Output(Environment.NewLine);
                    }
                }
                else
                {
                    Output("simexpr_go ERROR !!! NO arithexpr !!!");
                    Output(Environment.NewLine);
                }
            }
            else
            {
                Output("simexpr_go ERROR!!! 输入错误 !!!");
                Output(Environment.NewLine);
            }

            return result;
        }
    }
}

