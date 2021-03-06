﻿using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini
{
	public abstract class WindowViewModelBase : ClosableViewModelBase, IWindowViewModel
	{
      protected WindowViewModelBase(IViewAdapter viewAdapter) : base(viewAdapter) { }

		public void Show()
		{
			this._viewAdapter.Show();
		}
	}
}