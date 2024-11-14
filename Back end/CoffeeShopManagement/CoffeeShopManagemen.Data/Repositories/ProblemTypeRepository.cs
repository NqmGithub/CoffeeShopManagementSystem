using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.Repositories
{
    public class ProblemTypeRepository : GenericRepository<ProblemType>, IProblemTypeRepository
    {
        public ProblemTypeRepository(CoffeeShopDbContext db) : base(db)
        {
        }
    }
}
