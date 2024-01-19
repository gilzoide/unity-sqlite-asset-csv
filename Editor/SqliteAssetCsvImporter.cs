using System.IO;
using SQLite;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif
using UnityEngine;

namespace Gilzoide.SqliteAsset.Csv
{
    [ScriptedImporter(0, null, new[] { "csv" })]
    public class SqliteAssetCsvImporter : ScriptedImporter
    {
        [Header("SQLite asset options")]
        [Tooltip("Name of the table that will be created for holding the CSV data inside the database.")]
        [SerializeField] private string _tableName = "data";

        [Tooltip("Flags controlling how the SQLite connection should be opened. 'ReadWrite' and 'Create' flags will be ignored, since SQLite assets are read-only.")]
        [SerializeField] private SQLiteOpenFlags _openFlags = SQLiteOpenFlags.ReadOnly;

        [Tooltip("Whether to store DateTime properties as ticks (true) or strings (false).")]
        [SerializeField] private bool _storeDateTimeAsTicks = true;

        [Tooltip("Name of the file created for the database inside Streaming Assets folder during builds.\n\n"
            + "If empty, the database bytes will be stored in the asset itself.\n\n"
            + "Loading databases from Streaming Assets is not supported in Android and WebGL platforms.")]
        [SerializeField] private string _streamingAssetsPath;


        [Header("CSV options")]
        [Tooltip("Which separator character will be used when parsing the CSV file.")]
        [SerializeField] private CsvReader.SeparatorChar _CSVSeparator = CsvReader.SeparatorChar.Comma;

        [Tooltip("If true, the original CSV file will also be imported as a TextAsset")]
        [SerializeField] private bool _importCSVTextAsset = false;

        [Header("Additional SQL")]
        [Tooltip("SQL script that will be run before reading CSV data. Use this for configuring the generated database using PRAGMAs like 'page_size'.")]
        [SerializeField, Multiline] private string _SQLBeforeReadingCSV = "";

        [Tooltip("SQL script that will be run after reading CSV data. Use this for changing the table's schema, creating indices, etc.")]
        [SerializeField, Multiline] private string _SQLAfterReadingCSV = "";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            SqliteAsset asset;
            using (var tempDb = new SQLiteConnection(""))
            using (var file = File.OpenRead(assetPath))
            using (var stream = new StreamReader(file))
            {
                if (!string.IsNullOrWhiteSpace(_SQLBeforeReadingCSV))
                {
                    tempDb.Execute(_SQLBeforeReadingCSV);
                }
                tempDb.ImportCsvToTable(_tableName, stream, _CSVSeparator);
                if (!string.IsNullOrWhiteSpace(_SQLAfterReadingCSV))
                {
                    tempDb.Execute(_SQLAfterReadingCSV);
                }

                asset = tempDb.SerializeToAsset(_openFlags, _storeDateTimeAsTicks, _streamingAssetsPath);
            }
            ctx.AddObjectToAsset("sqlite", asset);
            ctx.SetMainObject(asset);

            if (_importCSVTextAsset)
            {
                var textAsset = new TextAsset(File.ReadAllText(assetPath))
                {
                    name = $"{Path.GetFileNameWithoutExtension(assetPath)}",
                };
                ctx.AddObjectToAsset("text", textAsset);
            }
        }
    }
}
