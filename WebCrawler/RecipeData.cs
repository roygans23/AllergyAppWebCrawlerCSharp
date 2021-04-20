using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler
{
    public class RecipeData
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public int Calories { get; private set; }

        public string[] Ingredients { get; private set; }

        public string[] instructions { get; private set; }

        public string Category { get; private set; }

        public string Author { get; private set; }

        public DateTime DatePublished { get; private set; }

        public string ImageUrl { get; private set; }
    }
}
