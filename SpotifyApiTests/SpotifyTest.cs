using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpotifyApi;
using SpotifyApi.Entities;

namespace SpotifyApiTests
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
    }
}
