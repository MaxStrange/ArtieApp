using AbstractDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace Matrices
{
    [Serializable]
    public class Point
    {
//Public fields
        public double x
        {
            get
            {
                return this.xComp.componentValue;
            }
            set
            {
                Component c = new Component(value);
                this.xComp = c;
            }
        }
        public double y
        {
            get
            {
                return this.yComp.componentValue;
            }
            set
            {
                Component c = new Component(value);
                this.yComp = c;
            }
        }
        public double z
        {
            get
            {
                return this.zComp.componentValue;
            }
            set
            {
                Component c = new Component(value);
                this.zComp = c;
            }
        }


//Internal fields
        private Component _x = new Component();
        private Component xComp
        {
            get { return this._x; }
            set { this._x = value; }
        }

        private Component _y = new Component();
        private Component yComp
        {
            get { return this._y; }
            set { this._y = value; }
        }

        private Component _z = new Component();
        private Component zComp
        {
            get { return this._z; }
            set { this._z = value; }
        }


//Private fields
        private Component[] _pointAsArray = new Component[3];
        private Component[] pointAsArray
        {
            get
            {
                this._pointAsArray[0] = this.xComp;
                this._pointAsArray[1] = this.yComp;
                this._pointAsArray[2] = this.zComp;
                return this._pointAsArray;
            }
            set
            {
                this._pointAsArray = value;
                this.xComp = this._pointAsArray[0];
                this.yComp = this._pointAsArray[1];
                this.zComp = this._pointAsArray[2];
            }
        }


//Constructors
        public Point()
        {
            this.xComp.componentValue = 0;
            this.yComp.componentValue = 0;
            this.zComp.componentValue = 0;
        }

        public Point(double x, double y, double z)
        {
            this.xComp.componentValue = x;
            this.yComp.componentValue = y;
            this.zComp.componentValue = z;
        }

        internal Point(Component x, Component y, Component z)
        {
            this.xComp = x;
            this.yComp = y;
            this.zComp = z;
        }



//Overridden operators
        public static Point operator +(Point p1, Point p2)
        {
            Point p3 = new Point();

            for (int i = 0; i < p3.pointAsArray.Length; i++)
            {
                Component p3Comp = p1.pointAsArray[i] + p2.pointAsArray[i];
                p3Comp.componentValue = (Double.IsNaN(p3Comp.componentValue) ? 0 : p3Comp.componentValue);
                p3.pointAsArray[i] = p3Comp;
            }

            return p3;
        }

        public static Point operator -(Point p1, Point p2)
        {
            //TODO : change this method to be like the + operator override
            Component p3x = p1.xComp - p2.xComp;
            Component p3y = p1.yComp - p2.yComp;
            Component p3z = p1.zComp - p2.zComp;

            if (Double.IsNaN(p3x.componentValue))
                p3x.componentValue = 0;
            if (Double.IsNaN(p3y.componentValue))
                p3y.componentValue = 0;
            if (Double.IsNaN(p3z.componentValue))
                p3z.componentValue = 0;


            Point p3 = new Point(p3x, p3y, p3z);
            

            return p3;
        }


//Public methods
        public override bool Equals(object obj)
        {
            Point otherPoint = (Point)obj;
            if (this.xComp.Equals(otherPoint.xComp) && this.yComp.Equals(otherPoint.yComp)
                && this.zComp.Equals(otherPoint.zComp))
                return true;
            else
                return false;
        }

        public double evaluationFunction(Point p2, Percent percent)
        {
            double xComp = Math.Pow(this.xComp.componentValue - p2.xComp.componentValue, 2);
            xComp = double.IsNaN(xComp) ? 0.0 : xComp;
            double yComp = Math.Pow(this.yComp.componentValue - p2.yComp.componentValue, 2);
            yComp = double.IsNaN(yComp) ? 0.0 : yComp;
            double zComp = Math.Pow(this.zComp.componentValue - p2.zComp.componentValue, 2);
            zComp = double.IsNaN(zComp) ? 0.0 : zComp;
            double evalValue = Math.Sqrt(xComp + yComp + zComp);
            return this.matches(p2, percent) ? 0.0 : evalValue;
        }

        public bool fallsWithinRegion(RegionInSpace region)
        {
            if (!((this.xComp.componentValue < region.maximum_X_OnRegion) && (this.xComp.componentValue > region.minimum_X_OnRegion)))
                return false;
            if (!((this.yComp.componentValue < region.maximum_Y_OnRegion) && (this.yComp.componentValue > region.minimum_Y_OnRegion)))
                return false;
            if (!((this.zComp.componentValue < region.maximum_Z_OnRegion) && (this.zComp.componentValue > region.minimum_Z_OnRegion)))
                return false;
            return true;
        }

        public bool matches(Point otherPoint, Percent precisionAsPercent)
        {
            if (this.xComp.matches(otherPoint.xComp, precisionAsPercent)
                && this.yComp.matches(otherPoint.yComp, precisionAsPercent)
                && this.zComp.matches(otherPoint.zComp, precisionAsPercent))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(String.Format("{0:0.00}", this.x)).Append(", ").Append(String.Format("{0:0.00}", this.y)).Append(", ").Append(String.Format("{0:0.00}", this.z)).Append(")");
            return sb.ToString();
        }


//Internal methods
        internal Vector convertToVector()
        {
            double[] components = new double[] { this.x, this.y, this.z };
            Vector v = new Vector(3, components);
            return v;
        }

        internal Tuple<Component, Component, Component> convertToTuple()
        {
            return new Tuple<Component, Component, Component>(this.xComp, this.yComp, this.zComp);
        }
    }
}
