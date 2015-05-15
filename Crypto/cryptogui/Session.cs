using Crypto;
using System;
using System.Collections.Generic;
using System.IO;
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

		public static List<string> GetUsers()
		{
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys");
			DirectoryInfo di = new DirectoryInfo(path);
			List<string> results = new List<string>();

			foreach (var dir in di.GetDirectories())
			{
				results.Add(dir.Name);
			}
			return results;
		}

		public static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		public static string GetPublicKey(string user)
		{
			try
			{
				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
				using (StreamReader sr = new StreamReader(path + "/public.xml"))
				{
					string key = sr.ReadToEnd();
					return key;
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
				return null;
			}
		}

		//string key = sr.ReadToEnd();
	}
}
