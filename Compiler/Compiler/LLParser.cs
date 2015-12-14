using System;
using System.Collections.Generic;

namespace Compiler
{
    public class LlParser
    {
        private static int _llSearchCounter = 0;
        private static string _llSearchFile = string.Empty;

        public static bool LL_search(Stack<string> wordStack, LinkedList<Token> tokenList, Stack<int> sum, LinkedList<Token> semanticList)
        {
            bool key = true;
            string stackTopWord = wordStack.Peek();
            Token tokenTop = tokenList.First.Value;
            string tokenData = tokenTop.Data;
            string tokenType = tokenTop.Type;
            int lineNumber = tokenTop.LineNumber;
            int columnNumber = tokenTop.ColumnNumber;

            if (stackTopWord == "program" && tokenData == "{")
            {
                wordStack.Pop();
                wordStack.Push("compoundstmt");
                PrintSpace(_llSearchCounter++);
                Output("program");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "program"));
            }
            else if (stackTopWord == "stmt" && tokenData == "if")
            {
                wordStack.Pop();
                wordStack.Push("ifstmt");
                PrintSpace(_llSearchCounter++);
                Output("stmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "stmt"));
            }
            else if (stackTopWord == "stmt" && tokenData == "while")
            {
                wordStack.Pop();
                wordStack.Push("whilestmt");
                PrintSpace(_llSearchCounter++);
                Output("stmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "stmt"));
            }
            else if (stackTopWord == "stmt" && tokenType == "ID")
            {
                wordStack.Pop();
                wordStack.Push("assgstmt");
                PrintSpace(_llSearchCounter++);
                Output("stmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "stmt"));
            }
            else if (stackTopWord == "stmt" && tokenData == "{")
            {
                wordStack.Pop();
                wordStack.Push("compoundstmt");
                PrintSpace(_llSearchCounter++);
                Output("stmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "stmt"));
            }
            else if (stackTopWord == "compoundstmt" && tokenData == "{")
            {
                wordStack.Pop();
                wordStack.Push("}");
                wordStack.Push("stmts");
                wordStack.Push("{");
                PrintSpace(_llSearchCounter++);
                Output("compoundstmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "compoundstmt"));
            }
            else if (stackTopWord == "stmts" && (tokenData == "if" || tokenData == "while" || tokenData == "{" || tokenType == "ID"))
            {
                wordStack.Pop();
                wordStack.Push("stmts");
                wordStack.Push("stmt");
                PrintSpace(_llSearchCounter++);
                Output("stmts");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "stmts"));
            }
            else if (stackTopWord == "stmts" && tokenData == "}")
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output("stmts:ε");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "stmts"));
                semanticList.AddLast(new Token("end_terminal", "stmts:ε"));
            }
            else if (stackTopWord == "ifstmt" && tokenData == "if")
            {
                wordStack.Pop();
                wordStack.Push("stmt");
                wordStack.Push("else");
                wordStack.Push("stmt");
                wordStack.Push("then");
                wordStack.Push(")");
                wordStack.Push("boolexpr");
                wordStack.Push("(");
                wordStack.Push("if");
                PrintSpace(_llSearchCounter++);
                sum.Push(_llSearchCounter);
                Output("ifstmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "ifstmt"));
            }
            else if (stackTopWord == "whilestmt" && tokenData == "while")
            {
                wordStack.Pop();
                wordStack.Push("stmt");
                wordStack.Push(")");
                wordStack.Push("boolexpr");
                wordStack.Push("(");
                wordStack.Push("while");
                PrintSpace(_llSearchCounter++);
                Output("whilestmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "whilestmt"));
            }
            else if (stackTopWord == "assgstmt" && tokenType == "ID")
            {
                wordStack.Pop();
                wordStack.Push(";");
                wordStack.Push("arithexpr");
                wordStack.Push("=");
                wordStack.Push("ID");
                sum.Push(_llSearchCounter);
                PrintSpace(_llSearchCounter++);
                Output("assgstmt");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "assgstmt"));
            }
            else if (stackTopWord == "boolexpr" && (tokenType == "ID" || tokenType == "integer" || tokenType == "real_number" || tokenData == "("))
            {
                wordStack.Pop();
                wordStack.Push("arithexpr");
                wordStack.Push("boolop");
                wordStack.Push("arithexpr");
                PrintSpace(_llSearchCounter++);
                Output("boolexpr");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "boolexpr"));
            }
            else if (stackTopWord == "boolop" && tokenType == "relation_operator")
            {
                wordStack.Pop();
                wordStack.Push(tokenData);
                PrintSpace(_llSearchCounter);
                Output("boolop");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "boolop"));
            }
            else if (stackTopWord == "arithexpr" && (tokenType == "integer" || tokenType == "real_number" || tokenType == "ID" || tokenData == "("))
            {
                wordStack.Pop();
                wordStack.Push("arithexprprime");
                wordStack.Push("multexpr");
                PrintSpace(_llSearchCounter++);
                Output("arithexpr");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "arithexpr"));
            }
            else if (stackTopWord == "arithexprprime" && tokenData == "+")
            {
                wordStack.Pop();
                wordStack.Push("arithexprprime");
                wordStack.Push("multexpr");
                wordStack.Push("+");
                PrintSpace(_llSearchCounter++);
                Output("arithexprprime");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "arithexprprime"));
            }
            else if (stackTopWord == "arithexprprime" && tokenData == "-")
            {
                wordStack.Pop();
                wordStack.Push("arithexprprime");
                wordStack.Push("multexpr");
                wordStack.Push("-");
                PrintSpace(_llSearchCounter++);
                Output("arithexprprime");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "arithexprprime"));
            }
            else if (stackTopWord == "arithexprprime" &&
                     (tokenType == "relation_operator" || tokenData == ")" || tokenData == ";"))
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output("arithexprprime:ε");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "arithexprprime"));
                semanticList.AddLast(new Token("end_terminal", "arithexprprime:ε"));
            }
            else if (stackTopWord == "multexpr" &&
                     (tokenType == "ID" || tokenType == "integer" || tokenType == "real_number" ||
                      tokenData == "("))
            {
                wordStack.Pop();
                wordStack.Push("multexprprime");
                wordStack.Push("simpleexpr");
                PrintSpace(_llSearchCounter++);
                Output("multexpr");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "multexpr"));
            }
            else if (stackTopWord == "multexprprime" && tokenData == "*")
            {
                wordStack.Pop();
                wordStack.Push("multexprprime");
                wordStack.Push("simpleexpr");
                wordStack.Push("*");
                PrintSpace(_llSearchCounter++);
                Output("multexprprime");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "multexprprime"));
            }
            else if (stackTopWord == "multexprprime" && tokenData == "/")
            {
                wordStack.Pop();
                wordStack.Push("multexprprime");
                wordStack.Push("simpleexpr");
                wordStack.Push("/");
                PrintSpace(_llSearchCounter++);
                Output("multexprprime");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "multexprprime"));
            }
            else if (stackTopWord == "multexprprime" && (tokenType == "relation_operator" || tokenData == "+" || tokenData == "-" || tokenData == ")" || tokenData == ";"))
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output("multexprprime:ε");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "multexprprime"));
                semanticList.AddLast(new Token("end_terminal", "multexprprime:ε"));
            }
            else if (stackTopWord == "simpleexpr" && tokenType == "ID")
            {
                wordStack.Pop();
                wordStack.Push("ID");
                PrintSpace(_llSearchCounter++);
                Output("simpleexpr");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "simpleexpr"));
            }
            else if (stackTopWord == "simpleexpr" && (tokenType == "integer" || tokenType == "real_number"))
            {
                wordStack.Pop();
                wordStack.Push("Num");
                PrintSpace(_llSearchCounter++);
                Output("simpleexpr");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "simpleexpr"));
            }
            else if (stackTopWord == "simpleexpr" && tokenData == "(")
            {
                wordStack.Pop();
                wordStack.Push(")");
                wordStack.Push("arithexpr");
                wordStack.Push("(");
                PrintSpace(_llSearchCounter++);
                Output("simpleexpr");
                Output(Environment.NewLine);
                semanticList.AddLast(new Token("non_terminal", "simpleexpr"));
            }
            else if (stackTopWord == "ID" && tokenType == "ID")
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output("ID");
                Output(Environment.NewLine);
                semanticList.AddLast(tokenList.First.Value);
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == "Num" && (tokenType == "integer" || tokenType == "real_number"))
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output("Num");
                Output(Environment.NewLine);
                semanticList.AddLast(tokenList.First.Value);
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == tokenData)
            {
                wordStack.Pop();
                Token t = tokenList.First.Value;

                semanticList.AddLast(t);
                tokenList.RemoveFirst();
                if (tokenType == "operator" || tokenType == "relation_operator")
                {
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == "(" || tokenData == "{")
                {
                    sum.Push(_llSearchCounter);
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == "}" || tokenData == ";" || tokenData == ")")
                {
                    _llSearchCounter = sum.Peek();
                    sum.Pop();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == "then")
                {
                    _llSearchCounter = sum.Peek();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == "else")
                {
                    _llSearchCounter = sum.Peek();
                    sum.Pop();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
            }
            else
            {
                Output("Warning:!!!! [");
                Output(lineNumber.ToString());
                Output(" ");
                Output(columnNumber.ToString());
                Output("] ERROR !!!");
                Output(Environment.NewLine);
                Output("栈中:");
                Output(stackTopWord);
                Output("  输入:");
                Output(tokenData);
                Output(Environment.NewLine);
                key = false;
            }

            return key;
        }

        public static bool LLparser(LinkedList<Token> in1, LinkedList<Token> semanticList)
        {
            bool key = true;
            Stack<int> sum = new Stack<int>();

            Stack<string> word = stack_init();

            while (word.Count > 0 && in1.Count > 0)
            {
                bool flag = LL_search(word, in1, sum, semanticList);
                if (flag == false)
                {
                    key = false;
                    word.Pop();
                    break;
                }
            }
            if (key)
            {
                Output("Graduation:!!!  LLparser Pass and the program is 合法 !!! ");
                Output(Environment.NewLine);
            }

            return key;
        }

        public static Stack<string> stack_init()
        {
            Stack<string> word = new Stack<string>();
            word.Push("program");

            return word;
        }

        public static void PrintSpace(int i)
        {
            for (; i > 0; i--)
            {
                Output(" ");
            }
        }

        public static void Output(string content)
        {
            _llSearchFile += content;
        }
    }
}
