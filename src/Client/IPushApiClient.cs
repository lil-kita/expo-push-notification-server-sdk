using ExpoCommunityNotificationServer.Models;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    public interface IPushApiClient
    {
        Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest);

        Task<PushResceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest);
    }
}
