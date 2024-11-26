using RestSharp;
using VipTest.Utlity;

namespace VipTest.Notifications.Utli;

public class OneSignalService
{
    public ILogger<OneSignalService> _logger;

    public OneSignalService(ILogger<OneSignalService> logger)
    {
        _logger = logger;
    }

    public static void SendNotification(string title, string description, string[] receiverId)
    {
        var configs = ConfigProvider.config;
        var client = new RestClient(configs["OneSignal:PushNotificationsUrl"]);
        var request = new RestRequest(configs["OneSignal:PushNotificationsUrl"], Method.POST);

        request.AddHeader("Authorization", configs["OneSignal:AppKey"]);
        request.AddHeader("Content-Type", "application/json");
        // request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");

        try
        {
            var body = new
            {
                app_id = configs["OneSignal:AppId"],
                headings = new { en = title, ar = title },
                contents = new { en = description, ar = description },
                include_external_user_ids = receiverId
            };

            request.AddJsonBody(body);
            client.Execute(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void SendNotificationForDriver(string title, string description, string[] driverId)
    {
        var configs = ConfigProvider.config;
        var client = new RestClient(configs["OneSignal_Delegator:PushNotificationsUrl"]);
        var request = new RestRequest(configs["OneSignal_Delegator:PushNotificationsUrl"], Method.POST);

        request.AddHeader("Authorization", configs["OneSignal_Delegator:AppKey"]);
        request.AddHeader("Content-Type", "application/json");
        // request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");

        try
        {
            var body = new
            {
                app_id = configs["OneSignal_Delegator:AppId"],
                headings = new { en = title, ar = title },
                contents = new { en = description, ar = description },
                include_external_user_ids = driverId
            };

            request.AddJsonBody(body);
            client.Execute(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}