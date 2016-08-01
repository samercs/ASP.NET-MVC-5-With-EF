using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrossOver.Entity;

namespace CrossOver.Models.User
{
    public class IndexViewModel
    {
        public Entity.User User { get; set; }
        public IList<StockViewModel> UserStockList { get; set; }
        
    }

    public class StockViewModel
    {
        public StockCode StockCode { get; set; }
        public int Prices { get; set; }
    }
}