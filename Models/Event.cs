using System;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class Event : IComparable<Event>
    {
        public int Id { get; set; } = 0;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; } = DateTime.MinValue;

        [Required]
        public string Description { get; set; } = string.Empty;

        // New property to separate Events from Announcements as per lecturer feedback
        public string Type { get; set; } = "Event"; // "Event" or "Announcement"

        // New property for event priority (for custom priority queue)
        public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High

        public Event() { }

        // Enhanced constructor with type and priority
        public Event(int id, string name, string category, DateTime date, string description, string type = "Event", int priority = 1)
        {
            Id = id;
            Name = name;
            Category = category;
            Date = date;
            Description = description;
            Type = type;
            Priority = priority;
        }

        // Implement IComparable for CustomPriorityQueue
        public int CompareTo(Event other)
        {
            if (other == null) return 1;
            return this.Priority.CompareTo(other.Priority);
        }
    }
}