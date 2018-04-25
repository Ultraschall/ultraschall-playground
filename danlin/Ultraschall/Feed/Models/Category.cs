using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Ultraschall.Feed.Models
{
    public class Category
    {
        public string Title { get; set; }
        public List<Category> SubCategories { get; set; }

        public Category(XElement node) : this()
        {
            Title = node.Value;

            var subcategories = from n in node.Elements()
                               where n.Name == "category"
                               select n;
            foreach (var subcategory in subcategories)
            {
               SubCategories.Add(new Category(subcategory));
            }
        }

        public Category()
        {
            SubCategories = new List<Category>();
        }
    }
}
