// Jack Swedjemark 2017-01-05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Net;
using System.Net.NetworkInformation;

namespace DeusDicio
{
    class Search
    {        
        private List<String> result = new List<string>();
        // variable to know domain status
        private bool inDomain = false;

        public Search()
            {
            CheckDomain();
            }

        /// <summary>
        /// Check if machine is in a domain
        /// </summary>
        /// <returns></returns>
        public void CheckDomain()
        {            
            try
            {
                System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
                inDomain = true;
            }
            catch
            {
                inDomain = false;
            }
        }

        /// <summary>
        /// Property for domain status
        /// </summary>
        public bool InDomain
        {
            get { return inDomain; }
        }

        /// <summary>
        /// Search Active Directory for matching computers
        /// </summary>
        /// <returns></returns>
        public bool SearchAD(string input)
        {
            bool res = false;
            if (!string.IsNullOrEmpty(input))
            {
                // create object for search
                DirectorySearcher searcher = new DirectorySearcher();
                // load only name from object
                searcher.PropertiesToLoad.Add("Name");
                // specify filter with input search string
                searcher.Filter = string.Format("(&(objectCategory=computer)(cn={0}))", input);
                // perform search
                SearchResultCollection results = searcher.FindAll();
                // check results
                if (results.Count >= 1)
                {
                    // clear old results
                    result.Clear();
                    foreach (SearchResult item in results)
                    {
                        // add each name from results to result list                        
                        if (item.Properties["name"].Count > 0)
                            result.Add(item.Properties["name"][0].ToString());
                    }                    
                    res = true;
                }
                // clean up                
                searcher.Dispose();
                results.Dispose();                                
            }
            return res;
        }

        /// <summary>
        /// Search DNS for matching records
        /// </summary>
        public bool SearchDNS(string input)
        /// <returns></returns>
        {
            bool res = false;
            if (!string.IsNullOrEmpty(input))
            {
                // clear old result
                result.Clear();
                // try to resolve hostname
                try
                {
                    // check if input is missing domain ending
                    if (!input.Contains("."))
                    {
                        // get computer domain and add to input
                        string domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                        input = input + "." + domain;
                    }
                        
                    
                    // attempt to resolve
                    Dns.GetHostEntry(input);
                    // remove domain for normalized names
                    input = input.Remove(input.IndexOf("."));
                    // if previous was successfull, add host to list                    
                    result.Add(input);                    
                    res = true;
                }
                catch { res = false; }
            }               
            return res;
        }

        /// <summary>
        /// Search property
        /// </summary>
        public List<String> Result
        {
            get { return result; }
        }

        public string GetItem(int index)
        {
            if (CheckIndex(index))
                return result[index];
            else
                return "empty";
        }

        /// <summary>
        /// Verify if index is within range of list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckIndex(int index)
        {
            if (index >= 0 && index <= result.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Clear list of content
        /// </summary>
        public void Clear()
        {
            result.Clear();
        }
    }
}
