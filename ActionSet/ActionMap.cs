using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace ActionSet
{
    /// <summary>
    /// This class contains all the allowed Artie Actions as strings and chars and the
    /// various tools used for mapping between them.
    /// </summary>
    public class ActionMap
    {   
        //A list of all the chars as an enum in case you ever need it.
        private enum serialChars
        {
            turnTightLeft = 65,//A
            turnTightRight = 66,//B
            turnWideLeft = 67,//C
            turnWideRight = 68,//D
            driveForwards = 69,//E
            driveBackwards = 70,//F
            idTestArduino = 71,//G
            removeArmSafetyForCalibration = 72,//H
            stopDriving = 73,//I
            getCalibrationData = 74,//J
            getPotValues = 75,//K
            getFreeMemory = 76,//L
            formatFlag_Number = 77,//M
            setArmSafety = 78,//N
            getCalibrationDataFromArm,//79 O
            frameEndChar = 80,//P
            driveJointAClockWise = 81,//Q
            driveJointACounterClockWise = 82,//R
            driveJointBClockWise = 83,//S
            driveJointBCounterClockWise = 84,//T
            driveJointCClockWise = 85,//U
            driveJointCCounterClockWise = 86,//V
            driveJointDClockWise = 87,//W
            connectedSignal = 88,//X
            driveJointDCounterClockWise = 89,//Y
            openGripper = 90,//Z
            closeGripper = 97,//a
            test = 116//t
        };

        public static class ActionChars
        {
            public const char stopDriving = 'I';

            public static class BodyChars
            {
                public const char turnTightLeft = 'A';
                public const char turnWideLeft = 'C';
                public const char turnTightRight = 'B';
                public const char turnWideRight = 'D';
                public const char driveForwards = 'E';
                public const char driveBackwards = 'F';

                public static readonly char[] bodyChars = { turnTightLeft, turnTightRight, turnWideLeft, turnWideRight, driveBackwards, driveForwards };
            }

            public static class ArmChars
            {
                public const char driveAClockWise = 'Q';
                public const char driveACounterClockWise = 'R';
                public const char driveBClockWise = 'S';
                public const char driveBCounterClockWise = 'T';
                public const char driveCClockWise = 'U';
                public const char driveCCounterClockWise = 'V';
                public const char driveDClockWise = 'W';
                public const char driveDCounterClockWise = 'Y';
                public const char openGripper = 'Z';
                public const char closeGripper = 'a';

                public static readonly char[] armChars = { driveAClockWise, driveACounterClockWise, driveBClockWise, driveBCounterClockWise, driveCClockWise, driveCCounterClockWise, driveDClockWise, driveDCounterClockWise, openGripper, closeGripper };
            }
        }

        public static class SerialChars_CommunicationProtocol
        {
            public const char idTestArduino = 'G';
            public const char frameEndChar = 'P';
            public const char test = 't';
            public const char connectedSignal = 'X';
            public const char formatFlag_Number = 'M';
        }

        public static class SerialChars_Orders
        {
            public const char getPotValues = 'K';
            public const char getFreeMemory = 'L';
            public const char getCalibrationDataFromBody = 'J';
            public const char getCalibrationDataFromArm = 'O';
            public const char removeArmSafetyForCalibration = 'H';
            public const char setArmSafety = 'N';
        }

        

        

//Public fields
        private static readonly ElementaryAction[] _allowedArtieActions = new ElementaryAction[16];
        /// <summary>
        /// These are the actions that can be used during a node expand.
        /// </summary>
        public static ElementaryAction[] allowedArtieActions
        {
            get { return _allowedArtieActions; }
        }
        private static Dictionary<string, char> _methodNameToActionCharMap;
        public static Dictionary<string, char> methodNameToActionCharMap
        {
            get { return _methodNameToActionCharMap; }
        }


//Public methods
        public static void initialize()
        {
            initializeMethodNameToActionCharMap();
            initializeAllowedArtieActions();
        }

        

//Private methods
        private static void initializeAllowedArtieActions()
        {
            _allowedArtieActions[0] = ElementaryAction.DRIVE_FORWARDS;
            _allowedArtieActions[1] = ElementaryAction.DRIVE_BACKWARDS;
            _allowedArtieActions[2] = ElementaryAction.TURN_TIGHT_LEFT;
            _allowedArtieActions[3] = ElementaryAction.TURN_TIGHT_RIGHT;
            _allowedArtieActions[4] = ElementaryAction.TURN_WIDE_LEFT;
            _allowedArtieActions[5] = ElementaryAction.TURN_WIDE_RIGHT;

            _allowedArtieActions[6] = ElementaryAction.DRIVE_A_CLOCKWISE;
            _allowedArtieActions[7] = ElementaryAction.DRIVE_A_COUNTERCLOCKWISE;
            _allowedArtieActions[8] = ElementaryAction.DRIVE_B_CLOCKWISE;
            _allowedArtieActions[9] = ElementaryAction.DRIVE_B_COUNTERCLOCKWISE;
            _allowedArtieActions[10] = ElementaryAction.DRIVE_C_CLOCKWISE;
            _allowedArtieActions[11] = ElementaryAction.DRIVE_C_COUNTERCLOCKWISE;
            _allowedArtieActions[12] = ElementaryAction.DRIVE_D_CLOCKWISE;
            _allowedArtieActions[13] = ElementaryAction.DRIVE_D_COUNTERCLOCKWISE;
            _allowedArtieActions[14] = ElementaryAction.OPEN_CLAW;
            _allowedArtieActions[15] = ElementaryAction.CLOSE_CLAW;
        }

        private static void initializeMethodNameToActionCharMap()
        {
            _methodNameToActionCharMap = new Dictionary<string, char>();
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_FORWARDS, ActionChars.BodyChars.driveForwards);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_BACKWARDS, ActionChars.BodyChars.driveBackwards);
            _methodNameToActionCharMap.Add(ElementaryAction.TURN_TIGHT_LEFT, ActionChars.BodyChars.turnTightLeft);
            _methodNameToActionCharMap.Add(ElementaryAction.TURN_TIGHT_RIGHT, ActionChars.BodyChars.turnTightRight);
            _methodNameToActionCharMap.Add(ElementaryAction.TURN_WIDE_LEFT, ActionChars.BodyChars.turnWideLeft);
            _methodNameToActionCharMap.Add(ElementaryAction.TURN_WIDE_RIGHT, ActionChars.BodyChars.turnWideRight);

            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_A_CLOCKWISE, ActionChars.ArmChars.driveAClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_A_COUNTERCLOCKWISE, ActionChars.ArmChars.driveACounterClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_B_CLOCKWISE, ActionChars.ArmChars.driveBClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_B_COUNTERCLOCKWISE, ActionChars.ArmChars.driveBCounterClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_C_CLOCKWISE, ActionChars.ArmChars.driveCClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_C_COUNTERCLOCKWISE, ActionChars.ArmChars.driveCCounterClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_D_CLOCKWISE, ActionChars.ArmChars.driveDClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.DRIVE_D_COUNTERCLOCKWISE, ActionChars.ArmChars.driveDCounterClockWise);
            _methodNameToActionCharMap.Add(ElementaryAction.OPEN_CLAW, ActionChars.ArmChars.openGripper);
            _methodNameToActionCharMap.Add(ElementaryAction.CLOSE_CLAW, ActionChars.ArmChars.closeGripper);
        }
    }
}
