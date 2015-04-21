using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Crypto;
using System.IO;

namespace CryptoConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			string p1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys");
			string p2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages");
			if (!Directory.Exists(p1))
			{
				Directory.CreateDirectory(p1);
			}
			if (!Directory.Exists(p2))
			{
				Directory.CreateDirectory(p2);
			}
			Home();
		}

		static void Home()
		{
			bool exit = false;
			while (!exit)
			{
				System.Console.WriteLine("(M)ake user.\n(E)ncrypt.\n(D)ecrypt.\n(Q)uit.");

				char c = (char)System.Console.Read();
				System.Console.ReadLine();
				switch (c)
				{
					case 'M': makeAccount(); break;
					case 'E': encrypt(); break;
					case 'D': decrypt(); break;
					case 'Q': exit = true; break;
					default: break;
				}
			}
		}

		static void makeAccount()
		{
			System.Console.Write("Name: ");
			string s = System.Console.ReadLine();
			//string t = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			//string path = "/AppDevCrypto/Keys/"+s;
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", s);
			System.Console.Write("Generating key...");
			RSACrypto rsa = new RSACrypto();
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
				using (StreamWriter sw = new StreamWriter(path + "/private.xml"))
				{
					sw.Write(rsa.PublicPrivateKeyXML);
				}
				using (StreamWriter sw = new StreamWriter(path + "/public.xml"))
				{
					sw.Write(rsa.PublicKeyOnlyXML);
				}
			}
			System.Console.Write("Done\n");
		}

		static string selectUserDiag()
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys");
			DirectoryInfo di = new DirectoryInfo(path);
			int offset = 0;
			List<string> users = new List<string>();
			
			foreach (var dir in di.GetDirectories())
			{
				if (dir.GetFiles().Length >= 2)
				{
					users.Add(dir.Name);
				}
			}
			
			//display

			while(true){
				for (int i = offset; i < offset + 10 && i<users.Count; ++i)
				{
					Console.WriteLine(i+": " + users[i]);
				}
				Console.WriteLine(((offset>=10)?"[L]ess":"") + "\t[M]ore\t[C]ancel");
				string v = System.Console.ReadLine();
				int index;
				if (!int.TryParse(v, out index))
				{
					switch (v.First())
					{
						case 'L': if (offset - 10 >= 0) { offset -= 10; }; break;
						case 'M': if (offset + 10 < users.Count) { offset += 10; } break;
						case 'C': return null;
						default: break;
					}
				}
				else
				{
					if (index >= 0 && index < users.Count) 
					{
						return users[index];
					}
				}
			}
		}
		static string selectEncryptedFileDiag(string user)
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages");
			DirectoryInfo di = new DirectoryInfo(Path.Combine(path,user));
			int offset = 0;
			List<string> files = new List<string>();
			if (!di.Exists)
			{
				di.Create();
			}
			foreach (var file in di.GetFiles())
			{
				files.Add(file.Name);
			}

			//display

			while (true)
			{
				for (int i = offset; i < offset + 10 && i < files.Count; ++i)
				{
					Console.WriteLine(i + ": " + files[i]);
				}
				Console.WriteLine(((offset >= 10) ? "[L]ess" : "") + "\t[M]ore\t[C]ancel");
				string v = System.Console.ReadLine();
				int index;
				if (!int.TryParse(v, out index))
				{
					switch (v.First())
					{
						case 'L': if (offset - 10 >= 0) { offset -= 10; }; break;
						case 'M': if (offset + 10 < files.Count) { offset += 10; } break;
						case 'C': return null;
						default: break;
					}
				}
				else
				{
					if (index >= 0 && index < files.Count)
					{
						return files[index];
					}
				}
			}
		}
		
		static void decrypt()
		{
			string user = selectUserDiag();
			if (user != null)
			{
				string file = selectEncryptedFileDiag(user);
				if (file != null)
				{
					string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
					using (StreamReader sr = new StreamReader(path + "/private.xml"))
					{
						byte[] messageBytes = File.ReadAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, file));
						string key = sr.ReadToEnd();
						RSACrypto rsa = new RSACrypto(key);

						Console.Write("Decrypting with " + user + "'s private key...");
						byte[] resultBytes = rsa.Decrypt(messageBytes);
						string result = Encoding.Default.GetString(resultBytes);
						Console.WriteLine(result);
						//rsa.Decrypt()
					}
				}
			}
		}

		static void encrypt() 
		{
			string user = selectUserDiag();
			if (user != null)
			{
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
				using (StreamReader sr = new StreamReader(path + "/public.xml"))
				{
					Console.Write("Opening...");
					string key = sr.ReadToEnd();
					RSACrypto rsa = new RSACrypto(key);
					Console.Write("Done.\n");
					Console.WriteLine("Write your message");
					string message = Console.ReadLine();

					Console.Write("Encrypting with " + user + "'s public key...");

					byte[] messageBytes = new byte[message.Length * sizeof(char)];
					System.Buffer.BlockCopy(message.ToCharArray(), 0, messageBytes, 0, messageBytes.Length);

					byte[] resultBytes = rsa.Encrypt(messageBytes);

					Console.WriteLine("Done.");
					string p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user);
					if (!Directory.Exists(p))
					{
						Directory.CreateDirectory(p);
						File.WriteAllBytes(Path.Combine(p, DateTime.Now.ToString("MMddhhmm")), resultBytes);
					}
				}
			}
		}
	}
}
