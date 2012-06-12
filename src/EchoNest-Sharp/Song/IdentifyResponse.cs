using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class IdentifyResponse : Response
    {
        [DataMember(Name = "songs")]
        public List<SongBucketItem> Songs { get; set; }

        
    }
}