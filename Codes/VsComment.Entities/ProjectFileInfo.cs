namespace VsComment.Entities
{
    public class ProjectFileInfo
    {
        public int Id { get; set; }

        /// <summary>
        /// The code file's name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// The code file's path
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// The lines in the code file
        /// </summary>
        public int Line { get; set; } = 0;
    }
}