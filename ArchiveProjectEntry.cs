using System.Collections.Generic;

namespace Archiver
{
    class ArchiveProjectEntry
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public ICollection<ArchiveProjectEntry> Children { get; private set; }

        public ArchiveProjectEntry()
        {
            Children = new List<ArchiveProjectEntry>();
        }
    }
}
