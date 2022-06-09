# community-expo-push-notification-server-sdk
## Installation

Install package through NuGet - [Community.Expo.Server.SDK](https://www.nuget.org/packages/Community.Expo.Server.SDK/)  

To send requests without access token use version **1.1.2**   
Since version **1.2.0** access token is required

## Usage

### Send push notification

```cs
	IPushApiClient _client = new PushApiClient("your token here");
	PushTicketRequest pushTicketRequest = new PushTicketRequest()
            {
                PushTo = new List<string>() { ... },
                PushTitle = "TEST 1",
                PushBody = "TEST 1",
                PushChannelId = "test"
            };

	PushTicketResponse result = await _client.SendPushAsync(pushTicketRequest);
```
If no errors, then wait for a few moments for the notifications to be delivered.   
Then request receipts for each push ticket.  

Later, after the Expo push notification service has delivered the notifications to Apple or Google (usually quickly, but allow the the service up to 30 minutes when under load),  
a "receipt" for each notification is created.  
The receipts will be available for at least a day; stale receipts are deleted.

### Get push notification receipts

The ID of each receipt is sent back in the response "ticket" for each notification.  
In summary, sending a notification produces a ticket, which contains a receipt ID you later use to get the receipt.  

The receipts may contain error codes to which you must respond.  
In particular, Apple or Google may block apps that continue to send notifications to devices that have blocked notifications or have uninstalled your app.  
Expo does not control this policy and sends back the feedback from Apple and Google so you can handle it appropriately.

```cs
	IPushApiClient _client = new PushApiClient("your token here");
	PushReceiptRequest pushReceiptRequest = new PushReceiptRequest() { PushTicketIds = new List<string>() { ... } };
	PushReceiptResponse pushReceiptResult = await _client.GetReceiptsAsync(pushReceiptRequest);

	foreach (var pushReceipt in pushReceiptResult.PushTicketReceipts) 
	{
		// handle delivery status, etc
	}
```

## See Also

  * https://github.com/glyphard/expo-server-sdk-dotnet/
  * https://docs.expo.io/versions/latest/guides/push-notifications/
 