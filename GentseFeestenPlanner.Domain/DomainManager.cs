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

        public Dictionary<int, string> GetUserNames()
        {
            return _userRepository.GetAllUsers().ToDictionary(x => x.UserId, x => (x.FirstName + " " + x.LastName).ToString());
            
        }

    }
}
