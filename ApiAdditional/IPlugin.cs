namespace ApiAdditional
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Plugin for api classes
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Name of api class
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Method
        /// </summary>
        Task<List<Country>> Do();
    }
}
