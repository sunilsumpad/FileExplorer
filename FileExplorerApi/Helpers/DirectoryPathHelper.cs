namespace FileExplorerApi.DirectoryPathHelpers
{
    public class DirectoryPathHelper {
        
        // Recursive function to get directory structure
        public static object GetDirectoryTree(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            return new
            {
                Directory = directoryInfo.Name,
                SubDirectories = directoryInfo.GetDirectories()
                    .Where(d => !IsHidden(d))
                    .Select(d => GetDirectoryTree(d.FullName))
                    .ToList(),
                Files = directoryInfo.GetFiles()
                    .Where(f => !IsHidden(f))
                    .Select(f => f.Name).ToList()
            };
        }

        // Helper function to check if a folder/file is hidden
        private static bool IsHidden(FileSystemInfo fileSystemInfo)
        {
            return (fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 || fileSystemInfo.Name.StartsWith(".");
        }
    }
}