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
        private DomainManager _domainManager;

        public GentseFeestenApplication(DomainManager domainManager)
        {
            _domainManager = domainManager;
        }
    }
}
