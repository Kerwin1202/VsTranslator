using System;

namespace VsComment.Entities
{
    /// <summary>
    /// Character Prefix, use this to set comment different color
    /// </summary>
    public class CharacterPrefix
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Character
        /// <example>!@</example>
        /// </summary>
        public string Character { get; set; } = string.Empty;

        /// <summary>
        /// The description for character
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The color for comment with the corresponding character
        /// </summary>
        public string Color { get; set; } = "#000000";

        /// <summary>
        /// The time when this character created
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// The count of quoted for this character
        /// </summary>
        public int QuotesCount { get; set; } = 0;
    }
}