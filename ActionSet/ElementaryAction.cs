using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionSet
{
    [Serializable]
    public sealed class ElementaryAction
    {
        private readonly string name;

        private static readonly Dictionary<string, ElementaryAction> instance =
            new Dictionary<string, ElementaryAction>();

        public static readonly ElementaryAction START = new ElementaryAction("start SearchNode");
        public static readonly ElementaryAction TURN_TIGHT_LEFT = new ElementaryAction("turnTightLeft");
        public static readonly ElementaryAction TURN_WIDE_LEFT = new ElementaryAction("turnWideLeft");
        public static readonly ElementaryAction TURN_TIGHT_RIGHT = new ElementaryAction("turnTightRight");
        public static readonly ElementaryAction TURN_WIDE_RIGHT = new ElementaryAction("turnWideRight");
        public static readonly ElementaryAction DRIVE_FORWARDS = new ElementaryAction("driveForwards");
        public static readonly ElementaryAction DRIVE_BACKWARDS = new ElementaryAction("driveBackwards");
        public static readonly ElementaryAction DRIVE_A_CLOCKWISE = new ElementaryAction("driveAClockWise");
        public static readonly ElementaryAction DRIVE_A_COUNTERCLOCKWISE = new ElementaryAction("driveACounterClockWise");
        public static readonly ElementaryAction DRIVE_B_CLOCKWISE = new ElementaryAction("driveBClockWise");
        public static readonly ElementaryAction DRIVE_B_COUNTERCLOCKWISE = new ElementaryAction("driveBCounterClockWise");
        public static readonly ElementaryAction DRIVE_C_CLOCKWISE = new ElementaryAction("driveCClockWise");
        public static readonly ElementaryAction DRIVE_C_COUNTERCLOCKWISE = new ElementaryAction("driveCCounterClockWise");
        public static readonly ElementaryAction DRIVE_D_CLOCKWISE = new ElementaryAction("driveDClockWise");
        public static readonly ElementaryAction DRIVE_D_COUNTERCLOCKWISE = new ElementaryAction("driveDCounterClockWise");
        public static readonly ElementaryAction OPEN_CLAW = new ElementaryAction("open claw");
        public static readonly ElementaryAction CLOSE_CLAW = new ElementaryAction("close claw");

        public static implicit operator ElementaryAction(string str)
        {
            ElementaryAction result;
            if (instance.TryGetValue(str, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public static implicit operator string(ElementaryAction action)
        {
            return action.ToString();
        }

        private ElementaryAction(string name)
        {
            this.name = name;
            instance[name] = this;
        }

        public override string ToString()
        {
            return this.name;
        }

        public sealed class Body
        {
            private readonly string name;

            private static readonly Dictionary<string, Body> instance = new Dictionary<string, Body>();

            public static readonly Body START = new Body("start SearchNode");
            public static readonly Body TURN_TIGHT_LEFT = new Body("turnTightLeft");
            public static readonly Body TURN_WIDE_LEFT = new Body("turnWideLeft");
            public static readonly Body TURN_TIGHT_RIGHT = new Body("turnTightRight");
            public static readonly Body TURN_WIDE_RIGHT = new Body("turnWideRight");
            public static readonly Body DRIVE_FORWARDS = new Body("driveForwards");
            public static readonly Body DRIVE_BACKWARDS = new Body("driveBackwards");


            public static implicit operator Body(string str)
            {
                Body result;
                if (instance.TryGetValue(str, out result))
                    return result;
                else
                    throw new InvalidCastException();
            }

            public static implicit operator string(Body bs)
            {
                return bs.ToString();
            }

            private Body(string name)
            {
                this.name = name;
                instance[name] = this;
            }

            public override string ToString()
            {
                return name;
            }
        }

        public sealed class Arm
        {
            private readonly string name;

            private static readonly Dictionary<string, Arm> instance = new Dictionary<string, Arm>();

            public static readonly Arm DRIVE_A_CLOCKWISE = new Arm("driveAClockWise");
            public static readonly Arm DRIVE_A_COUNTERCLOCKWISE = new Arm("driveACounterClockWise");
            public static readonly Arm DRIVE_B_CLOCKWISE = new Arm("driveBClockWise");
            public static readonly Arm DRIVE_B_COUNTERCLOCKWISE = new Arm("driveBCounterClockWise");
            public static readonly Arm DRIVE_C_CLOCKWISE = new Arm("driveCClockWise");
            public static readonly Arm DRIVE_C_COUNTERCLOCKWISE = new Arm("driveCCounterClockWise");
            public static readonly Arm DRIVE_D_CLOCKWISE = new Arm("driveDClockWise");
            public static readonly Arm DRIVE_D_COUNTERCLOCKWISE = new Arm("driveDCounterClockWise");
            public static readonly Arm OPEN_CLAW = new Arm("openClaw");
            public static readonly Arm CLOSE_CLAW = new Arm("closeClaw");

            public static implicit operator Arm(string str)
            {
                Arm result;
                if (instance.TryGetValue(str, out result))
                    return result;
                else
                    throw new InvalidCastException();
            }

            public static implicit operator string(Arm bs)
            {
                return bs.ToString();
            }

            private Arm(string name)
            {
                this.name = name;
                instance[name] = this;
            }

            public override string ToString()
            {
                return name;
            }
        }
    }
}
