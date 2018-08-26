using System;
using System.Threading;
using System.Threading.Tasks;
namespace CashClubApp.Core.Helpers
{
    public class TaskCounter
    {
        public async Task RunCounter(CancellationToken token)
        {
            await Task.Run(async() =>
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(250);

            }, token);
        }
    }
}
