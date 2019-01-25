using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
        }

        public IndexViewModel(IEnumerable<PostWithCommentsModel> userFeed, IEnumerable<UserIdAndName> searchPeople, FullUser userDetails)
        {
            UserFeed = userFeed;
            SearchPeople = searchPeople;
            UserDetails = userDetails;
        }

        public IEnumerable<PostWithCommentsModel> UserFeed {get;set ;}
        [Display(Name ="Find People")]
        public IEnumerable<UserIdAndName> SearchPeople { get; set; }
        public FullUser UserDetails { get; set; }
    }
}