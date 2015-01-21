using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrices
{
    public class RegionInSpace
    {
        private const int x = 0;
        private const int y = 1;
        private const int z = 2;


//Public fields
        public double minimum_X_OnRegion
        {
            get { return getMin(RegionInSpace.x); }
        }
        public double minimum_Y_OnRegion
        {
            get { return getMin(RegionInSpace.y); }
        }
        public double minimum_Z_OnRegion
        {
            get { return getMin(RegionInSpace.z); }
        }
        public double maximum_X_OnRegion
        {
            get { return getMax(RegionInSpace.x); }
        }
        public double maximum_Y_OnRegion
        {
            get { return getMax(RegionInSpace.y); }
        }
        public double maximum_Z_OnRegion
        {
            get { return getMax(RegionInSpace.z); }
        }
        

//Private fields
        private List<Point> _pointsThatDefineRegion = new List<Point>();
        private List<Point> pointsThatDefineRegion
        {
            get { return this._pointsThatDefineRegion; }
            set { this._pointsThatDefineRegion = value; }
        }


//Constructors
        public RegionInSpace(List<Point> pointsThatDefineRegion)
        {
            this.pointsThatDefineRegion = pointsThatDefineRegion;
        }


//Private methods        
        private double getMax(int xyz)
        {
            double max = initializeMinOrMax(xyz);
            foreach (Point p in this.pointsThatDefineRegion)
            {
                if (xyz == RegionInSpace.x)
                {
                    if (p.x > max)
                        max = p.x;
                }
                else if (xyz == RegionInSpace.y)
                {
                    if (p.y > max)
                        max = p.y;
                }
                else if (xyz == RegionInSpace.z)
                {
                    if (p.z > max)
                        max = p.z;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return max;
        }

        private double getMin(int xyz)
        {
            double min = initializeMinOrMax(xyz);
            foreach (Point p in this.pointsThatDefineRegion)
            {
                if (xyz == RegionInSpace.x)
                {
                    if (p.x < min)
                        min = p.x;
                }
                else if (xyz == RegionInSpace.y)
                {
                    if (p.y < min)
                        min = p.y;
                }
                else if (xyz == RegionInSpace.z)
                {
                    if (p.z < min)
                        min = p.z;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return min;
        }

        private double initializeMinOrMax(int xyz)
        {
            double minOrMax;
            switch (xyz)
            {
                case RegionInSpace.x:
                    minOrMax = this.pointsThatDefineRegion[0].x;
                    break;
                case RegionInSpace.y:
                    minOrMax = this.pointsThatDefineRegion[0].y;
                    break;
                case RegionInSpace.z:
                    minOrMax = this.pointsThatDefineRegion[0].z;
                    break;
                default:
                    minOrMax = 100;
                    break;
            };
            return minOrMax;
        }
    }
}
