using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Entity
{
    public class Laptop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ram { get; set; }
        public int Storage { get; set; }
        
        public string Poster { get; set; }

    }
}
