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
        public int ReportId { get; set; }

        /// <summary>
        /// TemplateId
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}
