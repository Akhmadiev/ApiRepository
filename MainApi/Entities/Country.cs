namespace MainApi
{
    using MainApi.Enums;
    using System;

    /// <summary>
    /// Country entity
    /// </summary>
    public class Country : Entity
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CapitalCity
        /// </summary>
        public string CapitalCity { get; set; }

        /// <summary>
        /// Continent
        /// </summary>
        public ContinentType ContinentType { get; set; }

        /// <summary>
        /// Start data
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return DateTime.Now;
            }
            set
            {
                StartDate = value;
            }
        }
    }
}
