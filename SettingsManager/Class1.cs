using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

namespace bche.SettingsManager
{
    public class SettingsManager
    {
        public string SettingsFileName = "settings.xml";

        private Dictionary<string, string> Settings = new Dictionary<string, string>();

        public void LoadSettings()
        {
            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

                if (!isoStore.FileExists(SettingsFileName))
                    return;

                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(SettingsFileName, FileMode.Open, FileAccess.Read, isoStore))
                using (StreamReader sr = new StreamReader(isoStream))
                {
                    string settings = sr.ReadToEnd();

                    XElement element = XElement.Parse(settings);

                    //Dictionary<string, string> newSettings = new Dictionary<string, string>();
                    Settings = (from field in element.Elements("appSettings").Elements()
                                select new
                                {
                                    Name = field.Name.ToString(),
                                    field.Value
                                }).ToDictionary(x => x.Name, x => x.Value);
                }
            }
            catch
            {
            }
        }

        public void SaveSettings()
        {
            try
            {
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(SettingsFileName, FileMode.OpenOrCreate, FileAccess.Write, isoStore))
                using (StreamWriter sw = new StreamWriter(isoStream))
                {
                    List<XElement> elements = Settings.Keys.Select(setting => new XElement(setting, Settings[setting])).ToList();

                    XElement element =
                        new XElement("config",
                            new XElement("appSettings", elements));

                    sw.Write(element.ToString());
                }
            }
            catch
            {
            }
        }

        public string this[string key]
        {
            get
            {
                if (Settings.ContainsKey(key))
                    return Settings[key];
                return "";
            }
            set { Settings[key] = value; }
        }
    }
}
