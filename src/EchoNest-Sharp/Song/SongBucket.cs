using System;
using System.ComponentModel;

namespace EchoNest.Song
{
    [Flags]
    public enum SongBucket
    {
        /// <summary>
        ///     1= audio_summary
        /// </summary>
        [Description("audio_summary")]
        AudioSummary = 1,

        /// <summary>
        ///     2= artist_familiarity
        /// </summary>
        [Description("artist_familiarity")]
        ArtistFamiliarity = 2,

        /// <summary>
        ///     4= artist_hotttnesss
        /// </summary>
        [Description("artist_hotttnesss")]
        ArtistHotttness = 4,

        /// <summary>
        ///     8= artist_location
        /// </summary>
        [Description("artist_location")]
        ArtistLocation = 8,

        /// <summary>
        ///     16= song_hotttnesss
        /// </summary>
        [Description("song_hotttnesss")]
        SongHotttness = 16,

        /// <summary>
        ///     32= tracks
        /// </summary>
        [Description("tracks")]
        Tracks = 32,

        /// <summary>
        ///     16384= id:musicbrainz
        /// </summary>
        [Description("id:musicbrainz")]
        IdMusicBrainz = 16384,

        /// <summary>
        ///     32768= id:playme
        /// </summary>
        [Description("id:playme")]
        IdPlayme = 32768,

        /// <summary>
        ///     65536= id:7digital
        /// </summary>
        [Description("id:7digital")]
        Id7digital = 65536,

        /// <summary>
        ///     131072= id:rdio-us-streaming
        /// </summary>
        [Description("id:rdio-us-streaming")]
        IdRdioUsStreaming = 131072
    }
}