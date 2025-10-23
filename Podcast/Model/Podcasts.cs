using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Model
{
    class Podcasts
    {
        private string name;
        private string description;
        private string date;

        private Podcasts() { }

        //getters and setters
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string Date { get { return date; } set { date = value; } }

    }
}
