using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.Context;
using Api.Infrastructure.Entities;
using HotChocolate;
using HotChocolate.Types;

namespace Api.GraphQL.Users
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class UserMutation
    {
        public async Task<User> AddUser([Service] ApplicationDbContext dbContext, User user)
        {
            user.Id ??= Guid.NewGuid();
            user.CreatedAt ??= DateTimeOffset.Now;

            if (dbContext.Users.Any(x => x.Id == user.Id || x.Email == user.Email))
            {
                throw new GraphQLException("The User already exists");
            }

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
    }
}
