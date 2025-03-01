using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.Accounts
{
	public class MessageModel
	{
		public List<MailboxAddress> To { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }

		public byte[] Attachment { get; set; }
		public string InvitedBy { get; set; }

		public MessageModel(IEnumerable<string> to, string subject, string content, byte[] attachment, string destinatary, string sourceSender)
		{
			To = new List<MailboxAddress>();
			To.AddRange(to.Select(x => new MailboxAddress(destinatary, x)));
			Subject = subject;
			Content = content;
			Attachment = attachment;
			InvitedBy = sourceSender;
		}
	}
}
