using cryptogui.Windows;
using System;
using System.Collections.Generic;
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
	/// Interaction logic for EmailPage.xaml
	/// </summary>
	public partial class EmailPage : UserControl
	{
		public EmailPage()
		{
			InitializeComponent();
		}

		private void Configure_Click(object sender, RoutedEventArgs e)
		{
			EmailConfig ec = new EmailConfig();
			ec.ShowDialog();
		}
	}
}
