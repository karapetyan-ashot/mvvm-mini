using System;
using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoWindow
{
	public interface IDemoWindowViewModel : IWindowViewModel { }

	public class DemoWindowViewModel : WindowViewModelBase, IDemoWindowViewModel
	{
		public DemoWindowViewModel(IViewAdapter viewAdapter) : base(viewAdapter)
		{
			this.Title = "Demo window";
			this.WelcomeText = $"Text from view model {DateTime.Now.ToString("HH:mm:ss")}";
		}

		private string _welcomeText;
		public string WelcomeText
		{
			get => this._welcomeText;
			set => SetProperty(ref this._welcomeText, value);
		}
	}
}