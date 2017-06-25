using System;
using System.Collections.Generic;

namespace VsComment.Entities
{
    public class Comment
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The character prefix of the comment
        /// </summary>
        public int CharacterPrefixId { get; set; }

        /// <summary>
        /// The keyword of this comment
        /// </summary>
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// The content of this comment
        /// </summary>
        public string CommentContent { get; set; } = string.Empty;

        /// <summary>
        /// The time when this comment was created
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// The file info of quote this comment
        /// </summary>
        public List<ProjectFileInfo> QuotesFileInfo { get; set; } = new List<ProjectFileInfo>();
    }
}