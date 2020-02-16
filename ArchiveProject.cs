namespace Archiver
{
    class ArchiveProject
    {
        public ArchiveProjectEntry Root { get; private set; }

        public ArchiveProject()
        {
            Root = new ArchiveProjectEntry
            {
                Name = "Root",
                Url = "/"
            };
        }
    }
}
