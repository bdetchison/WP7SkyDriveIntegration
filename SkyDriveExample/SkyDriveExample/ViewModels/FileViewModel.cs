using System;
using System.ComponentModel;

namespace SkyDriveExample.ViewModels
{
	public class FileViewModel : INotifyPropertyChanged
	{
		private Guid _id = Guid.NewGuid();
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		/// 
		public Guid Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (value != _id)
				{
					_id = value;
					NotifyPropertyChanged("Id");
				}
			}
		}

		private string _description = string.Empty;
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (value != _description)
				{
					_description = value;
					NotifyPropertyChanged("Description");
				}
			}
		}

		private bool _isNew = true;
		/// <summary>
		/// Sample ViewModel property; this property is used in the view to display its value using a Binding.
		/// </summary>
		/// <returns></returns>
		public bool IsNew
		{
			get
			{
				return _isNew;
			}
			set
			{
				if (value != _isNew)
				{
					_isNew = value;
					NotifyPropertyChanged("IsNew");
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