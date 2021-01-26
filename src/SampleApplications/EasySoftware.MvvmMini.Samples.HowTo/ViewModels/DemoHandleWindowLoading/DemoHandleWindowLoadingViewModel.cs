using System;
using System.Threading.Tasks;

using EasySoftware.MvvmMini.Core;
using EasySoftware.MvvmMini.Samples.HowTo.Services;

namespace EasySoftware.MvvmMini.Samples.HowTo.ViewModels.DemoHandleWindowLoading
{
	public interface IDemoHandleWindowLoadingViewModel : IWindowViewModel { }

	public class DemoHandleWindowLoadingViewModel : WindowViewModelBase, IDemoHandleWindowLoadingViewModel
	{
		IDateTimeService _dateTimeService;

		public DemoHandleWindowLoadingViewModel(IViewAdapter viewAdapter, IDateTimeService dateTimeService) : base(viewAdapter)
		{
			this._dateTimeService = dateTimeService;

			this.Title = "Handle window loaded";
		}

		private string _currentState;
		public string CurrentState
		{
			get => this._currentState;
			set => SetProperty(ref this._currentState, value);
		}

		protected override async Task Loaded()
		{
			this.IsBusy = true;

			DateTime dateTime = await this._dateTimeService.GetDate();
			this.CurrentState = $"Loaded asynchronously:  {dateTime}";

			this.IsBusy = false;
		}
	}
}
