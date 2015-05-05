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
	/// Interaction logic for DecryptPage.xaml
	/// </summary>
	public partial class DecryptPage : UserControl
	{
		private RSACrypto rsa;
		private string user;

		public DecryptPage(string user)
		{
			InitializeComponent();
			this.user = user;
			messagesListView.ItemsSource = getMessages(user);
			setRSASource(user);
		}

		private void setRSASource(string user)
		{
			string userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
			using (StreamReader sr = new StreamReader(userPath + "/private.xml"))
			{
				string key = sr.ReadToEnd();
				rsa = new RSACrypto(key);
			}
		}

		private List<string> getMessages(string user)
		{
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user);
			DirectoryInfo di = new DirectoryInfo(path);
			List<string> results = new List<string>();

			foreach (var dir in di.GetDirectories())
			{
				results.Add(dir.Name);
			}
			return results;
		}


		private void messagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//set messagebox to encrypted message
			string message = null;
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, messagesListView.SelectedItem as string);
			byte[] desEncrypted = File.ReadAllBytes(Path.Combine(path, "symmfile.crypt"));
			message=GetString(desEncrypted);
			txtboxMessage.Text=message;

		}
		static string GetString(byte[] bytes)
		{
			char[] chars = new char[bytes.Length / sizeof(char)];
			System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return new string(chars);
		}
		private void btnDecrypt_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				//decrypt message
				//show decrypted message (in textbox)

				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, messagesListView.SelectedItem as string);

				byte[] rsaEncrypted = File.ReadAllBytes(Path.Combine(path, "asymfile.crypt"));
				byte[] desEncrypted = File.ReadAllBytes(Path.Combine(path, "symmfile.crypt"));
				string md5Confirm = File.ReadAllText(Path.Combine(path, "hashfile.crypt"));

				//Get DES key from asymfile
				byte[] desBytes = rsa.Decrypt(rsaEncrypted);
				byte[] desIV = new byte[8];
				byte[] desKey = new byte[24];

				Buffer.BlockCopy(desBytes, 0, desIV, 0, desIV.Length);
				Buffer.BlockCopy(desBytes, desIV.Length, desKey, 0, desKey.Length);

				TripleDESCrypto des = new TripleDESCrypto(desKey, desIV);

				string messageDecrypted = GetString(des.Decrypt(desEncrypted));
				MD5Crypto md5 = new MD5Crypto();
				if (md5.GetHash(messageDecrypted) == md5Confirm)
				{
					txtboxMessage.Text = messageDecrypted;
				}
				else
				{
					txtboxMessage.Text = "Hatches do not match";
				}
			}
			catch (Exception ex)
			{
				txtboxMessage.Text = "An error occurred: "+ex.Message;
			}
		}
	}
}
