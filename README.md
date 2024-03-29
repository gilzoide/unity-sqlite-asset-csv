# SQLite Asset - CSV
[![openupm](https://img.shields.io/npm/v/com.gilzoide.sqlite-asset.csv?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.gilzoide.sqlite-asset.csv/)

Easily import ".csv" files as read-only [SQLite database assets](https://github.com/gilzoide/unity-sqlite-asset).


## Features
- Import CSV files as SQLite database assets by changing the importer to `Gilzoide.SqliteAsset.Csv.SqliteAssetCsvImporter` in the Inspector.
- `SQLiteConnection.ImportCsvToTable` extension method for importing a CSV data stream as a new table inside the database.


## Dependencies
- [SQLite-net](https://github.com/gilzoide/unity-sqlite-net): library for managing SQLite databases
- [SQLite Asset](https://github.com/gilzoide/unity-sqlite-asset): read-only SQLite database assets for Unity


## How to install
Either:
- Use the [openupm registry](https://openupm.com/) and install this package using the [openupm-cli](https://github.com/openupm/openupm-cli):
  ```
  openupm add com.gilzoide.sqlite-asset.csv
  ```
- Install using the [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) with the following URL:
  ```
  https://github.com/gilzoide/unity-sqlite-asset-csv.git#1.0.0-preview2
  ```
- Clone this repository or download a snapshot of it directly inside your project's `Assets` or `Packages` folder.
