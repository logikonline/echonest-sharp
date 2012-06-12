using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class Fingerprint
    {
        [DataMember(Name = "metadata")]
        public FingerprintMetadata Metadata { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }
    }
}