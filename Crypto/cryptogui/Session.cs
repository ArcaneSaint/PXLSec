using Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptogui
{
	public static class Session
	{
		public static string User { get; set; }
		private static CryptoMail mail;
		public static CryptoMail Mail 
		{ 
			get {return mail;}
			set { mail = value; Init(); } 
		}
		public static void Init()
		{
			mail.Init();
		}
		/*string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", Session.User, "mailsettings.xml");
			using (StreamReader sr = new StreamReader(path))
			{
				XmlSerializer sx = new XmlSerializer(typeof(CryptoMail));
				Session.Mail = (CryptoMail)sx.Deserialize(sr);
			}*/
	}
}
