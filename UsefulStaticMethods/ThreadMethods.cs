using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulStaticMethods
{
    public class ThreadMethods
    {
        public static Thread createNewBackgroundThread(ThreadStart method, string threadName)
        {
            Thread thread = new Thread(method);
            thread.Name = threadName;
            thread.IsBackground = true;
            return thread;
        }

        public static Thread createNewBackgroundThread(ParameterizedThreadStart method, string threadName)
        {
            Thread thread = new Thread(method);
            thread.Name = threadName;
            thread.IsBackground = true;
            return thread;
        }

        public static Thread createNewThread(ThreadStart method, string threadName)
        {
            Thread thread = new Thread(method);
            thread.Name = threadName;
            return thread;
        }

        public static Thread createNewThread(ParameterizedThreadStart method, string threadName)
        {
            Thread thread = new Thread(method);
            thread.Name = threadName;
            return thread;
        }
    }
}
