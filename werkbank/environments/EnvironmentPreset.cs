using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace werkbank.environments
{
    public class EnvironmentPreset
    {
        public bool? CompressOnArchive { get; }

        public EnvironmentPreset(bool? CompressOnArchive)
        {
            this.CompressOnArchive = CompressOnArchive;
        }
    }
}
