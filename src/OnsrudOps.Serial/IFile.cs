namespace OnsrudOps.Serial
{
    /// <summary>
    /// File interface for use in File Queue.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The contents of the file.
        /// </summary>
        public string FileContents { get; set; }
    }
}
