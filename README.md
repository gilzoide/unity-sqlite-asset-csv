# SQLite Asset - CSV
Easily import ".csv" files as read-only [SQLite database assets](https://github.com/gilzoide/unity-sqlite-asset).


## Features
- Import CSV files as SQLite database assets by changing the importer to `Gilzoide.SqliteAsset.Csv.SqliteAssetCsvImporter` in the Inspector.
- `SQLiteConnection.ImportCsvToTable` extension method for importing a CSV data stream as a new table inside the database.


## Dependencies
- [SQLite-net](https://github.com/gilzoide/unity-sqlite-net): library for managing SQLite databases
- [SQLite Asset](https://github.com/gilzoide/unity-sqlite-asset): read-only SQLite database assets for Unity


## How to install
Either:
- Install using the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) with the following URL:
  ```
  https://github.com/gilzoide/unity-sqlite-asset-csv.git
  ```
- Clone this repository or download a snapshot of it directly inside your project's `Assets` or `Packages` folder.
