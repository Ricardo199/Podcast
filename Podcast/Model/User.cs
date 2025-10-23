using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Podcast.Model
{
    public class User
    {
        private string _id;
        private string _name;
        private string _email;
        private string _password;
        private string[] _podcasts;
        private bool _isDeleted;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string[] Podcasts { get; set; }
        public bool IsDeleted { get; set; }
    }



}
