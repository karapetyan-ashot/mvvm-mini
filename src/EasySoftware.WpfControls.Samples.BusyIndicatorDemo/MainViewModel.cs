using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using EasySoftware.MvvmMini;
using EasySoftware.MvvmMini.Core;
using EasySoftware.WpfControls.Samples.BusyIndicatorDemo.BLL;

namespace EasySoftware.WpfControls.Samples.BusyIndicatorDemo
{
    public interface IMainViewModel : IWindowViewModel { }
    internal class MainViewModel : WindowViewModelBase, IMainViewModel
    {
        private readonly IBllService _bllService;
        private CancellationTokenSource? _cancellationTokenSource;

        public MainViewModel(IViewAdapter viewAdapter, IBllService bllService) : base(viewAdapter)
        {
            Logs = new ObservableCollection<string>();

            IsIndeterminate = true;
            ShowMessage = true;

            ShowBusyCommand = new RelayCommand(ShowBusy);
            TestCommand = new RelayCommand(Test);
            _bllService = bllService;
        }

        public IRelayCommand ShowBusyCommand { get; }

        private IRelayCommand _cancelBusyCommand;
        public IRelayCommand CancelBusyCommand
        {
            get => _cancelBusyCommand;
            set => SetProperty(ref _cancelBusyCommand, value);
        }

        public IRelayCommand TestCommand { get; }

        public ObservableCollection<string> Logs { get; }



        private string? _busyMessage;
        public string? BusyMessage
        {
            get => _busyMessage;
            set => SetProperty(ref _busyMessage, value);
        }

        private string? _progressMessage;
        public string? ProgressMessage
        {
            get => _progressMessage;
            set => SetProperty(ref _progressMessage, value);
        }

        private double? _progressValue;
        public double? ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        private bool _showMessage;
        public bool ShowMessage
        {
            get => _showMessage;
            set => SetProperty(ref _showMessage, value);
        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set => SetProperty(ref _isIndeterminate, value);
        }

        private bool _showProgressMessage;
        public bool ShowProgressMessage
        {
            get => _showProgressMessage;
            set => SetProperty(ref _showProgressMessage, value);
        }

        public bool ShowCancelButton
        {
            get => CancelBusyCommand != null;
            set
            {
                if (value)
                {
                    CancelBusyCommand = new RelayCommand(CancelBusy);
                }
                else
                {
                    CancelBusyCommand = null;
                }
            }
        }

        private async Task ShowBusy()
        {
            IsBusy = true;
            BusyMessage = null;
            ProgressMessage = null;

            if (ShowMessage)
                BusyMessage = "I Am Busy...";


            ProgressValue = null;

            if (IsIndeterminate)
            {
                if (ShowCancelButton)
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    try
                    {
                        await _bllService.DoSomeWork(3, _cancellationTokenSource.Token);
                        LogMessage("Work completed");
                    }
                    catch (OperationCanceledException)
                    {
                        LogMessage("Work cancelled");
                    }
                    finally
                    {
                        _cancellationTokenSource.Dispose();
                        _cancellationTokenSource = null;
                    }
                }
                else
                    await _bllService.DoSomeWork(3);
            }
            else
            {
                var progress = new Progress<int>(step =>
                {
                    ProgressValue = step * 100.0 / 30;
                    if (ShowProgressMessage)
                        ProgressMessage = $"Completed {step} of 30 steps";
                });

                await _bllService.DoSomeWork(progress, 30);
            }

            IsBusy = false;
        }

        private async Task CancelBusy()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                LogMessage("Cancellation requested");
            }

            IsBusy = false;
        }


        private async Task Test()
        {
            LogMessage("Run Test");
        }

        private void LogMessage(string message)
        {
            var msg = $"{DateTime.Now.ToString("hh:mm:ss.fff")}: {message}";
            Logs.Insert(0, msg);
        }
    }
}