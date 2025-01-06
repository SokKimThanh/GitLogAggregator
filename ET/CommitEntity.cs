using System;

namespace GitLogAggregator.Entities
{
    /// <summary>
    /// Represents individual commits in the git history.
    /// Xem lịch sử git log
    /// Useful for examining detailed information about specific commits, such as viewing the commit history or analyzing changes made in individual commits
    /// </summary>
    public class CommitEntity
    {
        /// <summary>
        /// The person who made the commit
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// The date and time when the commit was made
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The commit message
        /// </summary>
        public string Message { get; set; }
    }
}