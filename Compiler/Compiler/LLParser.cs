using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Compiler.DataModel.Runtime;

namespace Compiler
{
    public class LlParser
    {

        private static int _llSearchCounter;
        private static string _outputString = string.Empty;
        private static ObservableCollection<TreeItemViewModel> _treeItems;

        public static string Parser(LinkedList<Token> inputTokenList, LinkedList<Token> semanticList, ObservableCollection<TreeItemViewModel> treeItems)
        {
            _treeItems = treeItems;
            _outputString = string.Empty;
            bool key = true;
            Stack<int> sum = new Stack<int>();

            Stack<TreeItemViewModel> word = StackInit();

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
                Output("Congratulations:!!!  LLparser Pass and the program is 合法 !!! ");
                Output(Environment.NewLine);
            }

            return _outputString;
        }

        private static Stack<TreeItemViewModel> StackInit()
        {
            Stack<TreeItemViewModel> word = new Stack<TreeItemViewModel>();
            TreeItemViewModel child = new TreeItemViewModel
            {
                Text = ConstString.Program
            };
            word.Push(child);
            _treeItems.Add(child);
            return word;
        }

        private static bool LlSearch(Stack<TreeItemViewModel> wordStack, LinkedList<Token> tokenList, Stack<int> sum, LinkedList<Token> semanticList)
        {
            string stackTopWord = wordStack.Peek().Text;
            Token tokenTop = tokenList.First.Value;
            string tokenData = tokenTop.Data;
            string tokenType = tokenTop.Type;
            int lineNumber = tokenTop.LineNumber;
            int columnNumber = tokenTop.ColumnNumber;

            if (stackTopWord == ConstString.Program && tokenData == ConstString.左大括号)
            {
                string[] 体 = { ConstString.Compoundstmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Program);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Program));
            }
            else if (stackTopWord == ConstString.Stmt && tokenData == ConstString.If)
            {
                string[] 体 = { ConstString.Ifstmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Stmt));
            }
            else if (stackTopWord == ConstString.Stmt && tokenData == ConstString.While)
            {
                string[] 体 = { ConstString.Whilestmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Stmt));
            }
            else if (stackTopWord == ConstString.Stmt && tokenType == ConstString.Id)
            {
                string[] 体 = { ConstString.Assgstmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Stmt));
            }
            else if (stackTopWord == ConstString.Stmt && tokenData == ConstString.左大括号)
            {
                string[] 体 = { ConstString.Compoundstmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Stmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Stmt));
            }
            else if (stackTopWord == ConstString.Compoundstmt && tokenData == ConstString.左大括号)
            {
                string[] 体 = { ConstString.左大括号, ConstString.Stmts, ConstString.右大括号 };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Compoundstmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Compoundstmt));
            }
            else if (stackTopWord == ConstString.Stmts && (tokenData == ConstString.If || tokenData == ConstString.While || tokenData == ConstString.左大括号 || tokenType == ConstString.Id))
            {
                string[] 体 = { ConstString.Stmt, ConstString.Stmts };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Stmts);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Stmts));
            }
            else if (stackTopWord == ConstString.Stmts && tokenData == ConstString.右大括号)
            {
                string[] 体 = { };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                Output(ConstString.Stmts空);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Stmts));
                semanticList.AddLast(new Token(ConstString.EndTerminal, ConstString.Stmts空));
            }
            else if (stackTopWord == ConstString.Ifstmt && tokenData == ConstString.If)
            {
                string[] 体 = {ConstString.If, ConstString.左括号, ConstString.Boolexpr,
                    ConstString.右括号, ConstString.Then, ConstString.Stmt, ConstString.Else, ConstString.Stmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                sum.Push(_llSearchCounter);
                Output(ConstString.Ifstmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Ifstmt));
            }
            else if (stackTopWord == ConstString.Whilestmt && tokenData == ConstString.While)
            {
                string[] 体 = { ConstString.While, ConstString.左括号, ConstString.Boolexpr, ConstString.右括号, ConstString.Stmt };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Whilestmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Whilestmt));
            }
            else if (stackTopWord == ConstString.Assgstmt && tokenType == ConstString.Id)
            {
                string[] 体 = { ConstString.Id, ConstString.等号, ConstString.Arithexpr, ConstString.分号 };
                出栈入栈(wordStack, 体);
                sum.Push(_llSearchCounter);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Assgstmt);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Assgstmt));
            }
            else if (stackTopWord == ConstString.Boolexpr && (tokenType == ConstString.Id || tokenType == ConstString.Integer || tokenType == ConstString.RealNumber || tokenData == ConstString.左括号))
            {
                string[] 体 = { ConstString.Arithexpr, ConstString.Boolop, ConstString.Arithexpr };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Boolexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Boolexpr));
            }
            else if (stackTopWord == ConstString.Boolop && tokenType == ConstString.RelationOperator)
            {
                string[] 体 = { tokenData };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                Output(ConstString.Boolop);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Boolop));
            }
            else if (stackTopWord == ConstString.Arithexpr && (tokenType == ConstString.Integer || tokenType == ConstString.RealNumber || tokenType == ConstString.Id || tokenData == ConstString.左括号))
            {
                string[] 体 = { ConstString.Multexpr, ConstString.Arithexprprime };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Arithexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Arithexpr));
            }
            else if (stackTopWord == ConstString.Arithexprprime && tokenData == ConstString.加号)
            {
                string[] 体 = { ConstString.加号, ConstString.Multexpr, ConstString.Arithexprprime };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Arithexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Arithexprprime));
            }
            else if (stackTopWord == ConstString.Arithexprprime && tokenData == ConstString.减号)
            {
                string[] 体 = { ConstString.减号, ConstString.Multexpr, ConstString.Arithexprprime };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Arithexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Arithexprprime));
            }
            else if (stackTopWord == ConstString.Arithexprprime && (tokenType == ConstString.RelationOperator || tokenData == ConstString.右括号 || tokenData == ConstString.分号))
            {
                string[] 体 = { };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                Output(ConstString.Arithexprprime空);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Arithexprprime));
                semanticList.AddLast(new Token(ConstString.EndTerminal, ConstString.Arithexprprime空));
            }
            else if (stackTopWord == ConstString.Multexpr && (tokenType == ConstString.Id || tokenType == ConstString.Integer || tokenType == ConstString.RealNumber || tokenData == ConstString.左括号))
            {
                string[] 体 = { ConstString.Simpleexpr, ConstString.Multexprprime };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Multexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Multexpr));
            }
            else if (stackTopWord == ConstString.Multexprprime && tokenData == ConstString.乘号)
            {
                string[] 体 = { ConstString.乘号, ConstString.Simpleexpr, ConstString.Multexprprime };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Multexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Multexprprime));
            }
            else if (stackTopWord == ConstString.Multexprprime && tokenData == ConstString.除号)
            {
                string[] 体 = { ConstString.除号, ConstString.Simpleexpr, ConstString.Multexprprime };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Multexprprime);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Multexprprime));
            }
            else if (stackTopWord == ConstString.Multexprprime && (tokenType == ConstString.RelationOperator || tokenData == ConstString.加号 || tokenData == ConstString.减号 || tokenData == ConstString.右括号 || tokenData == ConstString.分号))
            {
                string[] 体 = { };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                Output(ConstString.Multexprprime空);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Multexprprime));
                semanticList.AddLast(new Token(ConstString.EndTerminal, ConstString.Multexprprime空));
            }
            else if (stackTopWord == ConstString.Simpleexpr && tokenType == ConstString.Id)
            {
                string[] 体 = { ConstString.Id };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Simpleexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Simpleexpr));
            }
            else if (stackTopWord == ConstString.Simpleexpr && (tokenType == ConstString.Integer || tokenType == ConstString.RealNumber))
            {
                string[] 体 = { ConstString.Num };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Simpleexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Simpleexpr));
            }
            else if (stackTopWord == ConstString.Simpleexpr && tokenData == ConstString.左括号)
            {
                string[] 体 = { ConstString.左括号, ConstString.Arithexpr, ConstString.右括号 };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                _llSearchCounter++;
                Output(ConstString.Simpleexpr);
                Output(Environment.NewLine);
                semanticList.AddLast(new Token(ConstString.NonTerminal, ConstString.Simpleexpr));
            }
            else if (stackTopWord == ConstString.Id && tokenType == ConstString.Id)
            {
                string[] 体 = { };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                Output(ConstString.Id);
                Output(Environment.NewLine);
                semanticList.AddLast(tokenTop);
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Num && (tokenType == ConstString.Integer || tokenType == ConstString.RealNumber))
            {
                string[] 体 = { };
                出栈入栈(wordStack, 体);
                PrintSpace(_llSearchCounter);
                Output(ConstString.Num);
                Output(Environment.NewLine);
                semanticList.AddLast(tokenTop);
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Boolop && (tokenType == ConstString.Id || tokenType == ConstString.Integer || tokenData == ConstString.右括号))
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Program && tokenData == ConstString.EndTerminal)
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Stmts && tokenData == ConstString.右大括号)
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Boolexpr && tokenData == ConstString.右括号)
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Arithexpr && (tokenType == ConstString.RelationOperator || tokenData == ConstString.分号 || tokenData == ConstString.右括号))
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Multexpr && (tokenData == ConstString.加号 || tokenData == ConstString.减号))
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Simpleexpr && (tokenData == ConstString.乘号 || tokenData == ConstString.除号))
            {
                wordStack.Pop();
            }
            else if (stackTopWord == ConstString.Program && (tokenData == ConstString.If || tokenData == ConstString.While || tokenType == ConstString.Id
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Ifstmt && (tokenData == ConstString.左大括号 || tokenData == ConstString.While || tokenType == ConstString.Id
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Whilestmt && (tokenData == ConstString.If || tokenData == ConstString.左大括号 || tokenType == ConstString.Id
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Assgstmt && (tokenData == ConstString.If || tokenData == ConstString.While || tokenData == ConstString.左大括号
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Stmt && (
                tokenData == ConstString.Stmts空 || tokenData == ConstString.左大括号 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenData == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Stmts && (
                 tokenType == ConstString.Integer || tokenData == ConstString.左大括号 || tokenData == ConstString.左括号 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Boolexpr && (tokenData == ConstString.If || tokenData == ConstString.左大括号 || tokenData == ConstString.While
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号
                ))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Boolop && (tokenData == ConstString.If || tokenData == ConstString.左大括号 || tokenData == ConstString.While
                || tokenData == ConstString.Stmts空 || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Arithexpr && (tokenData == ConstString.If || tokenData == ConstString.左大括号 || tokenData == ConstString.While
                || tokenData == ConstString.Stmts空 || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.右大括号
                ))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Arithexprprime && (tokenData == ConstString.If || tokenData == ConstString.左大括号 || tokenData == ConstString.While || tokenType == ConstString.Id
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号
                 || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.右大括号
                ))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Multexpr && (tokenData == ConstString.If || tokenData == ConstString.While || tokenData == ConstString.左大括号
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.RelationOperator
                || tokenData == ConstString.乘号 || tokenData == ConstString.除号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Multexprprime && (tokenData == ConstString.If || tokenData == ConstString.While || tokenType == ConstString.Id
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenType == ConstString.RelationOperator || tokenData == ConstString.左大括号
                || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Simpleexpr && (tokenData == ConstString.If || tokenData == ConstString.While || tokenData == ConstString.左大括号
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }
            else if (stackTopWord == ConstString.Compoundstmt && (tokenData == ConstString.If || tokenData == ConstString.While || tokenType == ConstString.Id
                || tokenData == ConstString.Stmts空 || tokenType == ConstString.Integer || tokenData == ConstString.左括号 || tokenData == ConstString.RelationOperator || tokenData == ConstString.加号
                || tokenData == ConstString.减号 || tokenData == ConstString.乘号 || tokenData == ConstString.除号
                 || tokenData == ConstString.分号 || tokenData == ConstString.右大括号 ||
                tokenData == ConstString.右括号))
            {
                tokenList.RemoveFirst();
            }



            else if (stackTopWord == tokenData)
            {
                string[] 体 = { };
                出栈入栈(wordStack, 体);
                semanticList.AddLast(tokenTop);
                tokenList.RemoveFirst();
                if (tokenType == ConstString.Operator || tokenType == ConstString.RelationOperator)
                {
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == ConstString.左括号 || tokenData == ConstString.左大括号)
                {
                    sum.Push(_llSearchCounter);
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == ConstString.右大括号 || tokenData == ConstString.分号 || tokenData == ConstString.右括号)
                {
                    _llSearchCounter = sum.Pop();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == ConstString.Then)
                {
                    _llSearchCounter = sum.Peek();
                    PrintSpace(_llSearchCounter);
                    Output(tokenData);
                    Output(Environment.NewLine);
                }
                else if (tokenData == ConstString.Else)
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

        private static void 出栈入栈(Stack<TreeItemViewModel> wordStack, string[] 产生式体)
        {
            TreeItemViewModel node = wordStack.Pop();
            for (int i = 产生式体.Length - 1; i >= 0; i--)
            {
                TreeItemViewModel child = new TreeItemViewModel
                {
                    Text = 产生式体[i]
                };

                wordStack.Push(child);
                node.Children.Insert(0, child);
            }
        }
    }
}
