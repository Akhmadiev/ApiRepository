namespace MainApi.Reports
{
    using MainApi.Interfaces;

    public class BaseReport
    {
        public IRepository Repository { get; set; }

        public ILogger Logger { get; set; }
    }
}
