using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ultraschall_scanner
{
  [DataContract]
  public class Podcast
  {
    [DataMember(Name = "id")]
    public Guid Guid {
      get; set;
    } = Guid.NewGuid();

    [DataMember(Name = "collectionId")]
    public String CollectionId {
      get; set;
    } = null;

    [DataMember(Name = "collectionName")]
    public String CollectionName {
      get; set;
    } = null;

    [DataMember(Name = "artistName")]
    public String ArtistName {
      get; set;
    } = null;

    [DataMember(Name = "feedUrl")]
    public String FeedUrl {
      get; set;
    } = null;

    [DataMember(Name = "releaseDate")]
    public DateTime ReleaseDate {
      get; set;
    } = DateTime.Now;

    [DataMember(Name = "trackCount")]
    public int TrackCount {
      get; set;
    } = 0;

    [DataMember(Name = "country")]
    public String Country {
      get; set;
    } = null;
  }
}
