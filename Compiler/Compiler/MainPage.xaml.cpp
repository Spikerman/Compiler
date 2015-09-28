//
// MainPage.xaml.cpp
// MainPage 类的实现。
//

#include "pch.h"
#include "MainPage.xaml.h"

using namespace Compiler;

using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Foundation::Collections;
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Windows::UI::Xaml::Controls::Primitives;
using namespace Windows::UI::Xaml::Data;
using namespace Windows::UI::Xaml::Input;
using namespace Windows::UI::Xaml::Media;
using namespace Windows::UI::Xaml::Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

MainPage::MainPage()
{
	InitializeComponent();

	_navLinks = ref new Platform::Collections::Vector<Compiler::DataModel::NavLink^>();
	auto navLink1 = ref new Compiler::DataModel::NavLink();
	navLink1->Label = "Main";
	navLink1->Symbol = Windows::UI::Xaml::Controls::Symbol::Home;
	_navLinks->Append(navLink1);
}


void Compiler::MainPage::Page_Loaded(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
}


void Compiler::MainPage::HamburgerButton_Click(Platform::Object^ sender, Windows::UI::Xaml::RoutedEventArgs^ e)
{
	splitView->IsPaneOpen = !splitView->IsPaneOpen;
}


void Compiler::MainPage::NavLinksList_ItemClick(Platform::Object^ sender, Windows::UI::Xaml::Controls::ItemClickEventArgs^ e)
{

}
