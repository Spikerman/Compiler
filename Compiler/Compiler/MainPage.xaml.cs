using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        //public static Pivot MainPivot { get; private set; }

        public MainPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<NavLink> NavLinks { get; } = new ObservableCollection<NavLink>
        {
            //new NavLink { Label = "Main", Symbol = Symbol.Home, DestPage = typeof(LibrariesPage) },
            //new NavLink { Label = "Test", Symbol = Symbol.Home },
            //new NavLink { Label = "获取文件夹访问权限", Symbol = Symbol.Library }
        };

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
    }
}
