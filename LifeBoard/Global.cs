using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using LifeBoard.Models;

namespace LifeBoard
{
    public static class Global
    {
        public static readonly string ApplicationFolder;

        public static readonly string BackupFolder;

        public static readonly string ConfigFile;

        public static string NewBackupFile
        {
            get { return String.Format("{0}\\{1}.life", BackupFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffffff", CultureInfo.InvariantCulture)); }
        }

        public static readonly int BackupMaxCount = 30;

        static Global()
        {
            ApplicationFolder = String.Format("{0}\\Life Board", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            BackupFolder = String.Format("{0}\\Backups", ApplicationFolder);
            ConfigFile = String.Format("{0}\\Config.xml",ApplicationFolder);

            if (!Directory.Exists(ApplicationFolder))
            {
                Directory.CreateDirectory(ApplicationFolder);
            }
            if (!Directory.Exists(BackupFolder))
            {
                Directory.CreateDirectory(BackupFolder);
            }
        }

        public static Language Language
        {
            get
            {
                return Application.Current.Resources.MergedDictionaries.Any(
                    dictionary => dictionary.Contains("Lang") && (string)dictionary["Lang"] == "ru") 
                    ? Language.Russian 
                    : Language.English;
            }
        }

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
                var dictionary = Application.Current.Resources.MergedDictionaries[i];
                if (dictionary.Contains("Lang"))
                {
                    Application.Current.Resources.MergedDictionaries.Remove(dictionary);
                    Application.Current.Resources.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(
                            new Uri(resourse, UriKind.Relative)));
                    break;
                }
            }
        }
    }
}
