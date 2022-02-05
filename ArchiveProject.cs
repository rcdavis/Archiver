using System.Collections.Generic;

namespace Archiver
{
    /// <summary>
    /// Represents a tree structure for archives.
    /// </summary>
    public class ArchiveProject
    {
        public ArchiveProjectEntry Root { get; set; }

        public ArchiveProject()
        {
            Root = new ArchiveProjectEntry
            {
                Name = "Root"
            };
        }

        public ICollection<ArchiveProjectEntry> GetFiles()
        {
            ICollection<ArchiveProjectEntry> entries = new List<ArchiveProjectEntry>();

            RecursiveFileCapture(entries, Root);

            return entries;
        }

        private void RecursiveFileCapture(ICollection<ArchiveProjectEntry> entries, ArchiveProjectEntry curEntry)
        {
            if (curEntry.IsFile)
            {
                entries.Add(curEntry);
            }

            foreach(ArchiveProjectEntry entry in curEntry.Children)
            {
                RecursiveFileCapture(entries, entry);
            }
        }
    }
}
