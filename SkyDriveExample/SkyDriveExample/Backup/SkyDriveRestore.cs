using System;
using Microsoft.Live;

namespace SkyDriveExample.Backup
{
	public class SkyDriveRestore
	{
		public LiveConnectClient LiveClient { get; set; }

		public SkyDriveRestore(LiveConnectClient liveConnectClient)
		{
			LiveClient = liveConnectClient;
		}

		public bool RestoreFile(string query, EventHandler<LiveDownloadCompletedEventArgs> callback)
		{
			if (query != null)
			{
				LiveClient.DownloadCompleted += callback;
				LiveClient.DownloadAsync(query);
				return true;
			}

			return false;
		}
	}
}
