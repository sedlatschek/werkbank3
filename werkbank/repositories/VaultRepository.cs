using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.models;
using werkbank.services;

namespace werkbank.repositories
{
    public static class VaultRepository
    {
        /// <summary>
        /// Get the vault directory for a given state.
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        public static string GetDirectory(WerkState State)
        {
            return State switch
            {
                WerkState.Hot => Settings.Properties.DirHotVault,
                WerkState.Cold => Settings.Properties.DirColdVault,
                WerkState.Archived => Settings.Properties.DirArchiveVault,
                _ => throw new NotImplementedException(State.ToString() + " has no directory linked"),
            };
        }
    }
}
