using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebAPIBatch.Models
{
    [CollectionDataContract(Name = "ResultSet", Namespace = "urn:yahoo:jp:news")]
    public class YahooNewsResultSet : List<YahooNewsResult>
    {
    }
}