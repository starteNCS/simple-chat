using System.Linq;
using Api.Infrastructure.Context;
using Api.Infrastructure.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Chats
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ChatQuery
    {
        [UseFiltering]
        public IQueryable<Chat> GetChats([Service] ApplicationDbContext dbContext) => dbContext.Chats
            .Include(c => c.Members)
            .Include(c => c.Messages);
    }
}
