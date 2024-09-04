namespace WebApiProject.DAL
{
    public interface IUnitOfWork: IDisposable
    {
        ICategory Categories { get; }
        ICustomer Customers { get; }
        IOrder Orders { get; }
        IOrderDetail OrderDetails { get; }
        IProduct Products { get; }
        IUser Users { get; }

        Task<int> SaveAsync();
    }
}
