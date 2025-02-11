public class DirectoryItem
{
    public required string Name { get; set; }
    public required string Type { get; set; } // "directory" or "file"
    public required string Path { get; set; } // Full path
    public string? FileExtension { get; set; } // Only for files
    public List<DirectoryItem> Children { get; set; } = new List<DirectoryItem>(); // Only for directories
}
