using System;

namespace Compiler.DataModel.View
{
    public class NavLink
    {
        public string Label { get; set; }
        public Windows.UI.Xaml.Controls.Symbol Symbol { get; set; }
        public Type DestPage { get; set; }
        public object Arguments { get; set; }
    }
}
