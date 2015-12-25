using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    public class LlTable
    {
        //Terminals
        //Nonterminals
        List<Production> Productions { get; } = new List<Production>
        {
            new Production
            {
                Head = ConstString.Program,
                Body = new [] { ConstString.Compoundstmt }
            },
            new Production
            {
                Head = ConstString.Stmt,
                Body = new [] { ConstString.Ifstmt }
            },
            new Production
            {
                Head = ConstString.Stmt,
                Body = new [] { ConstString.Whilestmt }
            },
            new Production
            {
                Head = ConstString.Stmt,
                Body = new [] { ConstString.Assgstmt }
            },
            new Production
            {
                Head = ConstString.Stmt,
                Body = new [] { ConstString.Compoundstmt }
            },
            new Production
            {
                Head = ConstString.Compoundstmt,
                Body = new [] { ConstString.左大括号, ConstString.Stmts, ConstString.右大括号 }
            },
            new Production
            {
                Head = ConstString.Stmts,
                Body = new [] { ConstString.Stmt, ConstString.Stmts }
            },
            new Production
            {
                Head = ConstString.Stmts,
                Body = new string[] { }
            },
            new Production
            {
                Head = ConstString.Ifstmt,
                Body = new [] {ConstString.If, ConstString.左括号, ConstString.Boolexpr, ConstString.右括号, ConstString.Then, ConstString.Stmt, ConstString.Else, ConstString.Stmt }
            },
            new Production
            {
                Head = ConstString.Whilestmt,
                Body = new [] { ConstString.While, ConstString.左括号, ConstString.Boolexpr, ConstString.右括号, ConstString.Stmt }
            },
            new Production
            {
                Head = ConstString.Assgstmt,
                Body = new [] { ConstString.Id, ConstString.等号, ConstString.Arithexpr, ConstString.分号 }
            },
            new Production
            {
                Head = ConstString.Boolexpr,
                Body = new [] { ConstString.Arithexpr, ConstString.Boolop, ConstString.Arithexpr }
            },
            new Production
            {
                Head = ConstString.Boolop,
                Body = new [] { ConstString.小于号 }
            },
            new Production
            {
                Head = ConstString.Boolop,
                Body = new [] { ConstString.大于号 }
            },
            new Production
            {
                Head = ConstString.Boolop,
                Body = new [] { ConstString.小于等于号 }
            },
            new Production
            {
                Head = ConstString.Boolop,
                Body = new [] { ConstString.大于等于号 }
            },
            new Production
            {
                Head = ConstString.Boolop,
                Body = new [] { ConstString.相等号 }
            },
            new Production
            {
                Head = ConstString.Arithexpr,
                Body = new [] { ConstString.Multexpr, ConstString.Arithexprprime }
            },
            new Production
            {
                Head = ConstString.Arithexprprime,
                Body = new [] { ConstString.加号, ConstString.Multexpr, ConstString.Arithexprprime }
            },
            new Production
            {
                Head = ConstString.Arithexprprime,
                Body = new [] { ConstString.减号, ConstString.Multexpr, ConstString.Arithexprprime }
            },
            new Production
            {
                Head = ConstString.Arithexprprime,
                Body = new string[] { }
            },
            new Production
            {
                Head = ConstString.Multexpr,
                Body = new [] { ConstString.Simpleexpr, ConstString.Multexprprime }
            },
            new Production
            {
                Head = ConstString.Multexprprime,
                Body = new [] { ConstString.乘号, ConstString.Simpleexpr, ConstString.Multexprprime }
            },
            new Production
            {
                Head = ConstString.Multexprprime,
                Body = new [] { ConstString.除号, ConstString.Simpleexpr, ConstString.Multexprprime }
            },
            new Production
            {
                Head = ConstString.Multexprprime,
                Body = new string[] { }
            },
            new Production
            {
                Head = ConstString.Simpleexpr,
                Body = new [] { ConstString.Id }
            },
            new Production
            {
                Head = ConstString.Simpleexpr,
                Body = new [] { ConstString.Num }
            },
            new Production
            {
                Head = ConstString.Simpleexpr,
                Body = new [] { ConstString.左括号, ConstString.Arithexpr, ConstString.右括号 }
            }
        };

        private Dictionary<string, FirstFollowSet> Nonterminals { get; } = new Dictionary<string, FirstFollowSet>
        {
            {
                ConstString.Program,
                new FirstFollowSet()
            },
            {
                ConstString.Compoundstmt,
                new FirstFollowSet()
            },
            {
                ConstString.Stmt,
                new FirstFollowSet()
            },
            {
                ConstString.Stmts,
                new FirstFollowSet()
            },
            {
                ConstString.Ifstmt,
                new FirstFollowSet()
            },
            {
                ConstString.Whilestmt,
                new FirstFollowSet()
            },
            {
                ConstString.Assgstmt,
                new FirstFollowSet()
            },
            {
                ConstString.Boolexpr,
                new FirstFollowSet()
            },
            {
                ConstString.Boolop,
                new FirstFollowSet()
            },
            {
                ConstString.Arithexpr,
                new FirstFollowSet()
            },
            {
                ConstString.Arithexprprime,
                new FirstFollowSet()
            },
            {
                ConstString.Multexpr,
                new FirstFollowSet()
            },
            {
                ConstString.Multexprprime,
                new FirstFollowSet()
            },
            {
                ConstString.Simpleexpr,
                new FirstFollowSet()
            }
        };



        //Dictionary<> 
        public LlTable()
        {

        }

        public ArrayList FirstSet()
        {
            ArrayList[] lstFirst = new ArrayList[Productions.Count];

            Queue queueFirst = new Queue();

            for (int i = 0; i < Productions.Count; i++)
            {
                lstFirst[i] = new ArrayList();
                Production production = Productions[i];
                //分成四步求FIRST集式 SELECT集 

                //1.如果 X 属于VT 则FIREST(X)={X}
                //if (SymbolSet.getInstance().IsInEndSet(production.Head))
                if (!Nonterminals.ContainsKey(production.Head))
                {
                    lstFirst[i].Add(production.Head);
                }
                else
                {
                    lstFirst[i] = new ArrayList();
                    //以此检测每一个右部
                    for (int j = 0; j < production.Body.Length; j++)
                    {
                        //2.如果 X 属于VN 且有产生式 X->a...,a属于VT,则 a 属于 FIREST(X)
                        string strFirstLetter = production.Body[j].Substring(0, 1);
                        if (!Nonterminals.ContainsKey(strFirstLetter))
                        {
                            if (!Tools.IsInList(strFirstLetter, lstFirst[i]))
                                lstFirst[i].Add(strFirstLetter);
                            //Console.Write(strFirstLetter + " x ");
                        }
                        //3.如果 X 推出 $ ,则 $ 属于 FIRST(X)
                        if (Tools.IsInList(production.Head, emptyList))
                        {
                            if (!Tools.IsInList("$", lstFirst[i]))
                            {
                                lstFirst[i].Add("$");
                                //if(i==1)Console.WriteLine("bbbbbbbbb");
                                //Console.Write("$ 0 ");
                            }
                        }

                        //4.如果产生式的右部的每一个字符都是非终结符集
                        if (production.Body.All(x =>).CheckRightItemAt(j))
                        {
                            string strRightTemp = production.StringRightItemAt(j);
                            if (CheckEmptySymbol(strRightTemp.Substring(0, 1)))
                            {
                                for (int l = 0; l < strRightTemp.Length; l++)
                                {
                                    bool bNext = true;
                                    if (CheckEmptySymbol(strRightTemp.Substring(l, 1)) && bNext)
                                    {
                                        string strQueueElement = "";
                                        if (l == strRightTemp.Length - 1)
                                        {
                                            strQueueElement = production.Head + production.StringRightItemAt(j).Substring(l, 1);
                                            //Console
                                        }
                                        else
                                            strQueueElement = production.Head + production.StringRightItemAt(j).Substring(l, 1) + "$";

                                        if (!Tools.IsInQueue(strQueueElement, queueFirst))
                                        {
                                            queueFirst.Enqueue(strQueueElement);
                                            Console.WriteLine(strQueueElement + "\txp");
                                        }
                                    }
                                    else
                                    {
                                        bNext = false;
                                        string strQueueElement = production.Head + production.StringRightItemAt(j).Substring(l, 1);
                                        if (!Tools.IsInQueue(strQueueElement, queueFirst))
                                        {
                                            queueFirst.Enqueue(strQueueElement);
                                            Console.WriteLine(strQueueElement + "\txp");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string strQueueElement = production.Head + production.StringRightItemAt(j).Substring(0, 1);
                                if (!Tools.IsInQueue(strQueueElement, queueFirst)) queueFirst.Enqueue(strQueueElement);
                                Console.WriteLine(strQueueElement + "\txp");
                            }
                        }
                        else
                        {
                            //假如产生式的右部并非都是非终结符
                            //那么取出第一个字符,假如是终结符,那么在上面的第2步里就已经计算过了,所以不用再计算
                            //假如是非终结符就假如到待取队列
                            if (SymbolSet.getInstance().IsInNotEndSet(production.StringRightItemAt(j).Substring(0, 1)))
                            {
                                string strQueueElement = production.Head + production.StringRightItemAt(j).Substring(0, 1);
                                if (!Tools.IsInQueue(strQueueElement, queueFirst)) queueFirst.Enqueue(strQueueElement);
                                Console.WriteLine(strQueueElement + "\txp");
                            }

                        }
                    }//end for j

                }//end if  IsInEndSet(X)
            }//end for i
            ArrayList lstItems = FinishFirst(lstFirst, queueFirst);
            return lstItems;
        }
    }
}
