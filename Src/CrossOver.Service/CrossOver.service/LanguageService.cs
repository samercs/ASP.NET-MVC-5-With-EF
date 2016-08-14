using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CrossOver.Data;
using CrossOver.Entity;

namespace CrossOver.Service
{
    public class LanguageService: ServiceBase
    {
        public LanguageService(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
        }

        public async Task<IEnumerable<Language>> GetAll()
        {
            using (var dc = DataContext())
            {
                return await dc.Languages.ToListAsync();
            }
        }

        public async Task<IEnumerable<Language>> GetByQuery(string query)
        {
            using (var dc = DataContext())
            {
                if (!string.IsNullOrEmpty(query))
                {
                    return await dc.Languages.Where(i => i.Name.ToLower().StartsWith(query.ToLower())).ToListAsync();
                }
                return await GetAll();

            }
        }
    }
}