namespace MainApi
{
    using ApiAdditional;
    using System.Collections.Generic;

    /// <summary>
    /// Country's interface
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Save entites
        /// </summary>
        void Save<T>(List<T> entities) where T : Entity;

        /// <summary>
        /// Save entity
        /// </summary>
        void Save<T>(T entity) where T : Entity;

        /// <summary>
        /// Parse result from json<string>
        /// </summary>
        List<Tto> ParsingResult<Tto, Tfrom>(List<Tfrom> data) where Tto : Entity where Tfrom : class;
    }
}