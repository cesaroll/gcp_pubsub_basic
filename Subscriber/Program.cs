using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace PubSubConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string projectId = "development-395212";
            string subscriptionId = "MyQueue-sub";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gcp_credentials.json"));

            var subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);
            var subscriber = await SubscriberClient.CreateAsync(subscriptionName);

            int messageCount = 0;

            Task startTask = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) =>
            {
                string text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
                Console.WriteLine($"Message: {message.MessageId}: {text}");
                Interlocked.Increment(ref messageCount);
                return Task.FromResult(SubscriberClient.Reply.Ack);
            });

            Console.WriteLine("Listening for messages. Press any key to exit...");
            Console.ReadKey();

            await subscriber.StopAsync(CancellationToken.None);
        }
    }
}
