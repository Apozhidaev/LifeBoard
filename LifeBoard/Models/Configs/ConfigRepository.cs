using System;
using System.IO;
using System.Xml.Serialization;

namespace LifeBoard.Models.Configs
{
    /// <summary>
    /// Class ConfigRepository
    /// </summary>
    public static class ConfigRepository
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <value>The config.</value>
        public static Config Config { get; private set; }

        /// <summary>
        /// Sets the document path.
        /// </summary>
        /// <param name="path">The path.</param>
        public static void SetDocumentPath(string path)
        {
            if (Config.DocumentPath != path)
            {
                Config.DocumentPath = path;
                Save();
            }
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        public static void Open()
        {
            FileStream fs = null;
            try
            {
                var serializer = new XmlSerializer(typeof (Config));
                fs = new FileStream(Global.ConfigFile, FileMode.Open);
                Config = (Config) serializer.Deserialize(fs);
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

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>Config.</returns>
        private static Config Create()
        {
            return new Config
                       {
                           Language = Language.English,
                           Display = new Display
                                         {
                                             ShowIssue = new ShowIssue
                                                             {
                                                                 Table = new Issue {IsIssueType = true},
                                                                 Sitebar = new Issue()
                                                             }
                                         }
                       };
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public static void Save()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof (Config));
                writer = new StreamWriter(Global.ConfigFile);
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