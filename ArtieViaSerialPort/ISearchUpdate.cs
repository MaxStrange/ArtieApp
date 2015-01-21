using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtieViaSerialPort
{
    public interface ISearchUpdate
    {
        bool keepSearchPollingFlag
        {
            get;
            set;
        }

        void refreshSearchTextByPolling(ref SearchPartitionToGUI spg);
    }
}
