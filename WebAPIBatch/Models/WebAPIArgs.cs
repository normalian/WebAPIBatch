using System.Collections.Generic;

namespace WebAPIBatch.Models
{
    public class WebAPIArgs
    {
        public string FromMailName { get; set; }
        public string FromMailAddress { get; set; }
        public IList<string> ToMailAddresses { get; set; }
    }
}