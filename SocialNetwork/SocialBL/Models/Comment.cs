using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Models
{
    public class Comment 
    {
        public string Id { get; set; }
        
        public string Content { get; set; }

        public DateTime Date { get; set; }

        public List<string> Mentions { get; set; }

        public string ImageUrl { get; set; }

        public int Likes { get; set; }
    }
}
