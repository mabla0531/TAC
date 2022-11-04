namespace TAC {

    //these objects are updated in EventHandlers in Game.cs

    class MouseHandler {
        public static float MouseX {get; set;}
        public static float MouseY {get; set;}

        public static bool LeftClick {get; set;}
        public static bool RightClick {get; set;}
        public static int WheelMove {get; set;}
    }
}