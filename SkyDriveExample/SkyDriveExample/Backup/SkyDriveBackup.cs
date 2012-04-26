using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Live;
using SkyDriveExample.WindowsLive;

namespace SkyDriveExample.Backup
{
	public class SkyDriveBackup
	{
		private readonly string _appFolderName;
		//   private string _folderId;
		private LiveConnectSession _session;
		private int _totalBackedUp = 0;
		public Dictionary<string, string> Files { get; set; }
		private List<string> FileNames { get; set; }

		private BackupViewModel _model;

		public MemoryStream ReadStream { get; set; }

		public string SkyDriveFolderId { get; private set; }
		public MyLiveConnectClient LiveClient { get; set; }



		public SkyDriveBackup(MyLiveConnectClient liveConnectClient, string appFolderName, LiveConnectSession session, List<string> fileNames, BackupViewModel model)
		{
			SkyDriveFolderId = string.Empty;
			Files = new Dictionary<string, string>();
			FileNames = fileNames;
			_session = session;
			_appFolderName = appFolderName;
			LiveClient = liveConnectClient;
			_model = model;
			GetFolders();
		}

		public void BackupFile(XDocument xdoc, string fileName)
		{
			Files.Clear();
			LiveClient.LiveClient.UploadCompleted += BackupCompletedCallback;

			ReadStream = new MemoryStream(Encoding.UTF8.GetBytes(xdoc.ToString()));

			LiveClient.LiveClient.UploadAsync(SkyDriveFolderId, fileName, true, ReadStream, null);
		}

		private void BackupCompletedCallback(object sender, LiveOperationCompletedEventArgs args)
		{
			_totalBackedUp += 1;
			if (args.Error == null)
			{
				_model.Message = "Backup complete.";

				if (_totalBackedUp == FileNames.Count)
				{
					ReadStream.Dispose();
					_totalBackedUp = 0;
				}

				_model.Date = "Checking for new backup...";

				//get the newly created fileID's (it will update the time too, and enable restoring)
				LiveClient.LiveClient = new LiveConnectClient(_session);
				LiveClient.GetLiveConnectData(SkyDriveFolderId + "/files", GetLiveConnectDataByFileIdCallback);
			}
			else
			{
				_model.Message = "Error uploading file: " + args.Error.ToString();
			}
		}

		private void GetFolders()
		{
			LiveClient.GetLiveConnectDataSkyDriveFiles("folders,albums", GetFoldersCallback);
		}

		private void GetFoldersCallback(object sender, LiveOperationCompletedEventArgs e)
		{
			_model.Message = "Loading folder...";

			Dictionary<string, object> folderData = (Dictionary<string, object>)e.Result;
			List<object> folders = (List<object>)folderData["data"];

			foreach (object item in folders)
			{
				Dictionary<string, object> folder = (Dictionary<string, object>)item;
				if (folder["name"].ToString() == _appFolderName)
				{
					SkyDriveFolderId = folder["id"].ToString();
				}
			}

			if (SkyDriveFolderId == string.Empty)
			{
				Dictionary<string, object> skyDriveFolderData = new Dictionary<string, object>();
				skyDriveFolderData.Add("name", _appFolderName);

				_model.Message = "Creating folder...";
				LiveClient.CreateLiveConnectFoldersSkyDrive(skyDriveFolderData, CreateLiveConnectFoldersSkyDriveCallback);
			}
			else
			{
				LiveClient.LiveClient = new LiveConnectClient(_session);
				_model.Message = "Ready to backup.";
				_model.Date = "Checking for previous backups...";
				_model.BackupEnabled = true;
				LiveClient.GetLiveConnectData(SkyDriveFolderId + "/files", GetLiveConnectDataByFileIdCallback);
			}
		}

		private void CreateLiveConnectFoldersSkyDriveCallback(object sender, LiveOperationCompletedEventArgs e)
		{
			_model.Message = "Ready to backup.";
			_model.Date = "No previous backup available to restore.";
			_model.BackupEnabled = true;
			Dictionary<string, object> folder = (Dictionary<string, object>)e.Result;
			SkyDriveFolderId = folder["id"].ToString();
		}

		private void GetLiveConnectDataByFileIdCallback(object sender, LiveOperationCompletedEventArgs e)
		{
			_model.Date = "Obtaining previous backup...";

			List<object> data = (List<object>)e.Result["data"];

			DateTimeOffset date = DateTime.MinValue;


			IDictionary<string, object> test = (IDictionary<string, object>)data.FirstOrDefault();

			try
			{
				date = DateTimeOffset.Parse(((string)test["updated_time"]).Substring(0, 19));
			}

			catch { }

			foreach (IDictionary<string, object> content in data)
			{
				if (FileNames.Contains((string)content["name"]) && !Files.Keys.Contains((string)content["name"]))
				{
					Files.Add((string)content["name"], (string)content["id"]);

					try
					{
						date = DateTimeOffset.Parse(((string)content["updated_time"]).Substring(0, 19));
					}

					catch { }

					// break;
				}
			}


			if (test != null)
			{
				try
				{
					_model.Date = (date != DateTime.MinValue) ? "Last backup on " + date.Add(date.Offset).DateTime : "Last backup on: unknown";
				}

				catch
				{
					_model.Date = "Last backup on: unknown";
				}

				_model.RestoreEnabled = true; //enable restoring since the file exists
			}

			else
				_model.Date = "No previous backup available to restore.";

		}
	}
}
