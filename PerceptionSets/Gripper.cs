using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrices;

namespace PerceptionSets
{
    [Serializable]
    public class Gripper : ArmComponent
    {
//Public fields
        private int _currentFingerWidth;
        public int currentFingerWidth
        {
            get { return _currentFingerWidth; }
            private set { this._currentFingerWidth = value; }
        }


//Constructors
        public Gripper()
        {
            base.location = new Point(0, 0, 0);
            base.jointFrame = new Orientation();
            base.jointFrame.alignVectorAlongAxis(Orientation.Vectors.nVector, Vector.Components.x);
            base.jointFrame.alignVectorAlongAxis(Orientation.Vectors.oVector, Vector.Components.y);
            base.jointFrame.alignVectorAlongAxis(Orientation.Vectors.pVector, Vector.Components.z);
        }

        public Gripper(Point location, Orientation frame)
        {
            base.location = location;
            base.jointFrame = frame;
        }



//Public methods
        public void openGripper()
        {
            this.currentFingerWidth += 5;//TODO : fix this value to be something that makes sense
        }

        public void closeGripper()
        {
            this.currentFingerWidth -= 5;
        }
    }
}
