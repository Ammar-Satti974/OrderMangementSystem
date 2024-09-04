using WebApiProject.Data;
using WebApiProject.Models;

namespace WebApiProject.DAL
{
    public class ProductRepo : Repo<Product>, IProduct
    {
        public ProductRepo(ApplicationDBContext dbContext) : base(dbContext) { }
    }
    
}
