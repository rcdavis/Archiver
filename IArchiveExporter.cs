using System.IO;

namespace Archiver
{
    interface IArchiveExporter
    {
        void Export(ArchiveProject project, Stream stream);
    }
}
