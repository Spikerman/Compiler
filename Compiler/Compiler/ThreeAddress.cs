using System;
using System.Collections.Generic;

namespace Compiler
{
    public static class ThreeAddress
    {
        public static string ErrorDisplay { get; } = "{0} ERROR: NO {1}.";
        private static string _outputString = string.Empty;

        private static void Output(string content)
        {
            _outputString += content;
        }

        public static string semantic_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.Count > 0)
            {
                if (semanticList.First.Value.Data == ConstString.Program)
                {
                    semanticList.RemoveFirst();
                    program_go(semanticList, ref counter);
                }
                else
                {
                    Output(string.Format(ErrorDisplay, nameof(semantic_go), ConstString.Program));
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
            if (semanticList.First.Value.Data == ConstString.Compoundstmt)
            {
                semanticList.RemoveFirst();
                comp_go(semanticList, ref counter);
            }
            else
            {
                Output(string.Format(ErrorDisplay, nameof(program_go), ConstString.Compoundstmt));
                Output(Environment.NewLine);
            }
        }

        private static void comp_go(LinkedList<Token> semanticList, ref int counter)
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
                stmts_go(semanticList, ref counter);
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

        private static void stmts_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.First.Value.Data == ConstString.Stmt)
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, ref counter);
                if (semanticList.First.Value.Data == ConstString.Stmts)
                {
                    semanticList.RemoveFirst();
                    stmts_go(semanticList, ref counter);
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

        private static void stmt_go(LinkedList<Token> semanticList, ref int counter)
        {
            if (semanticList.First.Value.Data == ConstString.Ifstmt)
            {
                semanticList.RemoveFirst();
                ifstmt_go(semanticList, ref counter);
            }
            else if (semanticList.First.Value.Data == ConstString.Whilestmt)
            {
                semanticList.RemoveFirst();
                whilestmt_go(semanticList, ref counter);
            }
            else if (semanticList.First.Value.Data == ConstString.Assgstmt)
            {
                semanticList.RemoveFirst();
                assgstmt_go(semanticList, ref counter);
            }
            else if (semanticList.First.Value.Data == ConstString.Compoundstmt)
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
            if (semanticList.First.Value.Data == ConstString.If)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO IF !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.左括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO ( !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Boolexpr)
            {
                semanticList.RemoveFirst();
                string boolreg = boolexpr_go(semanticList, ref counter);
                itemp = counter;
                Output(counter.ToString());
                counter++;
                Output(" ");
                Output(ConstString.Jmpf);
                Output(" ");
                Output(boolreg);
                Output(" , ");
                Output(itemp.ToString());
                Output(ConstString.Else);
                Output(Environment.NewLine);
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO Boolexpr !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.右括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO ) !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Then)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO then !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Stmt)
            {
                semanticList.RemoveFirst();
                stmt_go(semanticList, ref counter);
            }
            else
            {
                Output("ifstmt_go ERROR!!!NO stmt!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Else)
            {
                semanticList.RemoveFirst();
                Output(itemp.ToString());
                Output(ConstString.Else);
                Output(Environment.NewLine);
            }
            else
            {
                Output("ifstmt_go ERROR!!! NO else !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Stmt)
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

            string reg = calexpr_go(semanticList, ref counter);
            Output(counter.ToString());
            counter++;
            Output(" ");
            Output(ConstString.Mov);
            Output(" ");
            Output(id);
            Output(" , ");
            Output(reg);
            Output(Environment.NewLine);

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

        private static void whilestmt_go(LinkedList<Token> semanticList, ref int counter)
        {
            int itemp = 0;
            if (semanticList.First.Value.Data == ConstString.While)
            {
                semanticList.RemoveFirst();
                itemp = counter;
                Output(itemp.ToString());
                Output(ConstString.While);
                Output(Environment.NewLine);
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO while!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.左括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO (!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Boolexpr)
            {
                semanticList.RemoveFirst();
                string boolreg = boolexpr_go(semanticList, ref counter);
                Output(counter.ToString());
                counter++;
                Output(" ");
                Output(ConstString.Jmpf);
                Output(" ");
                Output(boolreg);
                Output(" , ");
                Output(itemp.ToString());
                Output(ConstString.Whileend);
                Output(Environment.NewLine);
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO boolexpr!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.右括号)
            {
                semanticList.RemoveFirst();
            }
            else
            {
                Output("whilestmt_go ERROR!!! NO )");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Stmt)
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
            Output(" ");
            Output(ConstString.Jmp);
            Output(" ");
            Output(itemp.ToString());
            Output(ConstString.While);
            Output(Environment.NewLine);
            Output(itemp.ToString());
            Output(ConstString.Whileend);
            Output(Environment.NewLine);
        }

        private static string boolexpr_go(LinkedList<Token> semanticList, ref int counter)
        {
            string result = "t1";
            string a = string.Empty;
            string b = string.Empty;
            string cal = string.Empty;
            string s = string.Empty;
            if (semanticList.First.Value.Data == ConstString.Arithexpr)
            {
                semanticList.RemoveFirst();
                a = calexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO arithexpr!!!");
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
                Output("boolexpr_go ERROR!!! NO boolop!!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Arithexpr)
            {
                semanticList.RemoveFirst();
                b = calexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("boolexpr_go ERROR!!! NO arithexpr!!!");
                Output(Environment.NewLine);
            }

            if (cal == ConstString.小于号)
            {
                s = ConstString.Lt;
            }
            else if (cal == ConstString.大于号)
            {
                s = ConstString.Gt;
            }
            else if (cal == ConstString.小于等于号)
            {
                s = ConstString.Le;
            }
            else if (cal == ConstString.大于等于号)
            {
                s = ConstString.Ge;
            }
            else if (cal == ConstString.相等号)
            {
                s = ConstString.Eq;
            }
            else
            {
                a = string.Empty;
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
            string a = string.Empty;
            string b = string.Empty;
            string cal = string.Empty;
            string s = string.Empty;
            string result = "t1";
            if (semanticList.First.Value.Data == ConstString.Multexpr)
            {
                semanticList.RemoveFirst();
                a = multexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("calexpr_ERROR!!! NO multexpr!!!");
                Output(Environment.NewLine);
            }
            if (semanticList.First.Value.Data == ConstString.Arithexprprime)
            {
                semanticList.RemoveFirst();
                b = calexprim_go(semanticList, ref counter, ref cal);
            }
            else
            {
                Output("calexpr_go ERROR!!! NO arithexprprime!!!");
                Output(Environment.NewLine);
            }
            if (cal != string.Empty)
            {
                if (cal == ConstString.加号)
                {
                    s = ConstString.Add;
                }
                else if (cal == ConstString.减号)
                {
                    s = ConstString.Sub;
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
            else if (cal == string.Empty)
            {
                result = a;
            }
            return result;
        }

        private static string calexprim_go(LinkedList<Token> semanticList, ref int counter, ref string cal)
        {
            string cal1 = string.Empty;
            string result = "t1";
            string m1 = string.Empty;
            string act = string.Empty;
            if (semanticList.First.Value.Data == ConstString.加号 || semanticList.First.Value.Data == ConstString.减号)
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == ConstString.Multexpr)
                {
                    semanticList.RemoveFirst();
                    m1 = multexpr_go(semanticList, ref counter);
                }
                else
                {
                    Output("calexprim ERROR!!!NO multexpr");
                    Output(Environment.NewLine);
                }
                if (semanticList.First.Value.Data == ConstString.Arithexprprime)
                {
                    semanticList.RemoveFirst();
                    string c1 = calexprim_go(semanticList, ref counter, ref cal1);
                    if (cal1 != string.Empty)
                    {
                        if (cal1 == ConstString.加号)
                        {
                            act = ConstString.Add;
                        }
                        else if (cal1 == ConstString.减号)
                        {
                            act = ConstString.Sub;
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
                    else if (cal1 == string.Empty)
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
            else if (semanticList.First.Value.Data == ConstString.Arithexprprime空)
            {
                cal = string.Empty;
                result = string.Empty;
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
            string cal = string.Empty;
            string b = string.Empty;
            string a = string.Empty;
            string act = string.Empty;
            if (semanticList.First.Value.Data == ConstString.Simpleexpr)
            {
                semanticList.RemoveFirst();
                a = simexpr_go(semanticList, ref counter);
            }
            else
            {
                Output("multexpr ERROR!!!NO simpleexpr !!!");
                Output(Environment.NewLine);
            }

            if (semanticList.First.Value.Data == ConstString.Multexprprime)
            {
                semanticList.RemoveFirst();
                b = multexprim_go(semanticList, ref counter, ref cal);
            }
            else
            {
                Output("multexpr_go ERROR!!! NO multexprprime !!!");
                Output(Environment.NewLine);
            }

            if (cal != string.Empty)
            {
                if (cal == ConstString.乘号)
                {
                    act = ConstString.Mul;
                }
                else if (cal == ConstString.除号)
                {
                    act = ConstString.Div;
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
            else if (cal == string.Empty)
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
            string a = string.Empty;
            string b = string.Empty;
            string result = "t1";
            string cal1 = string.Empty;
            string act = string.Empty;
            if (semanticList.First.Value.Data == ConstString.乘号 || semanticList.First.Value.Data == ConstString.除号)
            {
                cal = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == ConstString.Simpleexpr)
                {
                    semanticList.RemoveFirst();
                    a = simexpr_go(semanticList, ref counter);
                    if (semanticList.First.Value.Data == ConstString.Multexprprime)
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
                if (cal1 != string.Empty)
                {
                    if (cal1 == ConstString.乘号)
                    {
                        act = ConstString.Mul;
                    }
                    else if (cal1 == ConstString.除号)
                    {
                        act = ConstString.Div;
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
                else if (cal1 == string.Empty)
                {
                    result = a;
                }
                else
                {
                    Output("multexprim_go ERROR!!! boolop lack !!!");
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

            return result;
        }

        private static string simexpr_go(LinkedList<Token> semanticList, ref int counter)
        {
            string result = string.Empty;
            if (semanticList.First.Value.Type == ConstString.Id)
            {
                result = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else if (semanticList.First.Value.Type == ConstString.Integer || semanticList.First.Value.Type == ConstString.RealNumber)
            {
                result = semanticList.First.Value.Data;
                semanticList.RemoveFirst();
            }
            else if (semanticList.First.Value.Data == ConstString.左括号)
            {
                semanticList.RemoveFirst();
                if (semanticList.First.Value.Data == ConstString.Arithexpr)
                {
                    semanticList.RemoveFirst();
                    result = calexpr_go(semanticList, ref counter);
                    if (semanticList.First.Value.Data == ConstString.右括号)
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

