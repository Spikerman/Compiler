using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Compiler.DataModel.View;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Compiler
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly Singleton _instance = Singleton.Instance;

        public MainPage()
        {
            InitializeComponent();
        }
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void OpenAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".txt");
            _instance.CodeFile = await openPicker.PickSingleFileAsync();
            if (_instance.CodeFile == null)
            {
                return;
            }
            LinkedList<Token> tokenList = await Lexical.Separate(_instance.CodeFile);
            //SymbolTable在这里被增删内容
            _instance.TokenListWithType = Lexical.GiveType(tokenList, _instance.SymbolTable);

            MainWebView.Visibility = Visibility.Visible;
            ScenarioFrame.Visibility = Visibility.Collapsed;
            演示TextBox.Visibility = Visibility.Collapsed;
            高亮();
        }

        private void 词法分析AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.Visibility = Visibility.Collapsed;
            ScenarioFrame.Visibility = Visibility.Collapsed;
            演示TextBox.Visibility = Visibility.Visible;
            演示TextBox.Text = Lexical.TokenPrint(_instance.TokenListWithType);
        }

        private void 语法分析AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.Visibility = Visibility.Collapsed;
            ScenarioFrame.Visibility = Visibility.Visible;
            演示TextBox.Visibility = Visibility.Collapsed;
            ScenarioFrame.Navigate(typeof(TreeViewPage));
        }

        private void 三地址AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.Visibility = Visibility.Collapsed;
            ScenarioFrame.Visibility = Visibility.Collapsed;
            演示TextBox.Visibility = Visibility.Visible;
            //先在函数外面复制一份
            foreach (Token item in _instance.SemanticList)
            {
                _instance.ThreeAddressList.AddLast(item);
            }
            int i = 0;
            演示TextBox.Text = ThreeAddress.semantic_go(_instance.ThreeAddressList, ref i);
        }

        private void 属性文法AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.Visibility = Visibility.Collapsed;
            ScenarioFrame.Visibility = Visibility.Collapsed;
            演示TextBox.Visibility = Visibility.Visible;
            //先在函数外面复制一份
            LinkedList<Token> in1 = new LinkedList<Token>();
            foreach (Token item in _instance.SemanticList)
            {
                in1.AddLast(item);
            }
            演示TextBox.Text = Semantic.semantic_go(in1, _instance.SymbolTable);
            演示TextBox.Text += Environment.NewLine;
            演示TextBox.Text += SymbolTable.print_symbol_table(_instance.SymbolTable);
        }

        private void 高亮()
        {
            string css = "body{font-family: Consolas}span."
                + ConstString.KeyWord + "{color:#F75C5C;}span."
                + ConstString.Id + "{color:#9454D4;}span."
                + ConstString.RelationOperator + "{color:#54D494;}span."
                + ConstString.Integer + "{color:#B9AE1D;}span."
                + ConstString.Terminator + "{color:#000;}span."
                + ConstString.Operator + "{color:#ff007f;}span."
                + ConstString.Exponent + "{color:#000;}span."
                + ConstString.Comment + "{color:#008000;}span."
                + ConstString.RealNumber + "{color:#9454D4;}span."
                + ConstString.Fraction + "{color:#40e0d0;}";

            string header = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><title>Code</title><style>" + css + "</style></head><body><p>";
            string output = header;
            int lastLineNumber = 0;
            string newLine = "<br/>";
            string endHtml = "</p></body></html>";
            foreach (Token token in _instance.TokenListWithType)
            {
                if (token.LineNumber > lastLineNumber)
                {
                    output += newLine;
                }
                string line = "<span class=\"" + token.Type + "\">" + token.Data + "</span>&nbsp;";
                output += line;
                lastLineNumber = token.LineNumber;
            }
            output = output.Replace("&nbsp;<span class=\"terminator\">;</span>&nbsp;", "<span class=\"terminator\">;</span>&nbsp;");
            output += endHtml;
            MainWebView.NavigateToString(output);
        }
    }
}
