using GentseFeestenPlanner.Domain.DTO;
using GentseFeestenPlanner.Domain.Model;
using GentseFeestenPlanner.Domain.Repository;

namespace GentseFeestenPlanner.Domain
{
    public class DomainManager
    {
       private IUserRepository _userRepository;

        public DomainManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<string> GetAllUsers()
        {
            List<User> users = _userRepository.GetAllUsers();
            List<string> usersList = users.Select(user => user.UserId + ": " + user.FirstName + " " + user.LastName).ToList();
            return usersList;
            
        }

    }
}
