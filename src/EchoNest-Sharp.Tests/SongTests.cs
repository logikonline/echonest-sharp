using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using EchoNest.Artist;
using EchoNest.Song;
using NUnit.Framework;
using Search = EchoNest.Song.Search;
using SearchArgument = EchoNest.Song.SearchArgument;
using SearchResponse = EchoNest.Song.SearchResponse;

namespace EchoNest.Tests
{
    [TestFixture]
    public class SongTests
    {
        [Test]
        public void GetSongs_WhereDescription_Christmas_IsNotNull()
        {
            //arrange
            const string description = "christmas";

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                // arrange
                SearchArgument searchArgument = new SearchArgument {Results = 10, Bucket = SongBucket.ArtistHotttness, Sort = "artist_familiarity-desc"};

                searchArgument.Description.AddRange(new TermList {description}); 

                //act
                SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);

                //assert
                Assert.IsNotNull(searchResponse);

                // output
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
            }
        }


        [TestCase("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5", "Hendrix")]
        [TestCase("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5", "Floyd")]
        public void GetSongs_ForApocalypseNow_ExpectedArtist(string title, string styles, string moods, string expect)
        {
            // arrange
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            TermList moodTerms = new TermList();
            foreach (string s in moods.Split(','))
            {
                moodTerms.Add(s);
            }

            SearchArgument searchArgument = new SearchArgument
            {
                Mode = "0", /* minor */
                Sort = "artist_familiarity-desc",
                Results = 10
            };

            searchArgument.Styles.AddRange(styleTerms);

            searchArgument.Moods.AddRange(moodTerms);

            searchArgument.Description.AddRange(new TermList() {"80s"});
            
            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);

                //assert
                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);

                var matches = (from s in searchResponse.Songs
                               where s.ArtistName.ToLower().Contains(expect)
                               select s).ToList();
                

                Assert.IsNotNull(matches, "Failed to find songs where artist name contains: {0}", expect);

                // output
                Console.WriteLine("Tracks for '{0}'", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        [TestCase("James", "jazz", "James")]
        public void GetSongs_By_ArtistName_Mood(string name, string styles, string expect)
        {
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            SearchArgument searchArgument = new SearchArgument
            {
                Sort = "artist_familiarity-desc",
                Results = 10
            };

            //searchArgument.Styles.AddRange(styleTerms);

            searchArgument.Artist = name;

            searchArgument.MinTempo = 90;

            searchArgument.MaxTempo = 140;

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);

                //assert
                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);

                var matches = (from s in searchResponse.Songs
                               where s.ArtistName.ToLower().Contains(expect)
                               select s).ToList();

                Assert.IsNotNull(matches, "Failed to find songs where artist name contains: {0}", expect);

                // output
                Console.WriteLine("Tracks for '{0}'", name);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        [TestCase("Female Vocalist^0.8,Jazz^0.5", "50s", "ella")]
        public void GetFemaleVocalist_Genre_Decade(string styles, string decade, string expect)
        {
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            SearchArgument searchArgument = new SearchArgument
            {
                Sort = "artist_familiarity-desc",
                Results = 30
            };

            searchArgument.Styles.AddRange(styleTerms);

            searchArgument.Description.AddRange(new TermList {decade});

            searchArgument.MinTempo = 90;

            searchArgument.MaxTempo = 140;

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);

                //assert
                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);

                var matches = (from s in searchResponse.Songs
                               where s.ArtistName.ToLower().Contains(expect)
                               select s).ToList();

                Assert.IsNotNull(matches, "Failed to find songs where artist name contains: {0}", expect);

                // output
                Console.WriteLine("Tracks for '{0}'", styles);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        [TestCase("eJxVlIuNwzAMQ1fxCDL133-xo1rnGqNAEcWy_ERa2aKeZmW9ustWVYrXrl5bthn_laFkzguNWpklEmoTB74JKYZSPlbJ0sy9fQrsrbEaO9W3bsbaWOoK7IhkHFaf_ag2d75oOQSZczbz5CKA7XgTIBIXASvFi0A3W8pMUZ7FZTWTVbujCcADlQ_f_WbdRNJ2vDUwSF0EZmFvAku_CVy440fgiIvArWZZWoJ7GWd-CVTYC5FCFI8GQdECdROE20UQfLoIUmhLC7IiByF1gzbAs3tsSKctyC76MPJlHRsZ5qhSQhu_CJFcKtW4EMrHSIrpTGLFqsdItj1H9JYHQYN7W2nkC6GDPjZTAzL9dx0fS4M1FoROHh9YhLHWdRchQSd_CLTpOHkQQP3xQsA2-sLOUD7CzxU0GmHVdIxh46Oide0NrNEmjghG44Ax_k2AoDHsiV6WsiD6OFm8y-0Lyt8haDBBzeMlAnTuuGYIB4WA2lEPAWbdeOabgFN6TQMs6ctLA5fHyKMBB0veGrjPfP00IAlWNm9n7hEh5PiYYBGKQDP-x4F0CL8HkhoQnRWN997JyEpnHFR7EhLPQMZmgXS68hsHktEVErranvSSR2VwfJhQCnkuwhBUcINNY-xu1pmw3PmBqU9-8xu0kiF1ngOa8vwBSSzzNw==")]
        public void IdentifySongByFingerprintCode(string code)
        {
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                FingerprintArgument fingerprintArgument = new FingerprintArgument {Code = code};

                fingerprintArgument.Bucket = SongBucket.IdMusicBrainz | SongBucket.AudioSummary | SongBucket.ArtistFamiliarity;

                IdentifyResponse identifyResponse = session.Query<Identify>().Execute(fingerprintArgument);

                Assert.IsNotNull(identifyResponse);
                Assert.IsNotNull(identifyResponse.Songs);

                var song = identifyResponse.Songs.FirstOrDefault();

                Assert.IsTrue(song.Title.StartsWith("Billie Jean"));
                Assert.AreEqual("Michael Jackson", song.ArtistName);

                Console.WriteLine();

            }
        }

        [TestCase("SOHJOLH12A6310DFE5")]
        public void GetSongProfileById(string songId)
        {
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                ProfileArgument profileArgument = new ProfileArgument { SongIds = {songId}};

                profileArgument.Bucket = SongBucket.IdMusicBrainz | SongBucket.IdLyricFindUs| SongBucket.AudioSummary | SongBucket.ArtistFamiliarity;

                Song.ProfileResponse profileResponse = session.Query<Song.Profile>().Execute(profileArgument);

                Assert.IsNotNull(profileResponse);
                Assert.IsNotNull(profileResponse.Songs);

                var song = profileResponse.Songs.FirstOrDefault();

                Assert.IsTrue(song.Title.StartsWith("Karma Police"));
                Assert.AreEqual("Radiohead", song.ArtistName);

                Assert.IsNotEmpty(song.ForeignIds);

                Console.WriteLine();

            }
        }
    }
}