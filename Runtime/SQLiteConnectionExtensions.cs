using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace Gilzoide.SqliteAsset.Csv
{
    public static class SQLiteConnectionExtensions
    {
        public static void ImportCsvToTable(this SQLiteConnection db, string tableName, StreamReader csvStream, CsvReader.SeparatorChar separator = CsvReader.SeparatorChar.Comma, int maxFieldSize = int.MaxValue)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }
            if (csvStream == null)
            {
                throw new ArgumentNullException(nameof(csvStream));
            }

            var columns = new List<string>();
            bool parsingHeader = true;
            db.RunInTransaction(() =>
            {
                foreach (string field in CsvReader.ParseStream(csvStream, separator, maxFieldSize))
                {
                    if (field == null)  // newline
                    {
                        string joinedColumns = string.Join(", ", columns);
                        if (parsingHeader)
                        {
                            db.Execute($"CREATE TABLE IF NOT EXISTS {tableName} ({joinedColumns})");
                            parsingHeader = false;
                        }
                        else
                        {
                            db.Execute($"INSERT INTO {tableName} VALUES ({joinedColumns})");
                        }
                        columns.Clear();
                    }
                    else
                    {
                        if (parsingHeader && string.IsNullOrWhiteSpace(field))
                        {
                            throw new CsvException("Header cannot have empty column name.");
                        }

                        columns.Add(SQLiteConnection.Quote(field));
                    }
                }
            });
        }
    }
}
