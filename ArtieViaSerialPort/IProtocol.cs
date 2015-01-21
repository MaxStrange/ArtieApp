using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtieViaSerialPort
{
    public interface IProtocol
    {
        object formatDataReceived(RawData data);

        string formatDataToSend(object data);

        string handShakeSend();

        bool handShakeReceive(char data);
    }
}
