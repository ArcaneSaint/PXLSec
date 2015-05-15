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
using System.Xml.Serialization;

namespace cryptogui.Pages
{
	/// <summary>
	/// Interaction logic for LoginPage.xaml
	/// </summary>
	public partial class LoginPage : Page
	{
		public LoginPage(string name = null)
		{
			InitializeComponent();
			nameField.Text = name;

		}

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string name = nameField.Text.ToLower();
				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys");
				DirectoryInfo di = new DirectoryInfo(path);
				//int offset = 0;
				List<string> users = new List<string>();

				foreach (var dir in di.GetDirectories())
				{
					if (dir.GetFiles().Length >= 2 && dir.Name.ToLower() == name)
					{
						StartSession(name);
						return;
					}
				}
				lblError.Content = "Invalid name/password.";
			}
			catch (Exception ex)
			{
				Console.Error.WriteLineAsync(ex.StackTrace);
				lblError.Content = "An error occurred.";
			};
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{
			string name = nameField.Text;
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto");
			RSACrypto rsa = new RSACrypto();
			if (!Directory.Exists(Path.Combine(path, "Keys", name)))
			{
				Directory.CreateDirectory(Path.Combine(path, "Keys", name));
				Directory.CreateDirectory(Path.Combine(path, "Messages", name));
				Directory.CreateDirectory(Path.Combine(path, "Files", name));
				using (StreamWriter sw = new StreamWriter(Path.Combine(path, "Keys", name) + "/private.xml"))
				{
					sw.Write(rsa.PublicPrivateKeyXML);
				}
				using (StreamWriter sw = new StreamWriter(Path.Combine(path, "Keys", name) + "/public.xml"))
				{
					sw.Write(rsa.PublicKeyOnlyXML);
				}
				StartSession(name);
			}
			nameField.Text = "User with that name already exists";
			//(Window.GetWindow(this) as MainWindow).Content = new RegisterPage(this);
		}

		private void StartSession(string name)
		{
			(Window.GetWindow(this) as MainWindow).Title = "Crypto (" + name + ")";
			Session.User = name;
			SetMail(name);
			(Window.GetWindow(this) as MainWindow).Content = new AppPage();

		}

		private bool SetMail(string name)
		{
			try
			{
				string mailpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", name, "mailsettings.xml");
				using (StreamReader sr = new StreamReader(mailpath))
				{
					XmlSerializer sx = new XmlSerializer(typeof(CryptoMail));
					Session.Mail = (CryptoMail)sx.Deserialize(sr);
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
