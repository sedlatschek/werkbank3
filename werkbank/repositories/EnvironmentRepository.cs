using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using werkbank.environments;
using werkbank.exceptions;
using werkbank.services;

namespace werkbank.repositories
{
    public static class EnvironmentRepository
    {
        /// <summary>
        /// List of all environments.
        /// </summary>
        public static readonly List<environments.Environment> Environments = new()
        {
            new AudioEnvironment(0),
            new CppEnvironment(1),
            new CSharpEnvironment(2),
            new Delphi7Environment(3),
            new Delphi10Environment(4),
            new DockerEnvironment(5),
            new JavaEnvironment(6),
            new JavascriptEnvironment(7),
            new MarkdownEnvironment(8),
            new PhotoshopEnvironment(9),
            new PhpEnvironment(10),
            new PictureEnvironment(11),
            new PremiereEnvironment(12),
            new PythonEnvironment(13),
            new TerraformEnvironment(14),
            new UmlEnvironment(15),
            new VegasEnvironment(16),
            new WmmEnvironment(17),
        };

        /// <summary>
        /// List of the directories of all environments.
        /// </summary>
        public static readonly List<string> Directories = Environments.Select(x => x.Directory).ToList();

        /// <summary>
        /// List of the handles of all environments.
        /// </summary>
        public static readonly List<string> Handles = Environments.Select(x => x.Handle).ToList();

        /// <summary>
        /// Get an environment by its handle.
        /// </summary>
        /// <param name="Handle"></param>
        /// <returns></returns>
        public static environments.Environment? ByHandle(string Handle)
        {
            return Environments.Find(e => e.Handle == Handle);
        }

        /// <summary>
        /// Get an environment by its directory. Can be either the subfolder path or the full path within any vault.
        /// </summary>
        /// <param name="Directory"></param>
        /// <returns></returns>
        public static environments.Environment? ByDirectory(string Directory)
        {
            string dir = Directory.TrimEnd('\\');
            return Environments.Find(e => e.Directory.TrimEnd('\\') == dir
                || Path.Combine(Settings.Properties.DirHotVault, e.Directory).TrimEnd('\\') == dir
                || Path.Combine(Settings.Properties.DirColdVault, e.Directory).TrimEnd('\\') == dir
                || Path.Combine(Settings.Properties.DirArchiveVault, e.Directory).TrimEnd('\\') == dir
            );
        }
    }
}
