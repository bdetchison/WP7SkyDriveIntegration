using System;
using System.ComponentModel;

namespace SkyDriveExample.Backup
{
	public class BackupViewModel : INotifyPropertyChanged
	{
		private string _date = "";
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public string Date
		{
			get
			{
				return _date;
			}
			set
			{
				if (value != _date)
				{
					_date = value;
					NotifyPropertyChanged("Date");
				}
			}
		}

		private bool _restoreEnabled = false;
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public bool RestoreEnabled
		{
			get
			{
				return _restoreEnabled;
			}
			set
			{
				if (value != _restoreEnabled)
				{
					_restoreEnabled = value;
					NotifyPropertyChanged("RestoreEnabled");
				}
			}
		}

		private bool _backupEnabled = false;
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public bool BackupEnabled
		{
			get
			{
				return _backupEnabled;
			}
			set
			{
				if (value != _backupEnabled)
				{
					_backupEnabled = value;
					NotifyPropertyChanged("BackupEnabled");
				}
			}
		}

		private string _message = string.Empty;
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				if (value != _message)
				{
					_message = value;
					NotifyPropertyChanged("Message");
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}