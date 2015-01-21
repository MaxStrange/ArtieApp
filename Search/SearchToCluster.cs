using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeowulfCluster;

namespace Search
{
    public class SearchToCluster
    {
        private TCP_IP_Client tcpIPClient = null;

        public SearchToCluster(TCP_IP_Client tcp)
        {
            this.tcpIPClient = tcp;
        }
    }
}
