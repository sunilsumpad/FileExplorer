using Microsoft.VisualBasic;

namespace FileExplorerApi.DirectoryPathHelpers
{
    public class DirectoryPathHelper
    {

        // Recursive function to get directory structure
        public static object GetDirectoryTree(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            var directoryItem = CreateDirectoryItem(directoryInfo);
            // Add subdirectories
            directoryItem = AddSubDirectories(directoryItem, directoryInfo);
            // Add files
            directoryItem = AddFiles(directoryItem, directoryInfo);
            return directoryItem;
        }

        private static DirectoryItem CreateDirectoryItem(DirectoryInfo directoryInfo)
        {
            return new DirectoryItem
            {
                Name = directoryInfo.Name,
                Type = "directory",
                Path = directoryInfo.FullName,
                Children = new List<DirectoryItem>()
            };
        }

        private static DirectoryItem AddSubDirectories(DirectoryItem directoryItem, DirectoryInfo directoryInfo)
        {
            foreach (var dir in directoryInfo.GetDirectories().Where(d => !IsHidden(d)))
            {
                directoryItem.Children.Add((DirectoryItem)GetDirectoryTree(dir.FullName));
            }
            return directoryItem;
        }

        private static DirectoryItem AddFiles(DirectoryItem directoryItem, DirectoryInfo directoryInfo)
        {
            foreach (var file in directoryInfo.GetFiles().Where(f => !IsHidden(f)))
            {
                directoryItem.Children.Add(CreateFileItem(file));
            }
            return directoryItem;
        }

        private static DirectoryItem CreateFileItem(FileInfo file)
        {
            return new DirectoryItem
            {
                Name = file.Name,
                Type = "file",
                Path = file.FullName,
                FileExtension = file.Extension
            };
        }

        // Helper function to check if a folder/file is hidden
        private static bool IsHidden(FileSystemInfo fileSystemInfo)
        {
            return (fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 || fileSystemInfo.Name.StartsWith(".");
        }
    }
}