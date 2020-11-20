using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AngularProjectAPI.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; }

        //Relations
        [JsonIgnore]
        public ICollection<Article> Articles { get; set; }
    }
}
