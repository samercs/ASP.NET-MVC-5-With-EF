namespace CrossOver.Data
{
    public class DataContextFactory: IDataContextFactory
    {
        public IDataContext GetContext()
        {
            return new DataContext();
        }
    }
}