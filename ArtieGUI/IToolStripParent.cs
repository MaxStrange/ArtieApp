using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using BeowulfCluster;
using PerceptionSets;

namespace ArtieGUI
{
    public interface IToolStripParent
    {
        P_S_Arm currentArmPosition
        {
            get;
        }

        DataBaseAccessor DBAccess
        {
            get;
            set;
        }

        TCP_IP_Client michelleClient
        {
            get;
            set;
        }

        void appendToDataLog(String text);
    }
}
