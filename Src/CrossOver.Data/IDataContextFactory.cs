namespace CrossOver.Data
{
    public interface IDataContextFactory
    {
        IDataContext GetContext();
    }
}