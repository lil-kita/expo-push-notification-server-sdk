# expo-server-sdk-dotnet
### created by [Ashley Messer](https://github.com/glyphard)
### edited by Mikita Slaunikau


## Usage


```cs

using expo_server_sdk_dotnet.Client;
using expo_server_sdk_dotnet.Models;

	private PushApiClient _expoClient = new PushApiClient("your token here");
	PushTicketRequest pushTicketRequest1 = new PushTicketRequest()
            {
                PushTo = new List<string>() { ... },
                PushTitle = "TEST 1",
                PushBody = "TEST 1",
                PushChannelId = "test"
            };
        PushTicketRequest pushTicketRequest2 = new PushTicketRequest()
            {
                PushTo = new List<string>() { ... },
                PushTitle = "TEST 2",
                PushBody = "TEST 2",
                PushChannelId = "test"
            };

	PushTicketResponse result = await expoSDKClient.PushSendAsync(
		new List<PushTicketRequest>() { pushTicketRequest1, pushTicketRequest2 }
		);

	if (result?.PushTicketErrors?.Count() > 0) 
	{
		foreach (var error in result.PushTicketErrors) 
		{
			// handle errors
		}
	}

//If no errors, then wait for a few moments for the notifications to be delivered
//Then request receipts for each push ticket

...

// Later, after the Expo push notification service has delivered the
// notifications to Apple or Google (usually quickly, but allow the the service
// up to 30 minutes when under load), a "receipt" for each notification is
// created. The receipts will be available for at least a day; stale receipts
// are deleted.
//
// The ID of each receipt is sent back in the response "ticket" for each
// notification. In summary, sending a notification produces a ticket, which
// contains a receipt ID you later use to get the receipt.
//
// The receipts may contain error codes to which you must respond. In
// particular, Apple or Google may block apps that continue to send
// notifications to devices that have blocked notifications or have uninstalled
// your app. Expo does not control this policy and sends back the feedback from
// Apple and Google so you can handle it appropriately.
	
	PushReceiptRequest pushReceiptRequest = new PushReceiptRequest() { PushTicketIds = new List<string>() { ... } };
	PushReceiptResponse pushReceiptResult = await expoSDKClient.PushGetReceiptsAsync(pushReceiptRequest);

	if (pushReceiptResult?.ErrorInformations?.Count() > 0) 
	{
		foreach (var error in result.ErrorInformations) 
		{
			// handle errors
		}
	}
	foreach (var pushReceipt in pushReceiptResult.PushTicketReceipts) 
	{
		// handle delivery status, etc
	}
```

## See Also

  * https://github.com/glyphard/expo-server-sdk-dotnet/
  * https://docs.expo.io/versions/latest/guides/push-notifications/
 
