using System.Linq;
using Api.Infrastructure.Context;
using Api.Infrastructure.Entities;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Api.GraphQL.Users
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class UserQuery
    {
        [UseFiltering]
        [UseProjection]
        public IQueryable<User> GetUsers([Service] ApplicationDbContext dbContext) => dbContext.Users.Include(x => x.Chats);
    }
}
