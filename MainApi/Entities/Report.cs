namespace MainApi.Entities
{
    using ApiAdditional;
    using MainApi.Enums;
    using System;

    public class Report : Entity
    {
        /// <summary>
        /// ReportStatus
        /// </summary>
        public ReportStatus ReportStatus { get; set; }

        /// <summary>
        /// ReportId
        /// </summary>
        public string ReportId { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
