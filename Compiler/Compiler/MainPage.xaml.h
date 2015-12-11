//
// MainPage.xaml.h
// MainPage 类的声明。
//

#pragma once

#include "MainPage.g.h"
#include "NavLink.h"
#include "DynamicCommandBar.h"

namespace Compiler
{
	/// <summary>
	/// 可用于自身或导航至 Frame 内部的空白页。
	/// </summary>
	public ref class MainPage sealed
	{
	public:
		MainPage();
		//https://msdn.microsoft.com/zh-cn/library/windows/apps/hh700103.aspx:
		//如果你的类需要将序列容器传递到另一个 Windows 运行时组件，
		//请使用 Windows::Foundation::Collections::IVector<T> 作为参数或返回类型，
		//并使用 Platform::Collections::Vector<T> 作为具体实现。
		property Windows::Foundation::Collections::IVector<Compiler::DataModel::View::NavLink^>^ NavLinks
		{
			Windows::Foundation::Collections::IVector<Compiler::DataModel::View::NavLink^>^ get()
			{
				return _navLinks;
			}
		}
	private:
		Platform::Collections::Vector<Compiler::DataModel::View::NavLink^>^ _navLinks;
		void Page_Loaded(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void HamburgerButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e);
		void NavLinksList_ItemClick(Platform::Object^ sender, Windows::UI::Xaml::Controls::ItemClickEventArgs^ e);
	};
}
