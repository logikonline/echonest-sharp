using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract]
    public class ForeignId
    {
        [DataMember(Name = "catalog")]
        public string Catalog { get; set; }

        [DataMember(Name = "foreign_id")]
        public string Id { get; set; }
    }
}