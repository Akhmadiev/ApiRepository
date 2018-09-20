namespace MainApi
{
    using System.Threading.Tasks;

    public interface IPlugin1
    {
        string Name { get; }

        Task<T> Do<T>() where T : class;
    }
}