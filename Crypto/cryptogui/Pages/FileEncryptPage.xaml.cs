using Crypto;
using Microsoft.Win32;
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
	/// Interaction logic for FileEncryptPage.xaml
	/// </summary>
	public partial class FileEncryptPage : UserControl
	{
		private string file;
		public FileEncryptPage()
		{
			InitializeComponent();
		}

		public List<string> getUsers()
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

		private static byte[] GetBytes(string str)
		{
			byte[] bytes = new byte[str.Length * sizeof(char)];
			System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}

		private bool EncryptFile(string user)
		{
			if (user != null)
			{
				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
				using (StreamReader sr = new StreamReader(path + "/public.xml"))
				{
					string key = sr.ReadToEnd();
					RSACrypto rsa = new RSACrypto(key);
					MD5Crypto md5 = new MD5Crypto();
					TripleDESCrypto des = new TripleDESCrypto();

					byte[] messageBytes = File.ReadAllBytes(file);

					byte[] desResult = des.Encrypt(messageBytes);

					byte[] testBytes = new byte[des.IV.Length + des.Key.Length];
					Buffer.BlockCopy(des.IV, 0, testBytes, 0, des.IV.Length);
					Buffer.BlockCopy(des.Key, 0, testBytes, des.IV.Length, des.Key.Length);

					byte[] rsaResult = rsa.Encrypt(testBytes);
					string md5Result = md5.GetFileChecksum(file);

					string messageStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, DateTime.Now.ToString("MMddhhmm"));
					if (!Directory.Exists(messageStorePath))
					{
						Directory.CreateDirectory(messageStorePath);
					}
					File.WriteAllBytes(Path.Combine(messageStorePath, "asymfile.crypt"), rsaResult);
					File.WriteAllBytes(Path.Combine(messageStorePath, "symmfile.crypt"), desResult);
					File.WriteAllText(Path.Combine(messageStorePath, "hashfile.crypt"), md5Result);
					return true;
				}
			}
			return false;
		}

		private void btnEncrypt_Click(object sender, RoutedEventArgs e)
		{
			/*string user = usersListView.SelectedItem as string;
			string message = txtboxMessage.Text;
			if (EncryptMessage(user, message))
			{
				txtboxMessage.Clear();
				txtboxMessage.Text = "Message encrypted and stored";
			}*/
			
		}

		private void btnFileSelect_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = ".txt";
			ofd.Filter = "Text documents (.txt)|*.txt";

			// Display OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = ofd.ShowDialog();

			// Get the selected file name and display in a TextBox
			if (result == true)
			{
				// Open document
				file = ofd.FileName;
				string filename = ofd.SafeFileName;
				txtboxFileSelect.Text = filename;
			}
		}
	}
}
