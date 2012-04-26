using System;
using System.Windows;
using Microsoft.Phone.Controls;
using SkyDriveExample.Dal;
using SkyDriveExample.ViewModels;

namespace SkyDriveExample
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			//create files in iso storage
			FileDal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			FileDal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			FileDal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File2Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File2Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File2Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });
			File3Dal.Insert(new FileViewModel { Description = "Test Description", Id = Guid.NewGuid() });

			NavigationService.Navigate(new Uri("/BackupPage.xaml", UriKind.Relative));
		}
	}
}