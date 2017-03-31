// Jack Swedjemark 2017-01-05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusDicio
{
    class Hosts
    {   
        // Store hosts in list     
        private List<String> hostList = new List<string>();

        /// <summary>
        /// Hostlist property
        /// </summary>
        public List<String> List
            {
            get { return hostList; }                
            }

        public void AddRange(List<String> list)
        {
            if (list.Count > 0)
                hostList.AddRange(list);
        }

        /// <summary>
        /// Delete host at index
        /// </summary>
        /// <param name="index"></param>
        public void DeleteHost(int index)
        {
            if (CheckIndex(index))
                hostList.RemoveAt(index);
        }

        /// <summary>
        /// Add host to list
        /// </summary>
        /// <param name="host"></param>
        public void AddHost(string host)
        {
            if(!string.IsNullOrEmpty(host))
                hostList.Add(host);
        }

        /// <summary>
        /// Verify if index is within range of list
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckIndex(int index)
        {
            if (index >= 0 && index <= hostList.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns list content as array
        /// </summary>
        /// <returns></returns>
        public string[] ToArray()
        {
            string[] newArray;
            newArray = hostList.ToArray();
            return newArray;
        }

        /// <summary>
        /// Clear hostlist of all items
        /// </summary>
        public void Clear()
        {
            hostList.Clear();
        }

        /// <summary>
        /// Remove item at index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (CheckIndex(index))
                hostList.RemoveAt(index);
        }
    }
}
