using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Compiler.DataModel.Runtime;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace Compiler
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class TreeViewPage : Page
    {
        public TreeViewPage()
        {
            this.InitializeComponent();
        }

        private readonly Singleton _instance = Singleton.Instance;
        public ObservableCollection<TreeItemViewModel> TreeItems { get; } = new ObservableCollection<TreeItemViewModel>();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //if (e.Parameter != null)
            {
                //TokenListWithType列表内的Token会在这个函数内，被全部删除。
                //所以复制一份：
                LinkedList<Token> in1 = new LinkedList<Token>();
                foreach (Token item in _instance.TokenListWithType)
                {
                    in1.AddLast(item);
                }
                //SemanticList在这里产生。
                string log = LlParser.Parser(in1, _instance.SemanticList, TreeItems);
            }

            base.OnNavigatedTo(e);
        }
    }
}
