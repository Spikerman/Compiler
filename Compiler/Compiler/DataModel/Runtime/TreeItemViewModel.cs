using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Compiler.DataModel.Runtime
{

    public class TreeItemViewModel
    {
        public static readonly Windows.UI.Xaml.Controls.Symbol LibrarySymbol = Windows.UI.Xaml.Controls.Symbol.Library;
        public static readonly Windows.UI.Xaml.Controls.Symbol CategorySymbol = Windows.UI.Xaml.Controls.Symbol.List;
        public static readonly Windows.UI.Xaml.Controls.Symbol ItemSymbol = Windows.UI.Xaml.Controls.Symbol.NewWindow;
        public static readonly Windows.UI.Xaml.Controls.Symbol ErrorSymbol = Windows.UI.Xaml.Controls.Symbol.Cancel;

        public string Text { get; set; }

        public Windows.UI.Xaml.Controls.Symbol StatusSymbol { get; set; }
        public ObservableCollection<TreeItemViewModel> Children { get; set; } = new ObservableCollection<TreeItemViewModel>();


        #region Children

        //public IEnumerable<TreeItemViewModel> Children
        //{
        //    get
        //    {
        //        List<TreeItemViewModel> children = new List<TreeItemViewModel>();

        //        //foreach (StorageFolder item in StorageFolderList)
        //        //{
        //        //    if (StatusSymbol != LibrarySymbol)
        //        //    {
        //        //        //仅当文件夹下包含 项目属性.xml 文件，才显示该文件夹为 项目，否则就是分类。
        //        //        if (XmlItemController.GetInstance().IsItemExists(item) != null)
        //        //        {
        //        //            //说明这个文件夹是项目。
        //        //            if (StatusSymbol == 0)
        //        //            {
        //        //                StatusSymbol = ItemSymbol;
        //        //                continue;
        //        //            }
        //        //            else if (StatusSymbol == CategorySymbol)
        //        //            {
        //        //                StatusSymbol = ErrorSymbol;
        //        //                children.Clear();
        //        //                break;
        //        //            }
        //        //            else
        //        //            {
        //        //                continue;
        //        //            }
        //        //        }
        //        //        else
        //        //        {
        //        //            //说明这个文件夹是分类。
        //        //            if (StatusSymbol == 0)
        //        //            {
        //        //                StatusSymbol = CategorySymbol;
        //        //            }
        //        //            else if (StatusSymbol == ItemSymbol)
        //        //            {
        //        //                StatusSymbol = ErrorSymbol;
        //        //                children.Clear();
        //        //                break;
        //        //            }
        //        //        }
        //        //    }

        //        //    IReadOnlyList<StorageFolder> itemStorageFolderList = AsyncHelper.RunSync(() => item.GetFoldersAsync());

        //        //    IEnumerable<StorageFolder> storageFolderEnumerable = itemStorageFolderList.Where(x => PathHelper.系统文件夹枚举.Contains(x.Path.Substring(3)) == false);
        //        //    foreach (StorageFolder itemStorageFolder in storageFolderEnumerable)
        //        //    {
        //        //        TreeItemViewModel tmp = new TreeItemViewModel
        //        //        {
        //        //            Text = itemStorageFolder.Name,
        //        //            StorageFolderList = new List<StorageFolder>
        //        //                {
        //        //                    itemStorageFolder
        //        //                },
        //        //            Path = Path + "\\" + itemStorageFolder.Name
        //        //        };
        //        //        children.AddAndReduce(tmp);
        //        //    }
        //        //}

        //        return children;
        //    }
        //}

        #endregion
    }
}
