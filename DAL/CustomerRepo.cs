using WebApiProject.Data;
using WebApiProject.Models;
namespace WebApiProject.DAL
{
    public class CustomerRepo: Repo<Customer>, ICustomer
    {
        public CustomerRepo(ApplicationDBContext dBContext): base(dBContext) { }
    }
}
