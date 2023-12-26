using GentseFeestenPlanner.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.Domain.Repository
{
    public interface IUserRepository
    {

        List<User> GetAllUsers();
        User GetUserById(int userId);
    }
}
