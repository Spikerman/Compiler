using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Compiler
{
    public static class Lexical
    {
        private static string _outputString = string.Empty;
        //这个不得更改
        private const char Lf = '\n';

        private static void Output(string content)
        {
            _outputString += content;
        }

        public static async Task<LinkedList<Token>> Separate(StorageFile file)
        {
            LinkedList<Token> tokenList = new LinkedList<Token>();

            string tokenData = string.Empty;

            //等待构成符合词法的字符串
            string father = string.Empty;

            int lineNumber = 0;
            int columnNumber = 0;
            //标记是否为注释
            bool flag = false;
            //科学计数法
            bool eKey = false;

            string nativeCode = await FileIO.ReadTextAsync(file);

            foreach (char inputChar in nativeCode)
            {
                if (flag)
                {
                    if (inputChar == Lf)
                    {
                        tokenData = ConstString.两斜杠 + tokenData;
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber));
                        tokenData = string.Empty;
                        lineNumber++;
                        columnNumber = 0;
                        flag = false;
                    }
                    else
                    {
                        tokenData = tokenData + inputChar;
                    }
                }
                else if (eKey)
                {
                    columnNumber++;
                    if (inputChar == '+' || inputChar == '-')
                    {
                        if (tokenData[tokenData.Length - 1] == 'E' || tokenData[tokenData.Length - 1] == 'e')
                        {
                            tokenData = tokenData + inputChar;
                        }
                        else
                        {
                            tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber - 1));
                            tokenList.AddLast(new Token(inputChar.ToString(), lineNumber, columnNumber));
                            eKey = false;
                            tokenData = string.Empty;
                        }
                    }
                    else if (char.IsWhiteSpace(inputChar))
                    {
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber - 1));
                        eKey = false;
                        tokenData = string.Empty;
                        if (inputChar == Lf)
                        {
                            columnNumber = 0;
                            lineNumber++;
                        }
                    }
                    else if (inputChar == '*' || inputChar == '+' || inputChar == '-' || inputChar == '(' || inputChar == ')' || inputChar == '{' || inputChar == '}' || inputChar == ';')
                    {
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber - 1));
                        tokenList.AddLast(new Token(inputChar.ToString(), lineNumber, columnNumber));
                        eKey = false;
                        tokenData = string.Empty;
                    }
                    else if (inputChar == '<' || inputChar == '>' || inputChar == '=' || inputChar == '!' || inputChar == '/')
                    {
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber - 1));
                        father = inputChar.ToString();
                        eKey = false;
                        tokenData = string.Empty;
                    }
                    else
                    {
                        tokenData = tokenData + inputChar;
                    }
                }
                else if (!char.IsWhiteSpace(inputChar) && inputChar != ';' && inputChar != '{' && inputChar != '}' && inputChar != '+' && inputChar != '-' && inputChar != '*' && inputChar != '/' && inputChar != '(' && inputChar != ')' && inputChar != '<' && inputChar != '>' && inputChar != '=' && inputChar != '!')
                {
                    if ((inputChar == 'E' || inputChar == 'e') && IsIntnum(tokenData))
                    {
                        tokenData = tokenData + inputChar;
                        eKey = true;
                    }
                    else
                    {
                        columnNumber++;
                        tokenData = tokenData + inputChar;
                        if (father != string.Empty)
                        {
                            tokenList.AddLast(new Token(father, lineNumber, columnNumber));
                            father = string.Empty;
                        }
                    }
                }
                else if (inputChar == ';' || inputChar == '{' || inputChar == '}' || inputChar == '+' || inputChar == '-' || inputChar == '*' || inputChar == '(' || inputChar == ')')
                {
                    columnNumber++;
                    if (father != string.Empty)
                    {
                        tokenList.AddLast(new Token(father, lineNumber, columnNumber));
                        father = string.Empty;
                    }
                    if (tokenData != string.Empty)
                    {
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber));
                        tokenData = string.Empty;
                    }
                    tokenList.AddLast(new Token(inputChar.ToString(), lineNumber, columnNumber));

                }
                else if (inputChar == '<' || inputChar == '>' || inputChar == '=' || inputChar == '!' || inputChar == '/')
                {
                    columnNumber++;
                    if (tokenData != string.Empty)
                    {
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber));
                        tokenData = string.Empty;
                    }
                    if (father == string.Empty)
                    {
                        father = inputChar.ToString();
                    }
                    else if (tokenData == string.Empty)
                    {
                        father = father + inputChar;
                        if (father == ConstString.两斜杠)
                        {
                            flag = true;
                            father = string.Empty;
                        }
                        else
                        {
                            tokenList.AddLast(new Token(father, lineNumber, columnNumber));
                            father = string.Empty;
                        }
                    }
                    tokenData = string.Empty;
                }
                else
                {
                    if (father != string.Empty)
                    {
                        tokenList.AddLast(new Token(father, lineNumber, columnNumber));
                        father = string.Empty;
                    }
                    if (tokenData != string.Empty)
                    {
                        tokenList.AddLast(new Token(tokenData, lineNumber, columnNumber));
                    }
                    tokenData = string.Empty;
                    if (inputChar == Lf)
                    {
                        columnNumber = 0;
                        lineNumber++;
                    }
                }
            }

            return tokenList;
        }

        public static LinkedList<Token> GiveType(LinkedList<Token> tokenList, LinkedList<Symbol> symbolTable)
        {
            LinkedList<Token> tokenListWithType = new LinkedList<Token>();
            while (tokenList.Count > 0)
            {
                Token frontToken = tokenList.First.Value;
                string outTokenData = frontToken.Data;
                tokenList.RemoveFirst();
                if (outTokenData == ConstString.Int)
                {
                    frontToken.Type = ConstString.Type; // 类型
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.Char)
                {
                    frontToken.Type = ConstString.Type;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.Bool)
                {
                    frontToken.Type = ConstString.Type;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.加号)
                {
                    frontToken.Type = ConstString.Operator; // 操作符
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.减号)
                {
                    frontToken.Type = ConstString.Operator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.乘号)
                {
                    frontToken.Type = ConstString.Operator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.除号)
                {
                    frontToken.Type = ConstString.Operator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.大于号)
                {
                    frontToken.Type = ConstString.RelationOperator; // 判断符
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.小于号)
                {
                    frontToken.Type = ConstString.RelationOperator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.等号)
                {
                    frontToken.Type = ConstString.RelationOperator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.相等号)
                {
                    frontToken.Type = ConstString.RelationOperator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.大于等于号)
                {
                    frontToken.Type = ConstString.RelationOperator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.小于等于号)
                {
                    frontToken.Type = ConstString.RelationOperator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.不等号)
                {
                    frontToken.Type = ConstString.RelationOperator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.分号)
                {
                    frontToken.Type = ConstString.Terminator; // 终结符
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.左大括号)
                {
                    frontToken.Type = ConstString.Terminator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.右大括号)
                {
                    frontToken.Type = ConstString.Terminator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.左括号)
                {
                    frontToken.Type = ConstString.Terminator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.右括号)
                {
                    frontToken.Type = ConstString.Terminator;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.If)
                {
                    frontToken.Type = ConstString.KeyWord; // 关键字
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.Then)
                {
                    frontToken.Type = ConstString.KeyWord;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.Else)
                {
                    frontToken.Type = ConstString.KeyWord;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (outTokenData == ConstString.While)
                {
                    frontToken.Type = ConstString.KeyWord;
                    tokenListWithType.AddLast(frontToken);
                }
                else if (IsComment(outTokenData))
                {
                    frontToken.Type = ConstString.Comment; //注释
                    tokenListWithType.AddLast(frontToken);
                }
                else if (IsReal(outTokenData))
                {
                    frontToken.Type = ConstString.RealNumber; //实数
                    tokenListWithType.AddLast(frontToken);
                }
                else if (IsExponent(outTokenData))
                {
                    frontToken.Type = ConstString.Exponent; //指数
                    tokenListWithType.AddLast(frontToken);
                }
                else if (IsId(outTokenData))
                {
                    frontToken.Type = ConstString.Id;
                    tokenListWithType.AddLast(frontToken);
                    SymbolTable.init_symbol_table(new Symbol(frontToken.Data, frontToken.LineNumber, frontToken.ColumnNumber), symbolTable);
                }
                else if (IsIntnum(outTokenData))
                {
                    frontToken.Type = ConstString.Integer; // 整数
                    tokenListWithType.AddLast(frontToken);
                }
                else if (IsFraction(outTokenData))
                {
                    frontToken.Type = ConstString.Fraction; // 小数
                    tokenListWithType.AddLast(frontToken);
                }
                else
                {
                    frontToken.Type = ConstString.Error;
                    tokenListWithType.AddLast(frontToken);
                }
            }
            return tokenListWithType;
        }

        public static string TokenPrint(LinkedList<Token> tokenList)
        {
            _outputString = string.Empty;
            for (int i = tokenList.Count; i > 0; i--)
            {
                Token token = tokenList.First.Value;
                Output(token.Data);
                Output(" : ");
                Output(token.Type);
                Output(" [");
                Output(token.LineNumber.ToString());
                Output(" , ");
                Output(token.ColumnNumber.ToString());
                Output("]");
                Output(Environment.NewLine);
                tokenList.RemoveFirst();
                tokenList.AddLast(token);
            }
            return _outputString;
        }

        private static bool IsId(string str)
        {
            if (IsExponent(str))
            {
                return false;
            }
            bool key = true;
            for (int i = 1; i < str.Length; i++)
            {
                if (IsChar(str[i]) == false && IsDigit(str[i]) == false)
                {
                    key = false;
                }
            }
            return key && IsChar(str[0]);
        }

        private static bool IsComment(string out1)
        {
            return out1[0] == '/' && out1[1] == '/';
        }

        private static bool IsIntnum(string out1)
        {
            bool key = out1 != string.Empty;
            foreach (char t in out1)
            {
                if (IsDigit(t) == false)
                {
                    key = false;
                }
            }
            return key;
        }

        private static bool IsReal(string out1)
        {
            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;

            bool flag1 = false;
            bool flag2 = false;

            foreach (char t in out1)
            {
                if ((t == 'E' || t == 'e' || t == '.') && flag1 == false)
                {
                    flag1 = true;
                }
                else if ((t == 'E' || t == 'e' || t == '.') && flag1)
                {
                    flag2 = true;
                }

                if (flag1 == false)
                {
                    str1 = str1 + t;
                }
                else if (flag2 == false)
                {
                    str2 = str2 + t;
                }
                else
                {
                    str3 = str3 + t;
                }
            }
            if (flag1 == false)
            {
                return false;
            }
            if (flag2 == false)
            {
                if (IsIntnum(str1) && (IsFraction(str2) || IsExponent(str2)))
                {
                    return true;
                }
                return false;
            }
            if (IsIntnum(str1) && IsFraction(str2) && IsExponent(str3))
            {
                return true;
            }
            return false;
        }

        private static bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        private static bool IsChar(char ch)
        {
            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
        }

        private static bool IsExponent(string out1)
        {
            if (out1.Length < 2)
            {
                return false;
            }
            if (out1[0] != 'E' && out1[0] != 'e')
            {
                return false;
            }
            if (out1[1] != '+' && out1[1] != '-' && IsDigit(out1[1]) == false)
            {
                return false;
            }
            for (int i = 2; i < out1.Length; i++)
            {
                if (IsDigit(out1[i]) != true)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsFraction(string out1)
        {
            bool key = true;
            for (int i = 1; i < out1.Length; i++)
            {
                if (IsDigit(out1[i]) == false)
                {
                    key = false;
                }
            }
            return key && out1[0] == '.';
        }
    }
}

