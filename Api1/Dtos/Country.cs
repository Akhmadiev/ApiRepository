namespace Api1
{
    /// <summary>
    /// Country's dto
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// Adminregion
        /// </summary>
        public Region Adminregion { get; set; }

        /// <summary>
        /// IncomeLevel
        /// </summary>
        public Region IncomeLevel { get; set; }

        /// <summary>
        /// LendingType
        /// </summary>
        public Region LendingType { get; set; }

        /// <summary>
        /// CapitalCity
        /// </summary>
        public string CapitalCity { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public string Latitude { get; set; }
    }
}
