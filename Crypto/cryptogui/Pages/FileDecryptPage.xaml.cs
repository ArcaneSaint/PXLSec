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
	/// Interaction logic for FileDecryptPage.xaml
	/// </summary>
	public partial class FileDecryptPage : UserControl
	{
		private RSACrypto rsa;

		public FileDecryptPage()
		{
			InitializeComponent();
			filesListView.ItemsSource = GetFiles(Session.User);
			SetRSASource(Session.User);
		}
		private void SetRSASource(string user)
		{
			string userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", user);
			using (StreamReader sr = new StreamReader(userPath + "/private.xml"))
			{
				string key = sr.ReadToEnd();
				rsa = new RSACrypto(key);
			}
		}
		public List<string> GetFiles(string user)
		{
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Files", user);
			DirectoryInfo di = new DirectoryInfo(path);
			List<string> results = new List<string>();

			foreach (var dir in di.GetDirectories())
			{
				results.Add(dir.Name);
			}
			return results;
		}

		private void btnDecrypt_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Title = "Decrypted file save location";
			// Display OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = sfd.ShowDialog();
			if (result == true)
			{
				try
				{
					string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Files", Session.User, filesListView.SelectedItem as string);

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

					File.WriteAllBytes(sfd.FileName, des.Decrypt(desEncrypted));
					//string messageDecrypted = GetString(des.Decrypt(desEncrypted));
					MD5Crypto md5 = new MD5Crypto();
					if (md5.GetFileChecksum(sfd.FileName) == md5Confirm)
					{
						lblResult.Content = "Decryption Succesful";
					}
				}
				catch (Exception ex)
				{
					lblResult.Content = "An error occurred: " + ex.Message;
				}
			}
		}
	}
}
