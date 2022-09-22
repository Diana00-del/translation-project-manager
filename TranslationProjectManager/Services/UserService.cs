using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;

namespace TranslationProjectManager.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        private readonly TranslationProjectManagerContext context;

        public UserService(TranslationProjectManagerContext context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetAll()
        {
            return context.Users.Include(u => u.TranslationProjects).Include(u => u.TranslatorAssignments);
        }

        public User GetById(int id)
        {
            return context.Users.Find(id);
        }
    }
}
