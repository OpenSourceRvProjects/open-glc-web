using OpenGLC.Data.Entities;
using OpenGLC.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Emailer
{
	public interface IEmailSender
	{
		void SendEmail(MessageModel message, User user, Guid userID);

	}
}
