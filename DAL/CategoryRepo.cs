using Microsoft.EntityFrameworkCore;
using WebApiProject.Models;
using WebApiProject.Data;

namespace WebApiProject.DAL
{
    public class CategoryRepo: Repo<Category>, ICategory
    {
        public CategoryRepo(ApplicationDBContext dbContext): base(dbContext) { }
    }
}
