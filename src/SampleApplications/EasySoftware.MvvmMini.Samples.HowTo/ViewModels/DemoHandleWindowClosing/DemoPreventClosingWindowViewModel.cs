using System.ComponentModel;

using EasySoftware.MvvmMini.Core;

namespace EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowClosing
{
	public interface IDemoHandleWindowClosingViewModel : IWindowViewModel { }

	public class DemoHandleWindowClosingViewModel : WindowViewModelBase, IDemoHandleWindowClosingViewModel
	{
		public DemoHandleWindowClosingViewModel(IViewAdapter viewAdapter) : base(viewAdapter)
		{
			this.Title = "Prevent closing window demo";
		}

		private string _confirmation;
		public string Confirmation
		{
			get { return _confirmation; }
			set
			{
				if (_confirmation != value)
				{
					_confirmation = value;
					RaisePropertyChanged("Confirmation");
				}
			}
		}

		public override void OnClosing(CancelEventArgs e)
		{
			if (!this.CanClose())
				e.Cancel = true;
		}

		protected override bool CanClose()
		{
			return this.Confirmation == "close";
		}
	}
}