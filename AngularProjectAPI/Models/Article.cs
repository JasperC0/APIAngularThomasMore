using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ShortSummary { get; set; }
        public string Body { get; set; }

        //Relations
        public int TagID { get; set; }
        public Tag Tag { get; set; }
        public int UserID { get; set; }
        public User User { get; set; } //Creator
        public int ArticleStatusID { get; set; }
        public ArticleStatus ArticleStatus { get; set; }
    }
}
