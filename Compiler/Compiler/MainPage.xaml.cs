using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Compiler.DataModel.View;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Compiler
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public StorageFile CodeFile { get; set; }
        public LinkedList<Symbol> SymbolTable { get; } = new LinkedList<Symbol>();
        public LinkedList<Token> TokenListWithType { get; set; }
        public LinkedList<Token> SemanticList { get; } = new LinkedList<Token>();
        public LinkedList<Token> ThreeAddressList { get; } = new LinkedList<Token>();

        public ObservableCollection<NavLink> NavLinks { get; } = new ObservableCollection<NavLink>
        {
            //new NavLink { Label = "Main", Symbol = Symbol.Home, DestPage = typeof(LibrariesPage) },
            //new NavLink { Label = "Test", Symbol = Symbol.Home },
            //new NavLink { Label = "获取文件夹访问权限", Symbol = Symbol.Library }
        };

        public MainPage()
        {
            InitializeComponent();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {
            NavLink navLink = e.ClickedItem as NavLink;
            Debug.Assert(navLink != null, "navLink != null");
            if (navLink.Label == "获取文件夹访问权限")
            {
            }
            else if (navLink.Label == "Test")
            {
                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                //ScenarioFrame.Navigate(navLink.DestPage);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //MainPivot = MainPivotInstance;
            //ScenarioFrame.Navigate(typeof(LibrariesPage));
        }

        private async void OpenAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".txt");
            CodeFile = await openPicker.PickSingleFileAsync();
            if (CodeFile == null)
            {
                return;
            }
            LinkedList<Token> tokenList = await Lexical.Separate(CodeFile);
            //SymbolTable在这里被增删内容
            TokenListWithType = Lexical.GiveType(tokenList, SymbolTable);
        }

        private void 词法分析AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            演示TextBox.Text = Lexical.TokenPrint(TokenListWithType);
        }

        private void 语法分析AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //TokenListWithType列表内的Token会在这个函数内，被全部删除。
            //所以复制一份：
            LinkedList<Token> in1 = new LinkedList<Token>();
            foreach (Token item in TokenListWithType)
            {
                in1.AddLast(item);
            }
            //SemanticList在这里产生。
            演示TextBox.Text = LlParser.Parser(in1, SemanticList);
        }

        private void 三地址AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //先在函数外面复制一份
            foreach (Token item in SemanticList)
            {
                ThreeAddressList.AddLast(item);
            }
            int i = 0;
            演示TextBox.Text = ThreeAddress.semantic_go(ThreeAddressList, ref i);
        }

        private void 属性文法AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //先在函数外面复制一份
            LinkedList<Token> in1 = new LinkedList<Token>();
            foreach (Token item in SemanticList)
            {
                in1.AddLast(item);
            }
            演示TextBox.Text = Semantic.semantic_go(in1, SymbolTable);
            演示TextBox.Text += Environment.NewLine;
            演示TextBox.Text += Compiler.SymbolTable.print_symbol_table(SymbolTable);
        }
    }
}
