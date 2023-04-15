namespace TAC {
    abstract class DialogEntry {
        public int ID {get; set;}
        public string EntryMessage {get; set;}
    }

    class Prompt : DialogEntry {
        public int[] Options {get; set;}
    }

    class Response : DialogEntry {
        public string Transition {get; set;}
    }
}