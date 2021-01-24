using System;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.WpfDemo.ViewModels.DemoWindowWithParams
{
	public interface IDemoWindowWithParamsViewModel: IWindowViewModel { }

	public class DemoWindowWithParamsViewModel : WindowViewModelBase, IDemoWindowWithParamsViewModel
	{
		public DemoWindowWithParamsViewModel(IViewAdapter viewAdapter, DateTime dateTime) : base(viewAdapter)
		{
			this.Title = "Demo window with params";
			this.WelcomeText = $"Parameter passed to view model: {dateTime.ToString("HH:mm:ss")}";
		}

		private string _welcomeText;
		public string WelcomeText
		{
			get => this._welcomeText;
			set => SetProperty(ref this._welcomeText, value);
		}
	}
}