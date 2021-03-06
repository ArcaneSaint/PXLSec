﻿using Crypto;
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
			usersListView.ItemsSource= Session.GetUsers();
		}

		

		private bool EncryptMessage(string user, string message)
		{
			byte[] messageBytes = Session.GetBytes(message);

			string key = Session.GetPublicKey(user);
			if (key != null)
			{
				RSACrypto rsa = new RSACrypto(key);
				MD5Crypto md5 = new MD5Crypto();
				TripleDESCrypto des = new TripleDESCrypto();


				byte[] desResult = des.Encrypt(messageBytes);

				byte[] testBytes = new byte[des.IV.Length + des.Key.Length];
				Buffer.BlockCopy(des.IV, 0, testBytes, 0, des.IV.Length);
				Buffer.BlockCopy(des.Key, 0, testBytes, des.IV.Length, des.Key.Length);

				byte[] rsaResult = rsa.Encrypt(testBytes);
				string md5Result = md5.GetHash(message);

				string messageStorePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", user, DateTime.Now.ToString("dd-MM hh.mm"));
				if (!Directory.Exists(messageStorePath))
				{
					Directory.CreateDirectory(messageStorePath);
				}
				File.WriteAllBytes(Path.Combine(messageStorePath, "asymfile.crypt"), rsaResult);
				File.WriteAllBytes(Path.Combine(messageStorePath, "symmfile.crypt"), desResult);
				File.WriteAllText(Path.Combine(messageStorePath, "hashfile.crypt"), md5Result);
				return true;
			}
			return false;
		}

		private void btnEncrypt_Click(object sender, RoutedEventArgs e)
		{
			string user = usersListView.SelectedItem as string;
			string message = txtboxMessage.Text;
			if (EncryptMessage(user, message))
			{
				txtboxMessage.Clear();
				txtboxMessage.Text = "Message encrypted and stored";
			}
			btnEncrypt.IsEnabled = false;

		}

		private void usersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!String.IsNullOrWhiteSpace(txtboxMessage.Text))
			{
				btnEncrypt.IsEnabled = true;
			}
		}

		private void txtboxMessage_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (usersListView.SelectedIndex >= 0)
			{
				btnEncrypt.IsEnabled = true;
			}
		}
	}
}
