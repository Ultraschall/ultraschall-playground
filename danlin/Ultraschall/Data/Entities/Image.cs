namespace Ultraschall.Data.Entities
{
    public class Image : AnnotationBase
    {
        public string Mime { get; set; }
        public string Author { get; set; }
        public string License { get; set; }
        public string Copyright { get; set; }
    }
}