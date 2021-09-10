using Api.GraphQL;
using Api.GraphQL.Chats;
using Api.GraphQL.Messages;
using Api.GraphQL.Users;
using Api.Infrastructure.Context;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=./chats.db");
            });

            services.AddInMemorySubscriptions();

            services
                .AddGraphQLServer()
                .AddQueryType()
                    .AddTypeExtension<ChatQuery>()
                    .AddTypeExtension<UserQuery>()
                .AddMutationType()
                    .AddTypeExtension<ChatMutation>()
                    .AddTypeExtension<UserMutation>()
                    .AddTypeExtension<MessageMutation>()
                .AddSubscriptionType()
                    .AddTypeExtension<MessageSubscription>()
                .AddProjections()
                .AddFiltering();

            services.AddCors();
            services.AddAuthentication();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UsePlayground(new PlaygroundOptions
            {
                Path = "/playground",
                QueryPath = "/graphql"
            });

            app.UseWebSockets();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
