
using Google.Cloud.PubSub.V1;
using Google.Protobuf;

const string ProjectId = "development-395212";
const string TopicId = "MyQueue";
const string SubscriptionId = "MyQueue-sub";

var topicName = new TopicName(ProjectId, TopicId);


Console.WriteLine($"Hello {topicName.TopicId}");

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gcp_credentials.json"));


var publisher = await PublisherClient.CreateAsync(topicName);

var pubSubMessage = new PubsubMessage()
{
    Data = ByteString.CopyFromUtf8("This is another sample message!"),
};

string messageId = await publisher.PublishAsync(pubSubMessage);

Console.WriteLine($"MessageId: {messageId}");

