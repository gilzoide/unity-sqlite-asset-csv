using System;

namespace Gilzoide.SqliteAsset.Csv
{
    public class CsvException : Exception
    {
        public CsvException(string message) : base(message) {}
    }
}
