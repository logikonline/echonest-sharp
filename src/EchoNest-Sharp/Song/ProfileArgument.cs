using System.Collections.Generic;

namespace EchoNest.Song
{
    public class ProfileArgument
    {
        #region Constructors

        public ProfileArgument()
        {
            SongIds = new List<string>();
            TrackIds = new List<string>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     the song ID. An Echo Nest ID or a Rosetta ID (See Project Rosetta Stone)
        /// </summary>
        public List<string> SongIds { get; private set; }

        /// <summary>
        ///     The Echo Nest or Rosetta ID of a track. The track ID is mapped to the corresponding song ID. (See Project Rosetta Stone)
        /// </summary>
        public List<string> TrackIds { get; private set; }

        public SongBucket? Bucket { get; set; }

        /// <summary>
        /// if 'true', limit the results to any of the given idspaces or catalogs
        /// </summary>
        public bool Limit { get; set; }

        internal string ApiKey { get; set; }

        internal string BaseUrl { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            UriQuery query = new UriQuery(BaseUrl);
            query.Add("api_key", ApiKey);
            query.Add("format", "json");

            if (SongIds.Count > 0)
            {
                foreach (string songId in SongIds)
                {
                    query.Add("id", songId);
                }
            }

            if (TrackIds.Count > 0)
            {
                foreach (string trackId in TrackIds)
                {
                    query.Add("track_id", trackId);
                }
            }

            if (Bucket.HasValue)
            {
                foreach (string bucket in Bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucket);
                }
            }

            if (Limit)
            {
                query.Add("limit", Limit.ToString().ToLower());
            }

            return query.ToString();
        }

        #endregion Methods
    }
}