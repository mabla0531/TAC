using System.Collections.Generic;
using System.IO;

namespace TAC {
    class DialogTree {
        public List<Prompt> Prompts {get; set;}
        public List<Response> Responses {get; set;}
        public DialogTree(string file) {
            List<string> lines = new List<string>(File.ReadAllLines(file));
            string[] entry = new string[4];

            Prompts = new List<Prompt>();
            Responses = new List<Response>();

            foreach (string line in lines) {
                entry = line.Split("_");

                if (entry[0] == "response") {
                    Response r = new Response();
                    r.ID = int.Parse(entry[1]);
                    r.EntryMessage = entry[2];
                    r.Transition = entry[3];

                    Responses.Add(r);
                } else if (entry[0] == "prompt") {
                    Prompt p = new Prompt();
                    p.ID = int.Parse(entry[1]);
                    p.EntryMessage = entry[2];
                    p.Options = new int[3];

                    //add all options
                    string[] options = entry[3].Substring(1, entry[3].Length - 2).Split(",");
                    for (int i = 0; i < 3; i++) {
                        if (i >= options.Length) {
                            p.Options[i] = -1;
                            continue;
                        }

                        p.Options[i] = int.Parse(options[i]);
                    }

                    Prompts.Add(p);
                }
            }
        }
    
        public Prompt getPromptByID(int i) {
            foreach (Prompt p in Prompts) {
                if (p.ID == i)
                    return p;
            }

            return new Prompt();
        }
        
        public Response getResponseByID(int i) {
            foreach (Response r in Responses) {
                if (r.ID == i)
                    return r;
            }

            return new Response();
        }
        
    }
}