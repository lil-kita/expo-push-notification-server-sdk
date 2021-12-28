using ExpoCommunityNotificationServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpoCommunityNotificationServer.Helpers
{
    internal static class RequestHelper
    {
        public static bool IsReceiptRequestInValidRange(this PushReceiptRequest request) =>
            request.PushTicketIds != null
                && request.PushTicketIds.Count > 0
                && request.PushTicketIds.Count <= 1000;

        public static bool IsPushMessagesInValidRange(this PushTicketRequest[] pushTicketRequest)
        {
            if (pushTicketRequest == null)
            {
                return false;
            }

            int count = pushTicketRequest.Select(x => x.PushTo.Count).Sum();
            if (count > 0 && count <= 100)
            {
                return true;
            }
            return false;
        }
    }
}
