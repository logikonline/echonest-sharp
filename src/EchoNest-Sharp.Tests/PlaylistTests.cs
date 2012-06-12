using System;
using System.Configuration;
using System.Linq;
using EchoNest.Playlist;
using EchoNest.Song;
using NUnit.Framework;

namespace EchoNest.Tests
{
    [TestFixture]
    public class PlaylistTests
    {
        /*[TestCase("Jimi Hendrix")]
        [TestCase("Amy Winehouse")]
        [TestCase("Miles Davis")]
        [TestCase("Alison Krauss")]
        [TestCase("Led Zeppelin")]*/
        [TestCase("Ella Fitzgerald")]
        [Test]
        public void GetBasicPlaylist_WhereArtistName_HasSongsByArtist(string artistName)
        {
            //arrange
            BasicArgument basicArgument = new BasicArgument
            {
                Results = 30,
                Dmca = true
            };

            TermList artistTerms = new TermList { artistName };
            basicArgument.Artist.AddRange(artistTerms);
               

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
               //act
                PlaylistResponse searchResponse = session.Query<Basic>().Execute(basicArgument);

                //assert
                Assert.IsNotNull(searchResponse);

                // output
                Console.WriteLine("Songs for : {0}", artistName);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
            }
        }

        [TestCase("Ella Fitzgerald")]
        [Test]
        public void GetStaticArtistRadioPlaylist_WhereArtistName_HasSongsByArtist(string artistName)
        {
            //arrange
            BasicArgument argument = new StaticArgument()
            {
                Results = 30
            };

            TermList artistTerms = new TermList { artistName };
            argument.Artist.AddRange(artistTerms);

            argument.Type = "artist-radio";

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                //act
                PlaylistResponse searchResponse = session.Query<Basic>().Execute(argument);

                //assert
                Assert.IsNotNull(searchResponse);

                // output
                Console.WriteLine("Songs for : {0}", artistName);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
            }
        }



        [TestCase("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5")]
        [TestCase("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5")]
        [Test]
        public void GetStaticPlaylist_WhereMoodAndStyle_HasVarietyOfArtists(string title, string styles, string moods)
        {
            //arrange
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
            
            StaticArgument staticArgument = new StaticArgument
            {
                Results = 25,
                Adventurousness = 0.4,
                Type = "artist-description",
                Variety = 0.4 /* variety of artists */
            };

            staticArgument.Styles.AddRange(styleTerms);

            staticArgument.Moods.AddRange(moodTerms);
            
            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                //act
                PlaylistResponse searchResponse = session.Query<Static>().Execute(staticArgument);

                //assert
                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);
                Assert.IsTrue(searchResponse.Songs.Any());

                // output
                Console.WriteLine("Songs for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
            }
        }

        [TestCase("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5")]
        [TestCase("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5")]
        [Test]
        public void GetDynamicPlaylist_WhereMoodAndStyle_CanSteerDynamicPlaylistByMood(string title, string styles, string moods)
        {
            //arrange
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

            DynamicArgument dynamicArgument = new DynamicArgument
            {
                Adventurousness = 0.4,
                Type = "artist-description",
                Variety = 0.4 /* variety of artists */
            };

            dynamicArgument.Styles.AddRange(styleTerms);

            dynamicArgument.Moods.AddRange(moodTerms);

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                //act
                DynamicPlaylistResponse searchResponse = session.Query<Dynamic>().Execute(dynamicArgument);

                //assert
                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);
                Assert.IsTrue(searchResponse.Songs.Any());

                // arrange next part of test
                string sessionId = searchResponse.SessionId;

                // output
                Console.WriteLine("Dynamic Playlist Session Id: {0}", sessionId);
                Console.WriteLine();
                Console.WriteLine("Songs for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
                Console.WriteLine("Steering Playlist by mood = -happy");
                Console.WriteLine();

                dynamicArgument = new DynamicArgument { SteerMood = "-happy", SessionId = sessionId };
                searchResponse = session.Query<Dynamic>().Execute(dynamicArgument);
                Console.WriteLine("Next song in dynamic playlist for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
                Console.WriteLine("Steering Playlist by tempo = +10% (tempo^1.1)");
                Console.WriteLine();
                dynamicArgument = new DynamicArgument { Steer = "tempo^1.1", SessionId = sessionId };
                searchResponse = session.Query<Dynamic>().Execute(dynamicArgument);
                Console.WriteLine("Next song in dynamic playlist for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }
                Console.WriteLine();
            }
        }


        [TestCase("Female Vocalist,Jazz", "50s", "1949", "1960", "ella fitzgerald")]
        public void GetFemaleVocalist_Genre_Decade(string styles, string decade, string artistStartYearAfter, string artistStartYearBefore, string expect)
        {
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            StaticArgument staticArgument = new StaticArgument
            {
                Results = 30,
                Type = "artist-description",
            };

            staticArgument.Styles.AddRange(styleTerms);

            staticArgument.Description.AddRange(new TermList { decade });

            //staticArgument.ArtistStartYearAfter = artistStartYearAfter;

            staticArgument.ArtistStartYearBefore = artistStartYearBefore;

            staticArgument.Description.AddRange(new TermList { decade });

            //act
            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                PlaylistResponse searchResponse = session.Query<Static>().Execute(staticArgument);

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

    }
}