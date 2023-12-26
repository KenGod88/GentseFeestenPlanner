using GentseFeestenPlanner.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GentseFeestenPlanner.WPF
{
    public class GentseFeestenApplication
    {
        private MainWindow _mainWindow;
        private DomainManager _domainManager;

        public GentseFeestenApplication(DomainManager domainManager)
        {
            _domainManager = domainManager;

            _mainWindow = new MainWindow();
            _mainWindow.Show();

            _mainWindow.Users = _domainManager.GetAllUsers();

            


        }
    }
}
