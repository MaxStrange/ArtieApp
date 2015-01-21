using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAbstractDataTypes
{
    [Serializable]
    public class SearchNodeGraphInformation
    {
        private int _daughterID = -1;
        public int daughterID
        {
            get { return this._daughterID; }
            set { this._daughterID = value; }
        }

        private int _depth = -1;
        public int depth
        {
            get { return this._depth; }
            set { this._depth = value; }
        }

        private int _nodeID = -1;
        public int nodeID
        {
            get { return this._nodeID; }
            set { this._nodeID = value; }
        }

        private int _parentID = -1;
        public int parentID
        {
            get { return this._parentID; }
            set { this._parentID = value; }
        }

        private double _value = -1;
        public double value
        {
            get { return this._value; }
            set { this._value = value; }
        }


        public void setValueToNotComputed()
        {
            this.value = -1;
        }

        public bool valueIsNotComputed()
        {
            if (this.value < 0)
                return true;
            else
                return false;
        }
    }
}
