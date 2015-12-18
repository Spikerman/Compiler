using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace 数据管理.DataModel.Runtime
{
    public class LibraryBrowsePageDataContext
    {
        #region TreeItems

        public ObservableCollection<TreeItemViewModel> TreeItems { get; set; }

        #endregion

        private readonly LibraryListItem _libraryListItem;

        public LibraryBrowsePageDataContext(LibraryListItem libraryListItem)
        {
            TreeItems = new ObservableCollection<TreeItemViewModel>();
            _libraryListItem = libraryListItem;
        }

        public async Task Load(完成后执行 task)
        {
            TreeItems.Clear();
            List<string> paths = _libraryListItem.Paths;
            List<StorageFolder> storageFolderList = new List<StorageFolder>();
            foreach (string item in paths)
            {
                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(item);
                storageFolderList.Add(storageFolder);
            }
            TreeItems.Add(new TreeItemViewModel
            {
                Text = _libraryListItem.Name,
                Path = _libraryListItem.Name,
                StorageFolderList = storageFolderList,
                StatusSymbol = TreeItemViewModel.LibrarySymbol
            });
            task?.Invoke();
        }
    }
}
