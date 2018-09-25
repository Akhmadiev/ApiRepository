namespace MainApi.Entities
{
    using ApiAdditional;

    public class MaxReport : Entity
    {
        /// <summary>
        /// Report
        /// </summary>
        public int Report { get; set; }

        /// <summary>
        /// Max
        /// </summary>
        public decimal Max { get; set; }

        /// <summary>
        /// Min
        /// </summary>
        public decimal Min { get; set; }

        /// <summary>
        /// Avg
        /// </summary>
        public decimal Avg { get; set; }
    }
}
