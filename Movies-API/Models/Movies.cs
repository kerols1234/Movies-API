using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies_API.Models
{
    public class Movies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Release { get; set; }
        public double Duration { get; set; }
    }
}