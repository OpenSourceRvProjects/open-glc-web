using Microsoft.Extensions.Configuration;
using MimeKit;
using OpenGLC.Data.Entities;
using OpenGLC.Models.Accounts;
using MailKit.Net.Smtp;

namespace OpenGLC.Emailer
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailConfigurationModel _emailConfig;
		private IConfiguration _configuration;
		public EmailSender(IConfiguration configuration, EmailConfigurationModel emailConfig)
		{
			_configuration = configuration;
			_emailConfig = emailConfig;

		}

		public void SendEmail(MessageModel message, User user, Guid userID)
		{
			var emailMessage = CreateEmailMessage(message);
			Send(emailMessage);
		}

		private MimeMessage CreateEmailMessage(MessageModel message)
		{
			var emailMessage = new MimeMessage();
			var titleMessages = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "qa" ? "TEST-OpenGLC App" : "OpenGLC App";
			emailMessage.From.Add(new MailboxAddress(titleMessages, _emailConfig.From));
			emailMessage.To.AddRange(message.To);
			emailMessage.Subject = message.Subject;
			var emailBody = new MimeKit.BodyBuilder
			{
				HtmlBody = message.Content,

			};
			//emailBody.Attachments.Add(" " + message.InvitedBy + ".pdf", message.Attachment);

			emailMessage.Body = emailBody.ToMessageBody();

			//https://stackoverflow.com/questions/63171725/send-mailkit-email-with-an-attachment-from-memorystream

			return emailMessage;
		}

		private void Send(MimeMessage mailMessage)
		{
			using (var client = new SmtpClient())
			{
				try
				{
					client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
					client.AuthenticationMechanisms.Remove("XOAUTH2");
					client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
					client.Send(mailMessage);
				}
				catch (Exception ex)
				{
					//log an error message or throw an exception or both.
					throw ex;
				}
				finally
				{
					client.Disconnect(true);
					client.Dispose();
				}
			}
		}

	}
}
