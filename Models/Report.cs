using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class Report : IComparable<Report>
    {
        public int Id { get; set; } // NEW: Unique identifier for tracking

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        public List<string> MediaAttachments { get; set; } = new List<string>();

        public DateTime SubmissionTimestamp { get; set; } = DateTime.MinValue;

        // NEW: Status tracking for Service Request Status feature
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Resolved

        // NEW: Priority for service requests
        public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High, 4=Critical

        // NEW: Assigned department/staff
        public string AssignedTo { get; set; } = "Not Assigned";

        // NEW: Resolution details
        public string ResolutionNotes { get; set; } = string.Empty;
        public DateTime? ResolutionDate { get; set; }

        // Method to calculate days since submission
        public int DaysSinceSubmission => (DateTime.Now - SubmissionTimestamp).Days;

        // Implement IComparable for tree data structures
        public int CompareTo(Report other)
        {
            if (other == null) return 1;

            // Primary comparison by Priority (higher priority comes first)
            int priorityComparison = other.Priority.CompareTo(this.Priority); // Descending order
            if (priorityComparison != 0) return priorityComparison;

            // Secondary comparison by Submission Date (older requests come first)
            int dateComparison = this.SubmissionTimestamp.CompareTo(other.SubmissionTimestamp);
            if (dateComparison != 0) return dateComparison;

            // Tertiary comparison by ID for consistency
            return this.Id.CompareTo(other.Id);
        }

        // Override Equals and GetHashCode for consistency
        public override bool Equals(object obj)
        {
            return obj is Report report && Id == report.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}