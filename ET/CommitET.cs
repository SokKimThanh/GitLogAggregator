using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CommitET
    {
        public int CommitId { get; set; }            // Unique identifier for the commit
        public string CommitHash { get; set; }       // Hash value of the commit
        public string CommitMessage { get; set; }    // Commit message
        public DateTime CommitDate { get; set; }     // Date and time of the commit
        public string Author { get; set; }           // FirstCommitAuthor of the commit
        public string AuthorEmail { get; set; }      // Thêm thuộc tính email
        public int ProjectWeekId { get; set; }       // Identifier for the project week
        public DateTime Date { get; set; }           // Date of the commit
        public string Period { get; set; }           // Period of the day (morning, afternoon, evening)
        public DateTime CreatedAt { get; set; }      // Timestamp when the commit was created in the database
        public DateTime UpdatedAt { get; set; }      // Timestamp when the commit was last updated in the database

        // Optional: Override ToString() for a readable representation of the commit
        public override string ToString()
        {
            return $"CommitId: {CommitId}, CommitHash: {CommitHash}, CommitMessage: {CommitMessage}, CommitDate: {CommitDate}, FirstCommitAuthor: {Author}, ProjectWeekId: {ProjectWeekId}, Date: {Date}, Period: {Period}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
        }
    }
}
