using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpotifyApi;
using SpotifyApi.Entities;

namespace SpotifyTests
{
    [TestClass]
    public class SpotifyTest
    {
        [TestMethod]
        public void ArtistSearchFindsResults()
        {
            Spotify api = new Spotify();

            var artistResults = api.FindArtistAsync("Savant").Result;
            Assert.IsTrue(artistResults.Count() > 0);
        }

        [TestMethod]
        public void ArtistSearchCorrectFirstResult()
        {
            Spotify api = new Spotify();

            var artistResults = api.FindArtistAsync("Savant").Result;
            var artist = artistResults.OrderByDescending(a => a.Popularity).First();
            Assert.AreEqual("Savant", artist.Name);
            Assert.AreEqual("spotify:artist:5RBdF1pJSLF3ugc2Y2PoB8", artist.Uri);
        }

        [TestMethod]
        public void AlbumSearchFindsResults()
        {
            Spotify api = new Spotify();

            var albumResults = api.FindAlbumAsync("Under The Table And Dreaming").Result;
            Assert.IsTrue(albumResults.Count() > 0);
        }

        [TestMethod]
        public void AlbumSearchCorrectFirstResult()
        {
            Spotify api = new Spotify();

            var albumResults = api.FindAlbumAsync("Under The Table And Dreaming").Result;
            var album = albumResults.OrderByDescending(a => a.Popularity).First();
            Assert.AreEqual("Under The Table And Dreaming", album.Name);
            Assert.AreEqual("spotify:album:3eAA4fvTVttgUlE43vRVMq", album.Uri);
            Assert.AreEqual("spotify:artist:2TI7qyDE0QfyOlnbtfDo7L", album.ArtistUri);
        }

        [TestMethod]
        public void TrackSearchFindsResults()
        {
            Spotify api = new Spotify();

            var trackResults = api.FindTrackAsync("Moon Trance").Result;
            Assert.IsTrue(trackResults.Count() > 0);
        }

        [TestMethod]
        public void TrackSearchCorrectFirstResult()
        {
            Spotify api = new Spotify();

            var trackResults = api.FindTrackAsync("Moon Trance").Result;
            var track = trackResults.OrderByDescending(a => a.Popularity).First();
            Assert.AreEqual("Moon Trance", track.Name);
            Assert.AreEqual("spotify:track:3hG8BApT5ep4mdGleYiCdL", track.Uri);
            Assert.AreEqual("spotify:artist:378dH6EszOLFShpRzAQkVM", track.ArtistUri);
            Assert.AreEqual("spotify:album:3YTWAm90osBvLNWCdF8Nq2", track.AlbumUri);
        }

        [TestMethod]
        public void ArtistLookupCorrectResult()
        {
            Spotify api = new Spotify();
            string artistUri = "spotify:artist:3mLG4odhlcwpfXIYjWh5TT";

            var artistDetail = api.LookupArtistAsync(artistUri).Result;
            Assert.AreEqual("Souleye", artistDetail.Name);
            Assert.IsTrue(artistDetail.Albums.Count() > 1);
            Assert.AreEqual(artistUri, artistDetail.Albums.First().ArtistUri);
            Assert.IsTrue(artistDetail.Albums.First().Tracks.Count() > 0);
            // This doesnt pass for every artist & album (Spotify has some inconsistencies)
            //Assert.AreEqual(artistUri, artistDetail.Albums.First().Tracks.First().ArtistUri);
        }
    }
}
