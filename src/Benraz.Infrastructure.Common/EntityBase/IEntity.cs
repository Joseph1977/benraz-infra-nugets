namespace Benraz.Infrastructure.Common.EntityBase
{
    /// <summary>
    /// Entity.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        TKey Id { get; set; }
    }
}




