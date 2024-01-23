using System;
using System.Collections.Generic;
using System.IO;
using SQLite;

namespace Gilzoide.SqliteAsset.Csv
{
    public static class SQLiteConnectionExtensions
    {
        /// <summary>
        /// Import a CSV data stream into the table named <paramref name="tableName"/> inside the database.
        /// The table will be created if it doesn't exist yet.
        /// </summary>
        /// <param name="db">Open database connection</param>
        /// <param name="tableName">Name of the table that should be filled with data from the CSV data stream.</param>
        /// <param name="csvStream">Data stream with CSV-formatted contents.</param>
        /// <param name="separator">Separator used for parsing the CSV. Defaults to comma.</param>
        /// <param name="maxFieldSize">Maximum field size allowed.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of <paramref name="db"/>, <paramref name="tableName"/> and <paramref name="csvStream"/> are null.</exception>
        /// <exception cref="CsvException">Thrown if an error is found while parsing the CSV data.</exception>
        public static void ImportCsvToTable(this SQLiteConnection db, string tableName, TextReader csvStream, CsvReader.SeparatorChar separator = CsvReader.SeparatorChar.Comma, int maxFieldSize = int.MaxValue)
        {
            if (db == null)
            {
                throw new ArgumentNullException(nameof(db));
            }
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
