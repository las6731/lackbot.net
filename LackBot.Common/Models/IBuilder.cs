namespace LackBot.Common.Models
{
    /// <summary>
    /// Builds a new <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The type that will be built.</typeparam>
    public interface IBuilder<T>
    {
        /// <summary>
        /// Builds a new <see cref="T"/>.
        /// </summary>
        /// <returns>The new <see cref="T"/>.</returns>
        public T Build();
    }
}