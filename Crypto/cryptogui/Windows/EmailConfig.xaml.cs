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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace cryptogui.Windows
{
	/// <summary>
	/// Interaction logic for EmailConfig.xaml
	/// </summary>
	public partial class EmailConfig : Window
	{
		public EmailConfig()
		{
			InitializeComponent();
			Import();
			if (Session.Mail != null)
			{
				txtbox_Address.Text = Session.Mail.Address;
				txtbox_Port.Text = Session.Mail.Port.ToString();
				txtbox_Host.Text = Session.Mail.Host;
				pwdbox_Password.Password = Session.Mail.Password;
			}
		}

		private void Import()
		{
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string address = txtbox_Address.Text;
			int port = Convert.ToInt32(txtbox_Port.Text);
			string host = txtbox_Host.Text;	
			string password = pwdbox_Password.Password;
			CryptoMail cm = new CryptoMail(address, password, host, port);
			Export(cm);
		}

		private void Export(CryptoMail cm)
		{
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", Session.User, "mailsettings.xml");
			using(StreamWriter sw = new StreamWriter(path))
			{
				XmlSerializer xs = new XmlSerializer(cm.GetType());
				xs.Serialize(sw, cm);
			}
			
		}
	}
}
