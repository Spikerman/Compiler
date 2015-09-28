#pragma once

namespace Compiler
{
	namespace DataModel
	{
		[Windows::UI::Xaml::Data::Bindable]
		public ref class NavLink sealed
		{
		public:
			property Platform::String^ Label
			{
				Platform::String^ get()
				{
					return _label;
				}
				void set(Platform::String^ value)
				{
					if (!_label->Equals(value))
					{
						_label = value;
					}
				}
			}

			property Windows::UI::Xaml::Controls::Symbol Symbol
			{
				Windows::UI::Xaml::Controls::Symbol get()
				{
					return _symbol;
				}
				void set(Windows::UI::Xaml::Controls::Symbol value)
				{
					if (!_symbol.Equals(value))
					{
						_symbol = value;
					}
				}
			}

			//property Platform::Type^ ClassType
			//{
			//	Platform::Type^ get()
			//	{
			//		return _classType;
			//	}
			//	void set(Platform::Type^ value)
			//	{
			//		if (!_classType->Equals(value))
			//		{
			//			_classType = value;
			//		}
			//	}
			//}

		private:
			Platform::String^ _label;
			Windows::UI::Xaml::Controls::Symbol _symbol;
			//Platform::Type^ _classType;
		};
	}
}