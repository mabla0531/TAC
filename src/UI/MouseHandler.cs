namespace TAC {

    //these objects are updated in EventHandlers in Game.cs

    class MouseHandler {
        public static float MouseX {get; set;}
        public static float MouseY {get; set;}

        public static bool LeftButton {get; set;} //true if left button is down
        public static bool LeftPressed {get; set;} //true if left button recently pressed
        public static bool RightButton {get; set;} //same
        public static bool RightPressed {get; set;} //same
        public static int WheelMove {get; set;}
    }
}