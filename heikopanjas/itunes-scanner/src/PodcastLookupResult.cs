using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ultraschall_scanner
{
  [DataContract]
  public class PodcastLookupResult
  {
    [DataMember(Name = "resultCount")]
    public int ResultCount {
      get; set;
    } = 0;

    [DataMember(Name = "results")]
    public Podcast[] Results {
      get; set;
    } = null;
  }
}
