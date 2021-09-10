using System;

namespace Api.Infrastructure.Entities
{
    public class Message
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }

        public Guid SenderId { get; set; }
        public User Sender { get; set; }

        public Guid? ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
