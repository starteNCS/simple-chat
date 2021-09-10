using System;
using System.Collections.Generic;

namespace Api.Infrastructure.Entities
{
    public class User
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? CreatedAt { get; set; } = DateTimeOffset.Now;

        public virtual ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();
    }
}
