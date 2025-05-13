namespace WebApiCore.Class.ftp
{

    public enum ListItemType : byte
    {
        Directory = 1,
        File = 2
    }

    public class ListItem
    {
        public string Name { get; set; }
        public DateTime? LastModified { get; set; }
        public string Size { get; set; }
        public string Owner { get; set; }
        public string Group { get; set; }
        public string Permissions { get; set; }
        public ListItemType Type { get; set; }
    }
}
