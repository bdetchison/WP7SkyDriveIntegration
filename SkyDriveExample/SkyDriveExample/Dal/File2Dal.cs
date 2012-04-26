using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;
using SkyDriveExample.ViewModels;

namespace SkyDriveExample.Dal
{
	public class File2Dal
	{
		private const string FilePath = "File2.xml";

		public static XDocument ReadDataFromIsolatedStorageXmlDoc()
		{
			ObservableCollection<FileViewModel> obsColTimes = new ObservableCollection<FileViewModel>();

			using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!storage.FileExists(FilePath))
				{
					return new XDocument();
				}

				using (var isoFileStream = new IsolatedStorageFileStream(FilePath, FileMode.OpenOrCreate, storage))
				{
					using (XmlReader reader = XmlReader.Create(isoFileStream))
					{
						return XDocument.Load(reader);
					}
				}
			}
		}

		public static ObservableCollection<FileViewModel> ReadDataFromIsolatedStorage()
		{
			ObservableCollection<FileViewModel> obsColFiles = new ObservableCollection<FileViewModel>();

			using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
			{
				if (!storage.FileExists(FilePath))
				{
					return obsColFiles;
				}

				using (var isoFileStream = new IsolatedStorageFileStream(FilePath, FileMode.OpenOrCreate, storage))
				{
					using (XmlReader reader = XmlReader.Create(isoFileStream))
					{

						XDocument xml = XDocument.Load(reader);

						foreach (var file in xml.Root.Elements("File"))
						{
							Guid id;

							Guid.TryParse(file.Attribute("Id").Value, out id);
							string description = file.Attribute("Description").Value;

							obsColFiles.Add(new FileViewModel { Description = description, Id = id, IsNew = false });
						}

						return obsColFiles;
					}
				}
			}
		}

		public static void Insert(FileViewModel newFileRecord)
		{
			ObservableCollection<FileViewModel> obsColFiles = ReadDataFromIsolatedStorage();

			obsColFiles.Add(newFileRecord);

			XDocument doc = new XDocument();
			XElement item = new XElement("SkyDriveExample");

			foreach (FileViewModel file in obsColFiles)
			{
				XElement data = new XElement("File");
				XAttribute description = new XAttribute("Description", file.Description);
				XAttribute id = new XAttribute("Id", file.Id);

				data.Add(description);
				data.Add(id);
				item.Add(data);
			}

			doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), item);

			IsolatedStorage.SaveDataToIsolatedStorage(FilePath, FileMode.Create, doc);
		}

		public static void PutFile(XDocument xmlDoc)
		{
			IsolatedStorage.SaveDataToIsolatedStorage(FilePath, FileMode.Create, xmlDoc);
		}
	}
}
