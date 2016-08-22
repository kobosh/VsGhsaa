using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ghsaa.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string MessageToPost { get; set; }
        public string From { get; set; }
        public DateTime DatePosted { get; set; }

    }

    public class Reply
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string ReplyFrom { get; set; }
        [Required]
        public string ReplyMessage { get; set; }
        public DateTime ReplyDateTime { get; set; }

    }
    public class MessageReplyViewModel
    {
    
    private List<MessageReply> _replies = new List<MessageReply>();
        public Reply Reply { get; set; }
        
        public Message Message {get;set;}
        
        public List<MessageReply> Replies
        {
            get { return _replies; }
            set { _replies = value; }
        }
        
        public PagedList.IPagedList<Message> Messages { get; set; }

        public class MessageReply
        {
            public int Id { get; set; }
            public int MessageId { get; set; }
            public string MessageDetails { get; set; }
            public string ReplyFrom { get; set; }
           
            public string ReplyMessage { get; set; }
            public DateTime ReplyDateTime { get; set; }
        }
        

    }
}
