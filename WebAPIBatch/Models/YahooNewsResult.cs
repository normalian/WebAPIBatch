using System;
using System.Runtime.Serialization;

namespace WebAPIBatch.Models
{
    [DataContract(Name = "Result", Namespace = "urn:yahoo:jp:news")]
    public class YahooNewsResult
    {
        [DataMember(Name = "CreateTime", Order = 2)]
        public DateTime CreateTime { get; set; }

        [DataMember(Name = "Title", Order = 6)]
        public string Title { get; set; }

        [DataMember(Name = "Overview", Order = 10)]
        public string Overview { get; set; }

        [DataMember(Name = "SmartphoneUrl", Order = 21)]
        public Uri SmartphoneUrl { get; set; }
    }
}