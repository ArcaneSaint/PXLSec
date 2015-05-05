using Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace cryptogui.Pages
{
	/// <summary>
	/// Interaction logic for EncryptPage.xaml
	/// </summary>
	public partial class EncryptPage : UserControl
	{
		public EncryptPage()
		{
			InitializeComponent();
			usersListView.ItemsSource = getUsers();
		}

		public List<string> getUsers()
		{
			//string name = nameField.Text;
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys");
			DirectoryInfo di = new DirectoryInfo(path);
			List<string> results = new List<string>();
			//List<string> users = new List<string>();

			foreach (var dir in di.GetDirectories())
			{
				results.Add(dir.Name);
			}
			return results;
			//lblError.Content = "Invalid name/password.";
		}

		private static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private void btnEncrypt_Click(object sender, RoutedEventArgs e)
		{
			string user = usersListView.SelectedItem as string;
			Console.Write("Done.\n");
			Console.WriteLine("Write your message");
			string message = txtboxMessage.Text;
			byte[] messageBytes = GetBytes(message);
			if (user != null)
			{
				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
				using (StreamReader sr = new StreamReader(path + "/public.xml"))
				{
					Console.Write("Opening...");
					string key = sr.ReadToEnd();
					RSACrypto rsa = new RSACrypto(key);
					MD5Crypto md5 = new MD5Crypto();
					TripleDESCrypto des = new TripleDESCrypto();


					byte[] desResult = des.Encrypt(messageBytes);

					byte[] testBytes = new byte[des.IV.Length + des.Key.Length];
					Buffer.BlockCopy(des.IV, 0, testBytes, 0, des.IV.Length);
					Buffer.BlockCopy(des.Key, 0, testBytes, des.IV.Length, des.Key.Length);

					//string rsaResult = rsa.Encrypt(des.ConstructorString);
					byte[] rsaResult = rsa.Encrypt(testBytes);
					//string rsaResult = Convert.ToBase64String(rsaByteResult);


					string md5Result = md5.GetHash(message);

					//byte[] messageBytes = Convert.FromBase64String(message);

					//byte[] resultBytes = rsa.Encrypt(messageBytes);

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
