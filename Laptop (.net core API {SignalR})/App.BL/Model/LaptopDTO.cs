using Microsoft.AspNetCore.Http;

namespace App.BL.Model
{
    public class LaptopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ram { get; set; }
        public int Storage { get; set; }
        public string Poster { get; set; }
    }
}
