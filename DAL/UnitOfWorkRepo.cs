using WebApiProject.Data;

namespace WebApiProject.DAL
{
    public class UnitOfWorkRepo : IUnitOfWork
    {
        private readonly ApplicationDBContext _dBContext;

        public ICategory Categories { get; }
        public ICustomer Customers { get; }
        public IOrder Orders { get; }
        public IOrderDetail OrderDetails { get; }
        public IProduct Products { get; }

        public IUser Users { get; }

        public UnitOfWorkRepo(ApplicationDBContext dBContext, IProduct product, IOrderDetail orderDetails, IOrder orders, ICustomer customers, ICategory categories, IUser users)
        {
            _dBContext = dBContext;
            Products = product;
            OrderDetails = orderDetails;
            Customers = customers;
            Orders = orders;
            Categories = categories;
            Users = users;
        }

        public void Dispose()
        {
            _dBContext.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _dBContext.SaveChangesAsync();
        }
    }
}
