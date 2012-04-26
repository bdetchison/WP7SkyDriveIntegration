using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using SkyDriveExample.Backup;
using SkyDriveExample.WindowsLive;

namespace SkyDriveExample //CHANGE THIS to your app's namespace
{
	public partial class BackupPage : PhoneApplicationPage
	{
		private const string SkyDriveFolderName = "SkyDriveExample"; //the folder name for backups

		private LiveConnectClient _client;
		private LiveConnectSession _session;
		private MyLiveConnectClient _myClient;
		private SkyDriveBackup _backup;
		private int _numberDownLoaded;
		private readonly BackupViewModel _model;

		public BackupPage()
		{
			InitializeComponent();

			stackPanel.DataContext = _model = new BackupViewModel();
		}

		//event triggered when Skydrive sign in status is changed
		private void btnSignIn_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
		{
			//if the user is signed in
			if (e.Status == LiveConnectSessionStatus.Connected)
			{
				_session = e.Session;
				_client = new LiveConnectClient(e.Session);
				_model.Message = "Accessing SkyDrive...";


				_myClient = new MyLiveConnectClient(e.Session);

				_backup = new SkyDriveBackup(_myClient, SkyDriveFolderName, e.Session, new List<string> { "ReminderBackup.txt", "HistoryBackup.txt", "HarvestBackup.txt" }, _model);
			}
			else
			{
				_model.Message = "Not signed in.";
				_client = null;
			}
		}

		private void buttonBackup_Click(object sender, RoutedEventArgs e)
		{
			if (_client == null || _client.Session == null)
			{
				MessageBox.Show("You must sign in first.");
			}
			else
			{
				if (MessageBox.Show("Are you sure you want to backup? This will overwrite your old backup file!", "Backup?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
					UploadFile();
			}
		}

		public void UploadFile()
		{
			if (_backup.SkyDriveFolderId != string.Empty) //the folder must exist, it should have already been created
			{
				_model.Message = "Uploading backup...";
				_model.Date = "";

				try
				{

					List<XDocument> files = new List<XDocument>();
					files.Add(Dal.FileDal.ReadDataFromIsolatedStorageXmlDoc());
					files.Add(Dal.File2Dal.ReadDataFromIsolatedStorageXmlDoc());
					files.Add(Dal.File3Dal.ReadDataFromIsolatedStorageXmlDoc());


					_backup.BackupFile(files[0], "FileBackup.txt");
					_backup.BackupFile(files[1], "File2Backup.txt");
					_backup.BackupFile(files[2], "File3Backup.txt");
				}
				catch
				{
					MessageBox.Show("Error accessing IsolatedStorage. Please close the app and re-open it, and then try backing up again!", "Backup Failed", MessageBoxButton.OK);
					_model.Message = "Error. Close the app and start again.";
					_model.Date = "";
				}
			}
		}

		private void download()
		{
			foreach (var file in _backup.Files)
			{
				_model.Message = "Downloading backup...";

				_backup.LiveClient.LiveClient = new LiveConnectClient(_session);

				switch (file.Key)
				{
					case "FileBackup.txt":
						_backup.LiveClient.LiveClient.DownloadCompleted += new EventHandler<LiveDownloadCompletedEventArgs>(client_DownloadCompletedFile);
						break;
					case "File2Backup.txt":
						_backup.LiveClient.LiveClient.DownloadCompleted += new EventHandler<LiveDownloadCompletedEventArgs>(client_DownloadCompletedFile2);
						break;
					case "File3Backup.txt":
						_backup.LiveClient.LiveClient.DownloadCompleted += new EventHandler<LiveDownloadCompletedEventArgs>(client_DownloadCompletedFile3);
						break;
				}

				_backup.LiveClient.LiveClient.DownloadAsync(file.Value + "/content");
			}

			if (!_backup.Files.Any())
			{
				MessageBox.Show("Backup file doesn't exist!", "Error", MessageBoxButton.OK);
			}
		}

		private void client_DownloadCompletedFile(object sender, LiveDownloadCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				MemoryStream stream = (MemoryStream)e.Result;
				stream.Position = 0;

				XDocument xDoc = XDocument.Load(stream);
				Dal.FileDal.PutFile(xDoc);
			}
			else
			{
				MessageBox.Show("Unable to Restore files", "Error", MessageBoxButton.OK);
			}
		}

		private void client_DownloadCompletedFile2(object sender, LiveDownloadCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				MemoryStream stream = (MemoryStream)e.Result;
				stream.Position = 0;

				XDocument xDoc = XDocument.Load(stream);
				Dal.File2Dal.PutFile(xDoc);
			}
			else
			{
				MessageBox.Show("Unable to Restore files", "Error", MessageBoxButton.OK);
			}
		}

		private void client_DownloadCompletedFile3(object sender, LiveDownloadCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				MemoryStream stream = (MemoryStream)e.Result;
				stream.Position = 0;

				XDocument xDoc = XDocument.Load(stream);

				Dal.File3Dal.PutFile(xDoc);
			}
			else
			{
				MessageBox.Show("Unable to Restore files", "Error", MessageBoxButton.OK);
			}
		}

		private void buttonRestore_Click(object sender, RoutedEventArgs e)
		{
			if (_client == null || _client.Session == null)
			{
				MessageBox.Show("You must sign in first.");
			}
			else
			{
				if (MessageBox.Show("Are you sure you want to restore your data? This will overwrite all your current items and settings in the app!", "Restore?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
					download();
			}
		}
	}
}