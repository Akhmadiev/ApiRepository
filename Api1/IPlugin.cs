namespace Api1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlugin1
    {
        string Name { get; }

        Task<List<T>> Do<T>() where T : class;
    }
}
