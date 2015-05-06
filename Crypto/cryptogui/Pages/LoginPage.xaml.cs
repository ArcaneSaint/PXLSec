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

namespace cryptogui.Pages
{
	/// <summary>
	/// Interaction logic for LoginPage.xaml
	/// </summary>
	public partial class LoginPage : Page
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string name = nameField.Text;
				string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys");
				DirectoryInfo di = new DirectoryInfo(path);
				//int offset = 0;
				List<string> users = new List<string>();
			
				foreach (var dir in di.GetDirectories())
				{
					if (dir.GetFiles().Length >= 2 && dir.Name == name)
					{
						(Window.GetWindow(this) as MainWindow).Content = new AppPage(name);
						return;
						//(Window.GetWindow(this) as MainWindow).ContentFrame.Navigate(typeof(AppPage));
					}
				}
				lblError.Content = "Invalid name/password.";
			}
			catch (Exception ex)
			{
				lblError.Content = "Invalid name/password.";
			};
		}

		private void btnCreate_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
