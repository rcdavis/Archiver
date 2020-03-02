using System.Collections.Generic;

namespace Archiver
{
    /// <summary>
    /// Entry into an archive tree structure.
    /// </summary>
    class ArchiveProjectEntry
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsFile { get; set; }
        public ArchiveProjectEntry Parent { get; set; }
        public ICollection<ArchiveProjectEntry> Children { get; private set; }

        public ArchiveProjectEntry()
        {
            Children = new List<ArchiveProjectEntry>();
            IsFile = false;
        }

        public void AddChild(ArchiveProjectEntry entry)
        {
            entry.Parent = this;
            Children.Add(entry);
        }

        public void RemoveChild(ArchiveProjectEntry entry)
        {
            if (Children.Contains(entry))
            {
                Children.Remove(entry);
                entry.Parent = null;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
