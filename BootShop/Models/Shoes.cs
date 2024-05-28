using System.ComponentModel.DataAnnotations;

namespace BootShop.Models
{
    public class Shoes
    {
        [Key]
        public int Id { get; set; }
        public string product_name { get; set; }
        public string brand_name { get; set; }
        public string article { get; set; }
        public int price { get; set; }
        public string size1 { get; set; }
        public string size2 { get; set; }
        public string size3 { get; set; }
        public string season { get; set; }
        public string description { get; set; }
        public string gender { get; set; }
        public string color { get; set; }
        public string country { get; set; }
        public string composition { get; set; }
        public string image_url1 { get; set; }
        public string image_url2 { get; set; }
        public string image_url3 { get; set; }
        public string image_url4 { get; set; }
        public string image_url5 { get; set; }
        public string image_url6 { get; set; }
        public string image_url7 { get; set; }
    }
}