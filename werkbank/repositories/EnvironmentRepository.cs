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
        public static readonly List<environments.Environment> Environments = Init();

        /// <summary>
        /// Initiate the environment repository.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static List<environments.Environment> Init()
        {
            List<environments.Environment> list = new();

            var add = delegate (Type type)
            {
                object? obj = Activator.CreateInstance(type, list.Count);
                if (obj == null)
                {
                    throw new Exception("Could not initiate environments dynamically");
                }
                list.Add((environments.Environment)obj);
            };

            add(typeof(AudioEnvironment));
            add(typeof(CadEnvironment));
            add(typeof(CppEnvironment));
            add(typeof(CSharpEnvironment));
            add(typeof(Delphi7Environment));
            add(typeof(Delphi10Environment));
            add(typeof(DockerEnvironment));
            add(typeof(GoEnvironment));
            add(typeof(JavaEnvironment));
            add(typeof(JavascriptEnvironment));
            add(typeof(MarkdownEnvironment));
            add(typeof(PhotoshopEnvironment));
            add(typeof(PhpEnvironment));
            add(typeof(PictureEnvironment));
            add(typeof(PremiereEnvironment));
            add(typeof(PythonEnvironment));
            add(typeof(TerraformEnvironment));
            add(typeof(UmlEnvironment));
            add(typeof(VegasEnvironment));
            add(typeof(WmmEnvironment));

            return list;
        }

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

        public static environments.Environment ByHandleOrDie(string Handle)
        {
            return Environments.Find(e => e.Handle == Handle) ?? throw new NotFoundException("Environment with handle \"" + Handle + "\" not found");
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
