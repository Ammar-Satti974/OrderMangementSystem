using WebApiProject.Data;
using WebApiProject.Models;
namespace WebApiProject.DAL
{
    public class OrderRepo : Repo<Order>, IOrder
    {
        public OrderRepo(ApplicationDBContext dbContext) : base(dbContext) { }
    }
}
