using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAbstractDataTypes;

namespace ArtieViaSerialPort
{
    public interface ICalibrationRequester
    {
        CalibrationData calibrationData
        {
            get;
            set;
        }
    }
}
