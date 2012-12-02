using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard
{
    /// <summary>
    /// Class Global
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// The application folder
        /// </summary>
        public static readonly string ApplicationFolder;

        /// <summary>
        /// The backup folder
        /// </summary>
        public static readonly string BackupFolder;

        /// <summary>
        /// The config file
        /// </summary>
        public static readonly string ConfigFile;

        /// <summary>
        /// The backup max count
        /// </summary>
        public static readonly int BackupMaxCount = 30;

        /// <summary>
        /// Initializes static members of the <see cref="Global" /> class.
        /// </summary>
        static Global()
        {
            ApplicationFolder = String.Format("{0}\\Life Board",
                                              Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            BackupFolder = String.Format("{0}\\Backups", ApplicationFolder);
            ConfigFile = String.Format("{0}\\Config.xml", ApplicationFolder);

            if (!Directory.Exists(ApplicationFolder))
            {
                Directory.CreateDirectory(ApplicationFolder);
            }
            if (!Directory.Exists(BackupFolder))
            {
                Directory.CreateDirectory(BackupFolder);
            }
        }

        /// <summary>
        /// Gets the new backup file.
        /// </summary>
        /// <value>The new backup file.</value>
        public static string NewBackupFile
        {
            get
            {
                return String.Format("{0}\\{1}.life", BackupFolder,
                                     DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffffff", CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <value>The language.</value>
        public static Language Language
        {
            get
            {
                return Application.Current.Resources.MergedDictionaries.Any(
                    dictionary => dictionary.Contains("Lang") && (string) dictionary["Lang"] == "ru")
                           ? Language.Russian
                           : Language.English;
            }
        }

        /// <summary>
        /// Updates the resources.
        /// </summary>
        /// <param name="language">The language.</param>
        public static void UpdateResources(Language language)
        {
            if (Language == language)
            {
                return;
            }

            string resourse = "Assets/StringResources.xaml";
            if (language == Language.Russian)
            {
                resourse = "Assets/StringResources.ru.xaml";
            }

            for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
            {
                ResourceDictionary dictionary = Application.Current.Resources.MergedDictionaries[i];
                if (dictionary.Contains("Lang"))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dictionary);
                    Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary) Application.LoadComponent(
                        new Uri(resourse, UriKind.Relative)));
                    break;
                }
            }
        }
    }
}