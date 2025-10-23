namespace StyleAssistantCA1_SOA_MeghanKeightley
{

    public class StyleItem
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }

        public string ProductUrl { get; set; }
    }

    public class AsosApiResponse
    {
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public Price Price { get; set; }
        public string ImageUrl { get; set; }

        public string ProductUrl { get; set; }
    }

    public class Brand
    {
        public string Name { get; set; }
    }

    public class Price
    {
        public Current Current { get; set; }
    }

    public class Current
    {
        public string Text { get; set; }
    }
}
