using System;
using System.IO;
using System.Xml.Serialization;

namespace LifeBoard.Models.Configs
{
    public static class ConfigRepository
    {
        public static readonly string ConfigPath;

        public static readonly string ConfigFile;

        public static readonly string ConfigDirectory;

        static ConfigRepository()
        {
            ConfigFile = "Config.xml";
            ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Life Board";
            ConfigPath = ConfigDirectory + "\\" + ConfigFile;
        }

        public static Config Config { get; private set; }

        public static void SetDocumentPath(string path)
        {
            if (Config.DocumentPath != path)
            {
                Config.DocumentPath = path;
                Save();
            }
        }

        public static void Open()
        {
            FileStream fs = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Config));
                fs = new FileStream(ConfigPath, FileMode.Open);
                Config = (Config)serializer.Deserialize(fs);
            }
            catch (Exception)
            {
                Config = Create();
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

        }

        private static Config Create()
        {
            return new Config
                       {
                           Language = Language.English,
                           Display = new Display
                                         {
                                             ShowIssue = new ShowIssue
                                                             {
                                                                 Table = new Issue(),
                                                                 Sitebar = new Issue()
                                                             }
                                         }
                       };
        }

        public static void Save()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(Config));
                if (!Directory.Exists(ConfigDirectory))
                {
                    Directory.CreateDirectory(ConfigDirectory);
                }
                writer = new StreamWriter(ConfigPath);
                serializer.Serialize(writer, Config);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }
    }
}
