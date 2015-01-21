using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;

namespace ArtieGUI
{
    /// <summary>
    /// Contains all the things that the ControlPanelController requires of its parent.
    /// </summary>
    public interface IControlPanelParent
    {
        void refreshLabels();

        void refreshControlPanelText(string text);

        P_S_Body currentPosition
        {
            get;
        }
    }
}
