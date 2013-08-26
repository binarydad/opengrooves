using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenGrooves.Web.Models
{
    public class MessageModel
    {
        public Guid MessageId { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid RecipUserId { get; set; }

        public UserModel Sender { get; set; }
        public IEnumerable<MessageRecipientRelation> Recipients { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Date { get; set; }
    }

    public class MessageRecipientRelation
    {
        public Guid UserId { get; set; }
        public Guid MessageId { get; set; }
        public bool IsRead { get; set; }
        public UserModel User { get; set; }
        public MessageModel Message { get; set; }

    }
}
