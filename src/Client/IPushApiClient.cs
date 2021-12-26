using ExpoCommunityNotificationServer.Models;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Client
{
    public interface IPushApiClient
    {
        void SetToken(string token);

        Task<PushTicketResponse> SendPushAsync(params PushTicketRequest[] pushTicketRequest);

        Task<PushResceiptResponse> GetReceiptsAsync(PushReceiptRequest pushReceiptRequest);
    }
}
