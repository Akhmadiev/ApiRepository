namespace MainApi.Enums
{
    public enum ReportStatus
    {
        /// <summary>
        /// Report in state queue
        /// </summary>
        Queue = 0,

        /// <summary>
        /// Report started
        /// </summary>
        Started = 10,

        /// <summary>
        /// Report finished
        /// </summary>
        Finished = 20
    }
}
