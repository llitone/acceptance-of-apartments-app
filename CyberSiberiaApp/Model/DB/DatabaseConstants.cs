namespace CyberSiberiaApp.Model.DB
{
    public class DatabaseConstants
    {
        public const string DATABASE_FILENAME = "DP.db3";

        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite
            | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

        public static string DATABASE_PATH
        {
            get
            {
                return Path.Combine(FileSystem.AppDataDirectory, DATABASE_FILENAME);
            }
        }
    }
}
