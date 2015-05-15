using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Crypto
{
	[Serializable]
	public class CryptoMail
	{
		SmtpClient Client { get; set; }

		public string Host { get; set; }
		public int Port { get; set; }
		public string Address { get; set; }
		public string Password { get; set; }

		public CryptoMail() 
		{}

		public void Init()
		{
			Client = new SmtpClient();
			Client.Host = Host;
			Client.Port = Port;
			Client.UseDefaultCredentials = false;
			Client.EnableSsl = true;
			Client.Credentials = new System.Net.NetworkCredential(Address, Password);
		}

		public CryptoMail(string address, string password, string host, int port=25) 
		{
			Port = port;
			Host = host;
			Address = address;
			Password = password;
			Init();
		}

		public MailMessage CreateMessage(string recipient, string subject)
		{
			MailMessage mail = new MailMessage()
			{
				From = new MailAddress(Address),
				Subject = subject,
				IsBodyHtml = false,
				Body = "Send via Crypto app."
			};
			mail.To.Add(recipient);

			return mail;
		}

		public void SendMail(MailMessage mail)
		{
			Client.Send(mail);
		}

		public void SendMail(string recipient, string message, string subject)
		{
			if (Address != null)
			{
				var mail = new MailMessage();
				mail.From = new MailAddress(Address);
				mail.To.Add(recipient);
				mail.Subject = subject;
				mail.IsBodyHtml = false;
				mail.Body = message;

				Client.Send(mail);
			}
		}

	}
}
