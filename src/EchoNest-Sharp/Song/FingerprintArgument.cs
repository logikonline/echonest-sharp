namespace EchoNest.Song
{
    public class FingerprintArgument : Fingerprint
    {
        internal string ApiKey { get; set; }

        internal string BaseUrl { get; set; }

        public SongBucket? Bucket { get; set; }

        #region Methods

        public override string ToString()
        {
            UriQuery query = new UriQuery(BaseUrl) {{"api_key", ApiKey}, {"format", "json"}};

            if (Metadata != null)
            {
                if (!string.IsNullOrEmpty(Metadata.Artist))
                {
                    query.Add("artist", Metadata.Artist);
                }

                if (!string.IsNullOrEmpty(Metadata.Title))
                {
                    query.Add("title", Metadata.Title);
                }
            }

            if (Bucket.HasValue)
            {
                foreach (string bucket in Bucket.Value.GetBucketDescriptions())
                {
                    query.Add("bucket", bucket);
                }
            }

            if (!string.IsNullOrEmpty(Code))
            {
                query.Add("code", Code);
            }

            return query.ToString();
        }

        #endregion Methods
    }
}