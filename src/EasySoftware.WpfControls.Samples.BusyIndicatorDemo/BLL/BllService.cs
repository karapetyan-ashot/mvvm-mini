namespace EasySoftware.WpfControls.Samples.BusyIndicatorDemo.BLL
{
    public interface IBllService
    {
        Task DoSomeWork(int seconds, CancellationToken? cancellationToken = null);
        Task DoSomeWork(IProgress<int> progress, int seconds, CancellationToken? cancellationToken = null);
    }
    internal class BllService : IBllService
    {
        public async Task DoSomeWork(int seconds, CancellationToken? cancellationToken = null)
        {
            int currentSecond = 0;
            while (currentSecond < seconds * 10)
            {
                cancellationToken?.ThrowIfCancellationRequested();
                await Task.Delay(100, cancellationToken ?? CancellationToken.None);
                currentSecond++;
            }
        }

        public async Task DoSomeWork(IProgress<int> progress, int totalSteps, CancellationToken? cancellationToken = null)
        {
            int step = 0;
            while (step < totalSteps)
            {
                cancellationToken?.ThrowIfCancellationRequested();
                await Task.Delay(100, cancellationToken ?? CancellationToken.None);
                step++;
                if (step % 5 == 0)
                    progress.Report(step);
            }
        }
    }
}
