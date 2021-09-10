using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.Context;
using Api.Infrastructure.Entities;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Messages
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class MessageMutation
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITopicEventSender _eventSender;

        public MessageMutation(ApplicationDbContext dbContext, ITopicEventSender eventSender)
        {
            _dbContext = dbContext;
            _eventSender = eventSender;
        }

        public async Task<Message> SendMessage(Guid chatId, Message message)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                throw new GraphQLException("Chat does not exist");
            }

            message.Id ??= Guid.NewGuid();
            message.ChatId ??= chatId;

            _dbContext.Messages.Add(message);
            if (await _dbContext.SaveChangesAsync() == 1)
            {
                await _eventSender.SendAsync("messageSent", message);
            }

            return _dbContext.Messages
                .Include(m => m.Sender)
                .FirstOrDefault(m => m.Id == message.Id);
        }
    }
}
