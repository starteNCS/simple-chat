using System;
using System.Collections.Generic;

namespace Api.Infrastructure.Entities
{
    public class Chat
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }

        public virtual ICollection<User> Members { get; set; } = new HashSet<User>();
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
