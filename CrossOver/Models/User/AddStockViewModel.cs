using System.Collections.Generic;
using CrossOver.Entity;

namespace CrossOver.Models.User
{
    public class AddStockViewModel
    {
        public IList<StockCode> UserStockCodes { get; set; }
        public IEnumerable<StockCode> AllStockCodes { get; set; }
    }
}