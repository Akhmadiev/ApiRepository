namespace MainApi.Entities
{
    using ApiAdditional;

    public class Product : Entity
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } 

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
