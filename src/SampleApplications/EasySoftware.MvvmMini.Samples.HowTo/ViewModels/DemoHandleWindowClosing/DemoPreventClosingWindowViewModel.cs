using System.ComponentModel;
using System.Threading.Tasks;

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

		public override Task OnClosing(CancelEventArgs e)
		{
			if (!this.CanClose())
				e.Cancel = true;

			return base.OnClosing(e);
		}

		protected override bool CanClose()
		{
			return this.Confirmation == "close";
		}
	}
}