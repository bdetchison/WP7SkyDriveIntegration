using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Live;

namespace SkyDriveExample.WindowsLive
{
	public class MyLiveConnectClient
	{
		public LiveConnectClient LiveClient { get; set; }
		public LiveConnectSession LiveClientSession { get; private set; }

		public MyLiveConnectClient(LiveConnectSession session)
		{
			LiveClientSession = session;
			LiveClient = new LiveConnectClient(session);
		}

		public void GetLiveConnectData(string query, EventHandler<LiveOperationCompletedEventArgs> callback)
		{
			LiveClient.GetCompleted += callback;
			LiveClient.GetAsync(query);
		}

		public void CreateLiveConnectFolders(string directory, Dictionary<string, object> folderDictionary, EventHandler<LiveOperationCompletedEventArgs> callback)
		{
			LiveClient.PostCompleted += callback;
			LiveClient.PostAsync(directory, folderDictionary);
		}

		public void CreateLiveConnectFoldersSkyDrive(Dictionary<string, object> folderDictionary, EventHandler<LiveOperationCompletedEventArgs> callback)
		{
			if (folderDictionary.Any())
			{
				CreateLiveConnectFolders("me/skydrive", folderDictionary, callback);
			}
		}

		public void GetLiveConnectDataSkyDriveFiles(string filter, EventHandler<LiveOperationCompletedEventArgs> callback)
		{
			if (filter == string.Empty)
			{
				GetLiveConnectData("me/skydrive/files", callback);
			}
			else
			{
				GetLiveConnectData("me/skydrive/files?filter=" + filter, callback);
			}
		}
	}
}
