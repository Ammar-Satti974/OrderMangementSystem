using WebApiProject.DAL;
using WebApiProject.Models;
namespace WebApiProject.ServiceLayer
{
    public class OrderDetailService: IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            return await _unitOfWork.OrderDetails.GetAllAsync();
        }

        public async Task<OrderDetail> GetOrderDetailsByIdAsync(int id)
        {
            return await _unitOfWork.OrderDetails.GetByIdAsync(id);
        }

        public async Task AddOrderDetailsAsync(OrderDetail orderDetail)
        {
            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateOrderDetailsAsync(OrderDetail orderDetail)
        {
            await _unitOfWork.OrderDetails.UpdateAsync(orderDetail);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteOrderDetailsAsync(int id)
        {
            await _unitOfWork.OrderDetails.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
