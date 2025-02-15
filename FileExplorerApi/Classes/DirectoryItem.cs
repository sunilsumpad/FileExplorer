public class DirectoryItem
{
    public required string Name { get; set; }
    public required string Type { get; set; } // "directory" or "file"
    public required string Path { get; set; } // Full path
    public long? Size { get; set; } // Size of files only
    public DateTime CreationDateTime { get; set; } 
    public DateTime ModifiedDateTime { get; set; } 
    public string? FileExtension { get; set; } // File extension for files only
    public List<DirectoryItem> Children { get; set; } = new List<DirectoryItem>(); // Only for directories
}
