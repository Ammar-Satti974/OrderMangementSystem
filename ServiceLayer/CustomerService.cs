using WebApiProject.DAL;
using WebApiProject.Models;
namespace WebApiProject.ServiceLayer
{
    public class CustomerService: ICustomerService
    {
       private readonly IUnitOfWork _unitOfWork;
        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _unitOfWork.Customers.GetAllAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _unitOfWork.Customers.GetByIdAsync(id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _unitOfWork.Customers.UpdateAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _unitOfWork.Customers.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

    }
}
