﻿using cryptogui.Pages;
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

namespace cryptogui
{
	/// <summary>
	/// Interaction logic for AppPage.xaml
	/// </summary>
	public partial class AppPage : Page
	{
		public AppPage()
		{
			InitializeComponent();
			itemOne.Content = new EncryptPage();
			//Content="Pages/EncryptPage.xaml"
		}
	}
}