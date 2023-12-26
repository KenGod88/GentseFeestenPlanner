using GentseFeestenPlanner.Database;
using GentseFeestenPlanner.Domain;
using GentseFeestenPlanner.Domain.Repository;
using GentseFeestenPlanner.WPF;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GentseFeestenPlanner.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            IUserRepository userRepository = new UserMapper();
            IEventRepository eventRepository = new EventMapper();
            DomainManager domainManager = new DomainManager(userRepository, eventRepository);
            GentseFeestenApplication gentseFeestenApplication = new GentseFeestenApplication(domainManager);

            


        }
    }

}
