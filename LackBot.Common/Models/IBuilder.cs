namespace LackBot.Common.Models
{
    public interface IBuilder<T>
    {
        public T Build();
    }
}