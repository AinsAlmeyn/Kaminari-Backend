using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMS.Core.ContextInterfaces;
using WMS.Core.Entities.Api.Watch2GetherApi.BaseObjects;
using WMS.Core.RepositoryInterfaces;
using WMS.DataAccess.RepositoryEntities;

namespace WMS.DataAccess.RepositoryArticles
{
    public class TogetherRepository : GenericRepository<TogetherRoom>, ITogetherRepository
    {
        public TogetherRepository(IMongoDbContext _context) : base(_context)
        {
        }
    }
}