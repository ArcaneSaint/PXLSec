using cryptogui.Pages;
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

namespace cryptogui
{
	/// <summary>
	/// Interaction logic for AppPage.xaml
	/// </summary>
	public partial class AppPage : Page
	{
		public void refresh()
		{
			itemOne.Content = new EncryptPage();
			itemTwo.Content = new DecryptPage();
			itemThree.Content = new FileEncryptPage();
			itemFour.Content = new FileDecryptPage();
			itemFive.Content = new SteganographyPage();
			itemSix.Content = new EmailPage();
		}

		public AppPage()
		{
			InitializeComponent();
			itemOne.Content = new EncryptPage();
			itemTwo.Content = new DecryptPage();
			itemThree.Content = new FileEncryptPage();
			itemFour.Content = new FileDecryptPage();
			//Content="Pages/EncryptPage.xaml"
		}

		private void miLogout_Click(object sender, RoutedEventArgs e)
		{
			Logout();
		}

		private void miExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void tabcontrol_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.Source is TabControl)
			{
				refresh();
			}
		}

		private void miDelete_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("This will remove your private/public keypair, and any messages for you stored from this computer. Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
			if (messageBoxResult == MessageBoxResult.Yes) 
			{
				Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Keys", Session.User), true);
				Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Messages", Session.User), true);
				Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AppDevCrypto", "Files", Session.User), true);
				(Window.GetWindow(this) as MainWindow).Title = "Crypto";
				Logout();
			}
		}
		private void Logout()
		{
			(Window.GetWindow(this) as MainWindow).Content = new LoginPage();
		}
	}
}
