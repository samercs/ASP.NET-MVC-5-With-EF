using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CrossOver.Data;
using CrossOver.Entity;

namespace CrossOver.Service
{
    public class StockCodeService: ServiceBase
    {
        public StockCodeService(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
        }


        public async Task<IEnumerable<StockCode>> GetAll()
        {
            using (var dc = DataContext())
            {
                return await dc.StockCodes.ToListAsync();
            }
        }

        public async Task<StockCode> Update(StockCode stockCode)
        {
            using (var dc = DataContext())
            {
                dc.SetModified(stockCode);
                await dc.SaveChangeAsyn();
                return stockCode;
            }
        }

        public async Task AddStockToUser(User user, IList<int> stockCodes)
        {
            using (var dc = DataContext())
            {
                foreach (var stockCode in stockCodes)
                {
                    var stockCodeData = await dc.StockCodes
                        .Include(i => i.Users)
                        .FirstOrDefaultAsync(i => i.StockCodeId == stockCode);

                    if (!stockCodeData.Users.Contains(user))
                    {
                        stockCodeData.Users.Add(user);
                        dc.SetModified(stockCodeData);
                    }
                    
                    
                }

                await dc.SaveChangeAsyn();
            }
        }
    }
}