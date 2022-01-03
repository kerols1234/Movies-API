using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_API.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public string body { get; set; }
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movies Movie { get; set; }
    }
}