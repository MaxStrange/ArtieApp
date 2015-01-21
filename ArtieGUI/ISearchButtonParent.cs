using BeowulfCluster;
using DataBase;
using PerceptionSets;
using Search;
using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtieGUI
{
    public interface ISearchButtonParent
    {
        TextBox armXPosTextBox
        {
            get;
        }

        TextBox armYPosTextBox
        {
            get;
        }

        TextBox armZPosTextBox
        {
            get;
        }

        TextBox armNXTextBox
        {
            get;
        }

        TextBox armNYTextBox
        {
            get;
        }

        TextBox armNZTextBox
        {
            get;
        }

        TextBox armOXTextBox
        {
            get;
        }

        TextBox armOYTextBox
        {
            get;
        }

        TextBox armOZTextBox
        {
            get;
        }

        TextBox armPXTextBox
        {
            get;
        }

        TextBox armPYTextBox
        {
            get;
        }

        TextBox armPZTextBox
        {
            get;
        }

        CalibrationData calibrationData
        {
            get;
        }

        P_S_Arm currentArmPosition
        {
            get;
        }

        P_S_Body currentPosition
        {
            get;
        }

        DataBaseAccessor DBAccess
        {
            get;
            set;
        }

        ToolStripMenuItem DistributeToolStripMenuItem
        {
            get;
        }

        bool keepSearchPollingFlag
        {
            get;
            set;
        }

        TCP_IP_Client michelleClient
        {
            get;
            set;
        }

        TextBox NxTextBox
        {
            get;
        }

        TextBox NyTextBox
        {
            get;
        }

        TextBox PosXTextBox
        {
            get;
        }

        TextBox PosYTextBox
        {
            get;
        }

        TextBox PosZTextBox
        {
            get;
        }

        void refreshSearchTextByPolling(ref SearchPartitionToGUI spg);

        bool searching
        {
            get;
            set;
        }
        
        SearchPartitionToGUI searchPartitionToGUI
        {
            get;
            set;
        }

        bool searchStopRequested
        {
            get;
            set;
        }

        TrackBar TrackBar1
        {
            get;
        }

        ToolStripMenuItem UseDataBaseToolStripMenuItem
        {
            get;
        }
    }
}
