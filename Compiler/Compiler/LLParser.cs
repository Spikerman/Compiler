using System;
using System.Collections.Generic;

namespace Compiler
{
    public static class LlParser
    {

        private static int _llSearchCounter;
        private static string _outputString = string.Empty;

        public static string Parser(LinkedList<Token> inputTokenList, LinkedList<Token> semanticList)
        {
            _outputString = string.Empty;
            bool key = true;
            Stack<int> sum = new Stack<int>();

            Stack<string> word = StackInit();

            while (word.Count > 0 && inputTokenList.Count > 0)
            {
                bool flag = LlSearch(word, inputTokenList, sum, semanticList);
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

            return _outputString;
        }

        private static Stack<string> StackInit()
        {
            //Stack<TreeItemViewModel> word = new Stack<TreeItemViewModel>();
            Stack<string> word = new Stack<string>();
            word.Push(Const.program);

            return word;
        }

        private static bool LlSearch(Stack<string> wordStack, LinkedList<Token> tokenList, Stack<int> sum, LinkedList<Token> semanticList)
        {
            string stackTopWord = wordStack.Peek();
            Token tokenTop = tokenList.First.Value;
            string tokenData = tokenTop.Data;
            string tokenType = tokenTop.Type;
            int lineNumber = tokenTop.LineNumber;
            int columnNumber = tokenTop.ColumnNumber;

            if (stackTopWord == Const.program && tokenData == Const.左大括号)
            {
                wordStack.Pop();
                wordStack.Push(Const.compoundstmt);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.program);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.program));
            }
            else if (stackTopWord == Const.stmt && tokenData == Const.@if)
            {
                wordStack.Pop();
                wordStack.Push(Const.ifstmt);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.stmt));
            }
            else if (stackTopWord == Const.stmt && tokenData == Const.@while)
            {
                wordStack.Pop();
                wordStack.Push(Const.whilestmt);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.stmt));
            }
            else if (stackTopWord == Const.stmt && tokenType == Const.ID)
            {
                wordStack.Pop();
                wordStack.Push(Const.assgstmt);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.stmt));
            }
            else if (stackTopWord == Const.stmt && tokenData == Const.左大括号)
            {
                wordStack.Pop();
                wordStack.Push(Const.compoundstmt);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.stmt));
            }
            else if (stackTopWord == Const.compoundstmt && tokenData == Const.左大括号)
            {
                wordStack.Pop();
                wordStack.Push(Const.右大括号);
                wordStack.Push(Const.stmts);
                wordStack.Push(Const.左大括号);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.compoundstmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.compoundstmt));
            }
            else if (stackTopWord == Const.stmts && (tokenData == Const.@if || tokenData == Const.@while || tokenData == Const.左大括号 || tokenType == Const.ID))
            {
                wordStack.Pop();
                wordStack.Push(Const.stmts);
                wordStack.Push(Const.stmt);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.stmts);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.stmts));
            }
            else if (stackTopWord == Const.stmts && tokenData == Const.右大括号)
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output(Const.stmts空);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.stmts));
                semanticList.AddLast(new Token(Const.end_terminal, Const.stmts空));
            }
            else if (stackTopWord == Const.ifstmt && tokenData == Const.@if)
            {
                wordStack.Pop();
                wordStack.Push(Const.stmt);
                wordStack.Push(Const.@else);
                wordStack.Push(Const.stmt);
                wordStack.Push(Const.then);
                wordStack.Push(Const.右括号);
                wordStack.Push(Const.boolexpr);
                wordStack.Push(Const.左括号);
                wordStack.Push(Const.@if);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                sum.Push(_llSearchCounter);
                Output(Const.ifstmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.ifstmt));
            }
            else if (stackTopWord == Const.whilestmt && tokenData == Const.@while)
            {
                wordStack.Pop();
                wordStack.Push(Const.stmt);
                wordStack.Push(Const.右括号);
                wordStack.Push(Const.boolexpr);
                wordStack.Push(Const.左括号);
                wordStack.Push(Const.@while);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.whilestmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.whilestmt));
            }
            else if (stackTopWord == Const.assgstmt && tokenType == Const.ID)
            {
                wordStack.Pop();
                wordStack.Push(Const.分号);
                wordStack.Push(Const.arithexpr);
                wordStack.Push(Const.等号);
                wordStack.Push(Const.ID);
                sum.Push(_llSearchCounter);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.assgstmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.assgstmt));
            }
            else if (stackTopWord == Const.boolexpr && (tokenType == Const.ID || tokenType == Const.integer || tokenType == Const.real_number || tokenData == Const.左括号))
            {
                wordStack.Pop();
                wordStack.Push(Const.arithexpr);
                wordStack.Push(Const.boolop);
                wordStack.Push(Const.arithexpr);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.boolexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.boolexpr));
            }
            else if (stackTopWord == Const.boolop && tokenType == Const.relation_operator)
            {
                wordStack.Pop();
                wordStack.Push(tokenData);
                PrintSpace(_llSearchCounter);
                Output(Const.boolop);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.boolop));
            }
            else if (stackTopWord == Const.arithexpr && (tokenType == Const.integer || tokenType == Const.real_number || tokenType == Const.ID || tokenData == Const.左括号))
            {
                wordStack.Pop();
                wordStack.Push(Const.arithexprprime);
                wordStack.Push(Const.multexpr);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.arithexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.arithexpr));
            }
            else if (stackTopWord == Const.arithexprprime && tokenData == Const.加号)
            {
                wordStack.Pop();
                wordStack.Push(Const.arithexprprime);
                wordStack.Push(Const.multexpr);
                wordStack.Push(Const.加号);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.arithexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.arithexprprime));
            }
            else if (stackTopWord == Const.arithexprprime && tokenData == Const.减号)
            {
                wordStack.Pop();
                wordStack.Push(Const.arithexprprime);
                wordStack.Push(Const.multexpr);
                wordStack.Push(Const.减号);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.arithexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.arithexprprime));
            }
            else if (stackTopWord == Const.arithexprprime && (tokenType == Const.relation_operator || tokenData == Const.右括号 || tokenData == Const.分号))
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output(Const.arithexprprime空);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.arithexprprime));
                semanticList.AddLast(new Token(Const.end_terminal, Const.arithexprprime空));
            }
            else if (stackTopWord == Const.multexpr && (tokenType == Const.ID || tokenType == Const.integer || tokenType == Const.real_number || tokenData == Const.左括号))
            {
                wordStack.Pop();
                wordStack.Push(Const.multexprprime);
                wordStack.Push(Const.simpleexpr);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.multexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.multexpr));
            }
            else if (stackTopWord == Const.multexprprime && tokenData == Const.乘号)
            {
                wordStack.Pop();
                wordStack.Push(Const.multexprprime);
                wordStack.Push(Const.simpleexpr);
                wordStack.Push(Const.乘号);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.multexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.multexprprime));
            }
            else if (stackTopWord == Const.multexprprime && tokenData == Const.除号)
            {
                wordStack.Pop();
                wordStack.Push(Const.multexprprime);
                wordStack.Push(Const.simpleexpr);
                wordStack.Push(Const.除号);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.multexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.multexprprime));
            }
            else if (stackTopWord == Const.multexprprime && (tokenType == Const.relation_operator || tokenData == Const.加号 || tokenData == Const.减号 || tokenData == Const.右括号 || tokenData == Const.分号))
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output(Const.multexprprime空);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.multexprprime));
                semanticList.AddLast(new Token(Const.end_terminal, Const.multexprprime空));
            }
            else if (stackTopWord == Const.simpleexpr && tokenType == Const.ID)
            {
                wordStack.Pop();
                wordStack.Push(Const.ID);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.simpleexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.simpleexpr));
            }
            else if (stackTopWord == Const.simpleexpr && (tokenType == Const.integer || tokenType == Const.real_number))
            {
                wordStack.Pop();
                wordStack.Push(Const.Num);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.simpleexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.simpleexpr));
            }
            else if (stackTopWord == Const.simpleexpr && tokenData == Const.左括号)
            {
                wordStack.Pop();
                wordStack.Push(Const.右括号);
                wordStack.Push(Const.arithexpr);
                wordStack.Push(Const.左括号);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(Const.simpleexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(Const.non_terminal, Const.simpleexpr));
            }
            else if (stackTopWord == Const.ID && tokenType == Const.ID)
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output(Const.ID);
                Output(Environment.NewLine);
                semanticList.AddLast(tokenTop);
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == Const.Num && (tokenType == Const.integer || tokenType == Const.real_number))
            {
                wordStack.Pop();
                PrintSpace(_llSearchCounter);
                Output(Const.Num);
                Output(Environment.NewLine);
                semanticList.AddLast(tokenTop);
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == tokenData)
            {
                wordStack.Pop();
                semanticList.AddLast(tokenTop);
                tokenList.RemoveFirst();
                if (tokenType == Const.@operator || tokenType == Const.relation_operator)
                {
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == Const.左括号 || tokenData == Const.左大括号)
                {
                    sum.Push(_llSearchCounter);
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == Const.右大括号 || tokenData == Const.分号 || tokenData == Const.右括号)
                {
                    _llSearchCounter = sum.Pop();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == Const.then)
                {
                    _llSearchCounter = sum.Peek();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == Const.@else)
                {
                    _llSearchCounter = sum.Pop();
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
                return false;
            }

            return true;
        }

        private static void PrintSpace(int i)
        {
            for (; i > 0; i--)
            {
                Output(" ");
            }
        }

        private static void Output(string content)
        {
            _outputString += content;
        }
    }
}
