﻿using System;
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

            var artistResults = api.FindArtist("Savant");
            Assert.IsTrue(artistResults.Count() > 0);
        }

        [TestMethod]
        public void ArtistSearchCorrectFirstResult()
        {
            Spotify api = new Spotify();

            var artistResults = api.FindArtist("Savant");
            var artist = artistResults.OrderByDescending(a => a.Popularity).First();
            Assert.IsTrue(artist.Name == "Savant" && artist.Uri == "spotify:artist:5RBdF1pJSLF3ugc2Y2PoB8");
        }

        [TestMethod]
        public void AlbumSearchFindsResults()
        {
            Spotify api = new Spotify();

            var albumResults = api.FindAlbum("Under The Table And Dreaming");
            Assert.IsTrue(albumResults.Count() > 0);
        }

        [TestMethod]
        public void AlbumSearchCorrectFirstResult()
        {
            Spotify api = new Spotify();

            var albumResults = api.FindAlbum("Under The Table And Dreaming");
            var album = albumResults.OrderByDescending(a => a.Popularity).First();
            Assert.IsTrue(album.Name == "Under The Table And Dreaming" && 
                album.Uri == "spotify:album:3eAA4fvTVttgUlE43vRVMq" && 
                album.ArtistUri == "spotify:artist:2TI7qyDE0QfyOlnbtfDo7L");
        }

        [TestMethod]
        public void TrackSearchFindsResults()
        {
            Spotify api = new Spotify();

            var trackResults = api.FindTrack("Moon Trance");
            Assert.IsTrue(trackResults.Count() > 0);
        }

        [TestMethod]
        public void TrackSearchCorrectFirstResult()
        {
            Spotify api = new Spotify();

            var trackResults = api.FindTrack("Moon Trance");
            var track = trackResults.OrderByDescending(a => a.Popularity).First();
            Assert.IsTrue(track.Name == "Moon Trance" &&
                track.Uri == "spotify:track:3hG8BApT5ep4mdGleYiCdL" &&
                track.ArtistUri == "spotify:artist:378dH6EszOLFShpRzAQkVM" &&
                track.AlbumUri == "spotify:album:3YTWAm90osBvLNWCdF8Nq2");
        }
    }
}
