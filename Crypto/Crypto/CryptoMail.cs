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

		public CryptoMail( string address, string password, string host, int port=25) 
		{
			Port = port;
			Host = host;
			Address = address;
			Password = password;
			Init();
		}

		public void sendMail(string recipient, string message, string subject="CRYPTO MAIL")
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

	/*
		private void sendPXLMail(string recipient, string clientURL, string caretakerURL)
		{
			SmtpClient client = new SmtpClient("smtp.office365.com");
			var mail = new MailMessage();
			mail.From = new MailAddress("11308355@student.pxl.be");
			mail.To.Add(recipient);
			mail.Subject = "Questionnaire " + DateTime.Now.ToString();
			mail.IsBodyHtml = true;
			string body = "Link voor mantelzorger: http://finahfrontend.azurewebsites.net/questionnaire?id=" + caretakerURL + "\n Link voor patiënt: http://finahfrontend.azurewebsites.net/questionnaire?id=" + clientURL;
			mail.Body = body;
			client.Port = 587;
			client.UseDefaultCredentials = false;
			client.Credentials = new System.Net.NetworkCredential("11308355@student.pxl.be", "#####");
			client.EnableSsl = true;
			client.Send(mail);
		}*/
}
