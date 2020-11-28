using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public int ArticleID { get; set; }
        public Article Article { get; set; }
        public string Text { get; set; }
    }
}
