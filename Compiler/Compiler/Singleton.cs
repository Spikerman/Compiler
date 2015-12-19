using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Compiler
{
    public sealed class Singleton
    {
        private Singleton() { }

        public static Singleton Instance { get; } = new Singleton();

        public StorageFile CodeFile { get; set; }
        public LinkedList<Symbol> SymbolTable { get; } = new LinkedList<Symbol>();
        public LinkedList<Token> TokenListWithType { get; set; }
        public LinkedList<Token> SemanticList { get; } = new LinkedList<Token>();
        public LinkedList<Token> ThreeAddressList { get; } = new LinkedList<Token>();
    }
}
