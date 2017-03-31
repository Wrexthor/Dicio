// Jack Swedjemark 2017-01-05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace DeusDicio
{
    class Action
    {
        // List to store path to scripts
        private List<String> actions;        

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Action()
        {            
            PopulateList((AppDomain.CurrentDomain.BaseDirectory) + "\\Scripts");
        }

        /// <summary>
        /// Constructor override
        /// </summary>
        /// <param name="path"></param>
        public Action(string path)
        {
            PopulateList(path);
        }

        /// <summary>
        /// List property
        /// </summary>
        public List<String> List
        {
            get { return actions; }
        }

        /// <summary>
        /// Run selected script against selected hosts
        /// </summary>
        public void RunAction(string script, List<String> hosts)
        {
            // reformat list of hosts for argument passing
            string arg = string.Join(", ", hosts);
                      
            // run powershell using the script parameter and hosts as argument
            Process _Proc = Process.Start("Powershell.exe", @" -NoProfile -ExecutionPolicy unrestricted " + script + " " + arg + "");
        }

        /// <summary>
        /// Gathers all script names from given path
        /// </summary>
        public void PopulateList(string path)
        {
            // get scripts from path
            try
            {
                if ((Directory.GetFiles(path, "*.ps1").Select(Path.GetFileName).ToList()).Count > 0)
                    actions = Directory.GetFiles(path, "*.ps1").Select(Path.GetFileName).ToList();
            }                
            catch {}
            
        }
    }
}
