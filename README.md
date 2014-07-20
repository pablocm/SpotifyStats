SpotifyStats
============

Spotify Stats is a small app that enables you to download artist and album 
information using the Spotify Metadata API, and browse the data locally.

Requirements
------------
For building and running the app, the following are needed:
- Visual Studio 2012
- .NET Framework 4.5
- A running SQL Server instance

Instructions
------------
Open solution in VS 2012, hit build and run. The first time the app is built, VS 2012 should automatically download the Entity Framework 6.1.1 Nugget package. When the app is starting up, it will automatically create the database entities needed.

You can search and download artists using the search box in the "Download" tab. Once downloaded, you can check the album and track information in the "Explore" tab.
