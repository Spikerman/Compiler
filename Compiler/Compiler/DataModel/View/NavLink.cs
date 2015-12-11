using System;
using Windows.UI.Xaml.Controls;

namespace Compiler.DataModel.View
{
    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public Type DestPage { get; set; }
        public object Arguments { get; set; }
    }
}
