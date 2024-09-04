using WebApiProject.Data;
using WebApiProject.Models;
namespace WebApiProject.DAL
{
    public class OrderDetailRepo : Repo<OrderDetail>, IOrderDetail
    {
        public OrderDetailRepo(ApplicationDBContext dbContext) : base(dbContext) { }
    }
    
}
