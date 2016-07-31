using System;
using System.Data.Entity;
using System.Threading.Tasks;
using CrossOver.Entity;

namespace CrossOver.Data
{
    public interface IDataContext: IDisposable
    {
        IDbSet<StockCode> StockCodes { get; set; }
        int SaveChange();
        Task<int> SaveChangeAsyn();
        void SetModified(object entity);
    }
}