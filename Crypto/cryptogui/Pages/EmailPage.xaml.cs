using Crypto;
using cryptogui.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace cryptogui.Pages
{
	/// <summary>
	/// Interaction logic for EmailPage.xaml
	/// </summary>
	public partial class EmailPage : System.Windows.Controls.UserControl
	{
		string asymfile = null;
		string symmfile = null;
		string hashfile = null;
		string user = null;
		public EmailPage()
		{
			InitializeComponent();
		}

		private void Configure_Click(object sender, RoutedEventArgs e)
		{
			EmailConfig ec = new EmailConfig();
			ec.ShowDialog();
			//usersListView.ItemsSource = Session.GetUsers();
		}

		private void Send_Click(object sender, RoutedEventArgs e)
		{
			if (asymfile != null)
			{
				string recipient = txtbox_Recipient.Text;
				string subject = txtbox_Subject.Text;

				MailMessage mailmessage = Session.Mail.CreateMessage(recipient, subject);
				mailmessage.Attachments.Add(new Attachment(asymfile));
				mailmessage.Attachments.Add(new Attachment(symmfile));
				mailmessage.Attachments.Add(new Attachment(hashfile));

				if (chkbox_PubKey.IsChecked.Value)
				{
					//attach public key
					mailmessage.Attachments.Add(new Attachment(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", Session.User, "public.xml")));
				}
				Session.Mail.SendMail(mailmessage);
				txtbox_Recipient.Clear();
				txtbox_Subject.Clear();
				txtbox_AsymFileSelect.Clear();
				txtbox_SymmFileSelect.Clear();
				txtbox_HashFileSelect.Clear();
				asymfile = null;
				symmfile = null;
				hashfile = null;
			}
			btn_Send.IsEnabled = false;
		}

		private void CheckSendReady()
		{
			if (asymfile != null &&
				!String.IsNullOrWhiteSpace(txtbox_Recipient.Text) &&
				!String.IsNullOrWhiteSpace(txtbox_Subject.Text))
			{
				btn_Send.IsEnabled = true;
			}
			else
			{
				btn_Send.IsEnabled = false;
			}
		}

		private void btnFileSelect_Click(object sender, RoutedEventArgs e)
		{
			// FolderBrowserDialog fbd = new FolderBrowserDialog();
			FolderBrowserDialog ofd = new FolderBrowserDialog();

			ofd.SelectedPath = (Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Files"));
			//ofd.DefaultExt = ".crypt";
			//ofd.Filter = "Crypto files|*.crypt";

			// Display OpenFileDialog by calling ShowDialog method
			System.Windows.Forms.DialogResult result = ofd.ShowDialog();

			// Get the selected file name and display in a TextBox
			if (result == DialogResult.OK)
			{
				try
				{
					//Get selected directory

					// Open asym document
					asymfile = Path.Combine(ofd.SelectedPath, "asymfile.crypt");
					symmfile = Path.Combine(ofd.SelectedPath, "symmfile.crypt");
					hashfile = Path.Combine(ofd.SelectedPath, "hashfile.crypt");

					string asymfilename = "asymfile.crypt";
					string symmfilename = "symmfile.crypt";
					string hashfilename = "hashfile.crypt";
					txtbox_AsymFileSelect.Text = asymfilename;
					txtbox_SymmFileSelect.Text = symmfilename;
					txtbox_HashFileSelect.Text = hashfilename;
				}
				catch (Exception ex)
				{
					txtbox_AsymFileSelect.Text = "Error";
					txtbox_SymmFileSelect.Text = "Error";
					txtbox_HashFileSelect.Text = "Error";
				}
			}
		}

		private void CheckSendReady(object sender, TextChangedEventArgs e)
		{
			CheckSendReady();
		}
	}
}
