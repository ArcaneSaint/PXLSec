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
		static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}

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
			List<string> dirs = new List<string>();
			if (!di.Exists)
			{
				di.Create();
			}
			foreach (var dir in di.GetDirectories())
			{
				dirs.Add(dir.Name);
			}

			//display

			while (true)
			{
				for (int i = offset; i < offset + 10 && i < dirs.Count; ++i)
				{
					Console.WriteLine(i + ": " + dirs[i]);
				}
				Console.WriteLine(((offset >= 10) ? "[L]ess" : "") + "\t[M]ore\t[C]ancel");
				string v = System.Console.ReadLine();
				int index;
				if (!int.TryParse(v, out index))
				{
					switch (v.First())
					{
						case 'L': if (offset - 10 >= 0) { offset -= 10; }; break;
						case 'M': if (offset + 10 < dirs.Count) { offset += 10; } break;
						case 'C': return null;
						default: break;
					}
				}
				else
				{
					if (index >= 0 && index < dirs.Count)
					{
						return dirs[index];
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
					string messagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, file);
					string userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
					using (StreamReader sr = new StreamReader(userPath + "/private.xml"))
					{



						byte[] rsaEncrypted = File.ReadAllBytes(Path.Combine(messagePath, "asymfile.crypt"));
						byte[] desEncrypted = File.ReadAllBytes(Path.Combine(messagePath, "symmfile.crypt"));
						string md5Confirm = File.ReadAllText(Path.Combine(messagePath, "hashfile.crypt"));


						string key = sr.ReadToEnd();
						RSACrypto rsa = new RSACrypto(key);

						//Get DES key from asymfile
						byte[] desBytes = rsa.Decrypt(rsaEncrypted);
						byte[] desIV = new byte[8];
						byte[] desKey = new byte[24];

						Buffer.BlockCopy(desBytes, 0, desIV, 0, desIV.Length);
						Buffer.BlockCopy(desBytes, desIV.Length, desKey, 0, desKey.Length);

						TripleDESCrypto des = new TripleDESCrypto(desKey, desIV);

						string message = GetString(des.Decrypt(desEncrypted));
						MD5Crypto md5 = new MD5Crypto();
						if (md5.GetHash(message) == md5Confirm)
						{
							Console.WriteLine("Hashes match");
							//string md5 = 

							Console.WriteLine("-- START MESSAGE --\n");
							Console.WriteLine(message);
							Console.WriteLine("\n-- END MESSAGE --");
						}
						else
						{
							Console.WriteLine("Hashes do not match");
						}
						//decrypt symfile with DES key


						//check hashes

						//Console.Write("Decrypting with " + user + "'s private key...");
						//byte[] resultBytes = rsa.Decrypt(Convert.FromBase64String(message));

						//string result = Convert.ToBase64String(resultBytes);
						//Console.WriteLine(result);
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
					MD5Crypto md5 = new MD5Crypto();
					TripleDESCrypto des = new TripleDESCrypto();

					Console.Write("Done.\n");
					Console.WriteLine("Write your message");
					string message = Console.ReadLine();
					byte[] messageBytes = GetBytes(message);

					Console.Write("Encrypting with DES...");
					byte[] desResult = des.Encrypt(messageBytes);
					Console.WriteLine("Done.");

					Console.Write("Encrypting DES Key with " + user + "'s public key...");

					byte[] testBytes = new byte[des.IV.Length + des.Key.Length];
					Buffer.BlockCopy(des.IV, 0, testBytes, 0, des.IV.Length);
					Buffer.BlockCopy(des.Key, 0, testBytes, des.IV.Length, des.Key.Length);

					//string rsaResult = rsa.Encrypt(des.ConstructorString);
					byte[] rsaResult = rsa.Encrypt(testBytes);
					//string rsaResult = Convert.ToBase64String(rsaByteResult);
					Console.WriteLine("Done.");


					Console.Write("Computing hash...");
					string md5Result = md5.GetHash(message);
					Console.WriteLine("Done.");

					//byte[] messageBytes = Convert.FromBase64String(message);

					//byte[] resultBytes = rsa.Encrypt(messageBytes);

					Console.WriteLine("Done.");
					string messageStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, DateTime.Now.ToString("MMddhhmm"));
					if (!Directory.Exists(messageStorePath))
					{
						Directory.CreateDirectory(messageStorePath);
					}
					File.WriteAllBytes(Path.Combine(messageStorePath, "asymfile.crypt"), rsaResult);
					File.WriteAllBytes(Path.Combine(messageStorePath, "symmfile.crypt"), desResult);
					File.WriteAllText(Path.Combine(messageStorePath, "hashfile.crypt"), md5Result);
				}
			}
		}
	}
}
