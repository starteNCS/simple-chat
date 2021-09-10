using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.Context;
using Api.Infrastructure.Entities;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Chats
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class ChatMutation
    {
        public async Task<Chat> CreateChat([Service] ApplicationDbContext dbContext, Chat chat)
        {
            chat.Id ??= Guid.NewGuid();
            chat.CreatedAt ??= DateTimeOffset.Now;

            if (dbContext.Chats.Any(c => c.Id == chat.Id))
            {
                throw new GraphQLException("Chat already exists");
            }

            dbContext.Chats.Add(chat);
            await dbContext.SaveChangesAsync();
            return chat;
        }

        public async Task<Chat> JoinChat([Service] ApplicationDbContext dbContext, Guid chatId, Guid userId)
        {
            var chat = await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
            {
                throw new GraphQLException("Chat does not exist");
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new GraphQLException("User does not exist");
            }

            if (chat.Members.Contains(user))
            {
                return chat;
            }

            chat.Members.Add(user);
            dbContext.Chats.Update(chat);
            await dbContext.SaveChangesAsync();

            return chat;
        }
    }
}
