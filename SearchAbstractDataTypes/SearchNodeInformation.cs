using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAbstractDataTypes
{
    public class SearchNodeInformation
    {
        private SearchNode _node = null;
        private SearchNode node
        {
            get { return this._node; }
            set { this._node = value; }
        }

        public SearchNodeInformation(SearchNode n)
        {
            this.node = n;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.node.methodUsedToDeriveNodeFromParent);
            //sb.Append("_x:_").Append(String.Format("{0:0.000}", this.node.perceptionState_Body.location.x));
            //sb.Append("_y:_").Append(String.Format("{0:0.000}", this.node.perceptionState_Body.location.y));
            //sb.Append("_nx:_").Append(String.Format("{0:0.000}", this.node.perceptionState_Body.artieFrame.nVector.listOfvalues[(int)Vector.Components.x]));
            //sb.Append("_ny:_").Append(String.Format("{0:0.000}", this.node.perceptionState_Body.artieFrame.nVector.listOfvalues[(int)Vector.Components.y]));

            sb.Append("ClawX: ").Append(String.Format("{0:0.00}", this.node.perceptionState_Arm.gripper.location.x));
            sb.Append("_Y: ").Append(String.Format("{0:0.00}", this.node.perceptionState_Arm.gripper.location.y));
            sb.Append("_Z: ").Append(String.Format("{0:0.00}", this.node.perceptionState_Arm.gripper.location.z));
            
            return sb.ToString();
        }
    }
}
