using WebApiProject.DAL;
using WebApiProject.Models;

namespace WebApiProject.ServiceLayer
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailsByIdAsync(int id);
        Task AddOrderDetailsAsync(OrderDetail orderDetails);
        Task UpdateOrderDetailsAsync(OrderDetail orderDetails);
        Task DeleteOrderDetailsAsync(int id);
    }
}
