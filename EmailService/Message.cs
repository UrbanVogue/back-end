using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MimeKit;

namespace EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> addresses, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(addresses.Select(x => new MailboxAddress("email", x)));

            Subject = subject;
            Content = content;
        }
    }
}
