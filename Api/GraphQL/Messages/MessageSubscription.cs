using Api.Infrastructure.Entities;
using HotChocolate;
using HotChocolate.Types;

namespace Api.GraphQL.Messages
{
    [ExtendObjectType(OperationTypeNames.Subscription)]
    public class MessageSubscription
    {
        [Subscribe]
        [Topic("messageSent")]
        public Message SubscribeToMessages([EventMessage] Message message) => message;
    }
}
