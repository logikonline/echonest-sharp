using System.Runtime.Serialization;

namespace EchoNest.Song
{
    [DataContract(Name = "metadata")]
    public class FingerprintMetadata
    {
        [DataMember(Name = "artist")]
        public string Artist { get; set; }

        [DataMember(Name = "release")]
        public string Release { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "genre")]
        public string Genre { get; set; }

        [DataMember(Name = "bitrate")]
        public string Bitrate { get; set; }

        [DataMember(Name = "sample_rate")]
        public string Samplerate { get; set; }

        [DataMember(Name = "duration")]
        public string Duration { get; set; }

        [DataMember(Name = "filename")]
        public string Filename { get; set; }

        [DataMember(Name = "samples_decoded")]
        public string SamplesDecoded { get; set; }

        [DataMember(Name = "given_duration")]
        public string GivenDuration { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }
    }
}