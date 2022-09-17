using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.environments;

namespace werkbank.repositories
{
    public static class EnvironmentRepository
    {
        /// <summary>
        /// List of all environments.
        /// </summary>
        public static readonly List<environments.Environment> Environments = new()
        {
            new CSharpEnvironment(0),
            new Delphi7Environment(1),
        };

        /// <summary>
        /// List of the directories of all environments.
        /// </summary>
        public static readonly List<string> Directories = Environments.Select(x => x.Directory).ToList();

        /// <summary>
        /// List of the handles of all environments.
        /// </summary>
        public static readonly List<string> Handles = Environments.Select(x => x.Handle).ToList();

        public static environments.Environment ByHandle(string Handle)
        {            
            return Environments.Where(x => x.Handle == Handle).First();
        }
    }
}
