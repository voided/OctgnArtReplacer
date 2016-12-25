using OctgnArtReplacer.Octgn.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Octgn
{
    abstract class OctgnContext
    {
        const string SetFileName = "set.xml";

        public async Task<IEnumerable<Set>> GetSetsAsync()
        {
            var sets = new List<Set>();

            IEnumerable<Guid> setIds = await GetSetIds();

            foreach (var setId in setIds)
            {
                sets.Add(await GetSetAsync(setId));
            }

            return sets;
        }


        public Task<IEnumerable<Guid>> GetSetIds()
        {
            return Task.Run<IEnumerable<Guid>>(() =>
            {
                var setIds = new List<Guid>();

                var setsDir = new DirectoryInfo(GetSetsPath());

                foreach (DirectoryInfo dir in setsDir.EnumerateDirectories())
                {
                    Guid setId;

                    if (!Guid.TryParse(dir.Name, out setId))
                        continue;

                    setIds.Add(setId);
                }

                return setIds;
            });
        }
        public Task<Set> GetSetAsync(Guid setId)
        {
            string setFileName = GetSetFile(setId);
            return Set.LoadFromFileAsync(setFileName);
        }

        /// <summary>
        /// Is OCTGN installed?
        /// </summary>
        public bool IsOctgnInstalled => Directory.Exists(GameDatabasePath);
        /// <summary>
        /// Is the game definition specified by this context installed?
        /// </summary>
        public bool IsInstalled => Directory.Exists(GetSetsPath());

        protected abstract Guid GetGameId();


        private string GetSetsPath() => Path.Combine(GameDatabasePath, GetGameId().ToString(), "Sets");
        private string GetSetFile(Guid setId) => Path.Combine(GetSetsPath(), setId.ToString(), SetFileName);

        private string GameDatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"OCTGN\GameDatabase");
        private string ImageDatabasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"OCTGN\GameDatabase");
    }
}
