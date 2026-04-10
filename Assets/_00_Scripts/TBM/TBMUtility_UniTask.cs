using System.Threading;

namespace TenMaker.Utility
{
    public partial class TBMUtility
    {
        public static void CancelAndDispose(this CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}