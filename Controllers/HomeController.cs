using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MunicipalServicesApp.Controllers
{
    public class HomeController : Controller
    {
        // CUSTOM DATA STRUCTURES FOR PART 2 & POE REQUIREMENTS
        private static readonly CustomStack<string> _eventHistory = new CustomStack<string>();
        private static readonly CustomQueue<Event> _eventRegistrationQueue = new CustomQueue<Event>();
        private static readonly CustomPriorityQueue<Event> _priorityEvents = new CustomPriorityQueue<Event>();
        private static readonly CustomSet<string> _uniqueCategories = new CustomSet<string>();

        // POE: ADVANCED DATA STRUCTURES
        private static readonly BinarySearchTree<Report> _reportsBST = new BinarySearchTree<Report>();
        private static readonly AVLTree<Report> _reportsAVL = new AVLTree<Report>();
        private static readonly RedBlackTree<Report> _reportsRBT = new RedBlackTree<Report>();
        private static readonly MinHeap<Report> _reportsMinHeap = new MinHeap<Report>();
        private static readonly Graph<int> _serviceGraph = new Graph<int>();

        // Enhanced reports storage with status tracking for POE
        private static readonly List<Report> _reports = new List<Report>();
        private static int _reportCounter = 1;

        // Enhanced events with types and priorities - ALL 15 EVENTS
        private static readonly List<Event> _events = new List<Event>
        {
            // EVENTS (8 events)
            new Event { Id = 1, Name = "Community Clean-Up", Category = "Environment",
                       Description = "Join us to keep our neighborhood clean!",
                       Date = DateTime.Today.AddDays(3), Type = "Event", Priority = 2 },

            new Event { Id = 2, Name = "Health Awareness Day", Category = "Health",
                       Description = "Free health screenings and awareness sessions.",
                       Date = DateTime.Today.AddDays(5), Type = "Event", Priority = 3 },

            new Event { Id = 3, Name = "Youth Sports Tournament", Category = "Sports",
                       Description = "Annual youth soccer and basketball games.",
                       Date = DateTime.Today.AddDays(10), Type = "Event", Priority = 2 },

            new Event { Id = 4, Name = "Tree Planting Drive", Category = "Environment",
                       Description = "Help us plant 100 new trees this weekend.",
                       Date = DateTime.Today.AddDays(7), Type = "Event", Priority = 1 },

            new Event { Id = 5, Name = "Tech & Innovation Fair", Category = "Education",
                       Description = "Discover new local startups and innovations.",
                       Date = DateTime.Today.AddDays(15), Type = "Event", Priority = 2 },

            new Event { Id = 6, Name = "Local Farmers Market", Category = "Food",
                       Description = "Fresh produce and homemade treats every Sunday.",
                       Date = DateTime.Today.AddDays(1), Type = "Event", Priority = 1 },

            new Event { Id = 7, Name = "Book Donation Drive", Category = "Education",
                       Description = "Donate old books and promote literacy in the community.",
                       Date = DateTime.Today.AddDays(8), Type = "Event", Priority = 1 },

            new Event { Id = 8, Name = "Neighborhood Safety Talk", Category = "Public Safety",
                       Description = "Learn about safety measures with local authorities.",
                       Date = DateTime.Today.AddDays(4), Type = "Event", Priority = 3 },

            // ANNOUNCEMENTS (7 announcements)
            new Event { Id = 9, Name = "Charity Fun Run", Category = "Health",
                       Description = "Join our 5K charity run for hospital donations.",
                       Date = DateTime.Today.AddDays(12), Type = "Announcement", Priority = 2 },

            new Event { Id = 10, Name = "Cultural Food Festival", Category = "Culture",
                       Description = "Celebrate diversity with traditional food stalls.",
                       Date = DateTime.Today.AddDays(9), Type = "Announcement", Priority = 2 },

            new Event { Id = 11, Name = "Coding Bootcamp", Category = "Education",
                       Description = "Learn coding basics in one day.",
                       Date = DateTime.Today.AddDays(14), Type = "Announcement", Priority = 1 },

            new Event { Id = 12, Name = "Art in the Park", Category = "Culture",
                       Description = "Enjoy local artists displaying and selling artwork.",
                       Date = DateTime.Today.AddDays(11), Type = "Announcement", Priority = 1 },

            new Event { Id = 13, Name = "Pet Adoption Day", Category = "Community",
                       Description = "Find your furry friend from our local shelters.",
                       Date = DateTime.Today.AddDays(6), Type = "Announcement", Priority = 2 },

            new Event { Id = 14, Name = "Water Conservation Workshop", Category = "Environment",
                       Description = "Learn how to save water efficiently at home.",
                       Date = DateTime.Today.AddDays(2), Type = "Announcement", Priority = 1 },

            new Event { Id = 15, Name = "Blood Donation Drive", Category = "Health",
                       Description = "Give the gift of life. Donate blood today.",
                       Date = DateTime.Today.AddDays(13), Type = "Announcement", Priority = 3 }
        };

        // Initialize custom data structures
        static HomeController()
        {
            // Populate unique categories set
            foreach (var ev in _events)
                _uniqueCategories.Add(ev.Category);

            // Populate priority queue with high priority events
            foreach (var ev in _events.Where(e => e.Priority >= 2))
                _priorityEvents.Enqueue(ev);

            // Add some events to registration queue
            _eventRegistrationQueue.Enqueue(_events[0]);
            _eventRegistrationQueue.Enqueue(_events[1]);

            // Initialize with some sample reports for demonstration
            var sampleReports = new List<Report>
            {
                new Report { Id = 1, Location = "Main Street", Category = "Roads", Description = "Pothole near intersection", Status = "In Progress", Priority = 3, AssignedTo = "Road Maintenance", SubmissionTimestamp = DateTime.Now.AddDays(-2) },
                new Report { Id = 2, Location = "City Park", Category = "Sanitation", Description = "Overflowing trash bins", Status = "Resolved", Priority = 2, AssignedTo = "Sanitation Dept", SubmissionTimestamp = DateTime.Now.AddDays(-5), ResolutionDate = DateTime.Now.AddDays(-1) },
                new Report { Id = 3, Location = "Downtown Area", Category = "Utilities", Description = "Street light not working", Status = "Pending", Priority = 2, AssignedTo = "Not Assigned", SubmissionTimestamp = DateTime.Now.AddDays(-1) },
                new Report { Id = 4, Location = "Residential Area", Category = "Roads", Description = "Damaged sidewalk", Status = "Pending", Priority = 1, AssignedTo = "Not Assigned", SubmissionTimestamp = DateTime.Now.AddDays(-3) },
                new Report { Id = 5, Location = "Community Center", Category = "Utilities", Description = "Water leakage", Status = "In Progress", Priority = 4, AssignedTo = "Water Dept", SubmissionTimestamp = DateTime.Now.AddDays(-1) }
            };

            foreach (var report in sampleReports)
            {
                _reports.Add(report);
                _reportsBST.Insert(report);
                _reportsAVL.Insert(report);
                _reportsRBT.Insert(report);
                _reportsMinHeap.Insert(report);
            }
            _reportCounter = 6;

            // Initialize service graph for demonstration
            InitializeServiceGraph();
        }

        private static void InitializeServiceGraph()
        {
            // Create a graph representing service areas and their connections
            _serviceGraph.AddVertex(1); // Downtown
            _serviceGraph.AddVertex(2); // Residential Area
            _serviceGraph.AddVertex(3); // Industrial Area
            _serviceGraph.AddVertex(4); // Commercial Area
            _serviceGraph.AddVertex(5); // Park Areas

            // Add edges with weights representing service connectivity
            _serviceGraph.AddEdge(1, 2, 5);  // Downtown to Residential
            _serviceGraph.AddEdge(1, 3, 8);  // Downtown to Industrial
            _serviceGraph.AddEdge(1, 4, 3);  // Downtown to Commercial
            _serviceGraph.AddEdge(2, 4, 6);  // Residential to Commercial
            _serviceGraph.AddEdge(3, 5, 7);  // Industrial to Park
            _serviceGraph.AddEdge(4, 5, 4);  // Commercial to Park
        }

        // -------------------------
        // Part 1: Reports (enhanced with advanced data structures)
        // -------------------------
        public IActionResult Index()
        {
            // Track main menu visit in event history (custom stack)
            _eventHistory.Push($"Visited Main Menu at {DateTime.Now:HH:mm:ss}");

            // Statistics for dashboard
            ViewBag.TotalReports = _reports.Count;
            ViewBag.PendingReports = _reports.Count(r => r.Status == "Pending");
            ViewBag.InProgressReports = _reports.Count(r => r.Status == "In Progress");
            ViewBag.ResolvedReports = _reports.Count(r => r.Status == "Resolved");

            return View();
        }

        public IActionResult ReportIssue()
        {
            return View(new Report());
        }

        [HttpPost]
        public async Task<IActionResult> ReportIssue(Report model, List<IFormFile> mediaAttachments)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Save uploaded files to wwwroot/uploads
            var attachmentPaths = new List<string>();
            if (mediaAttachments != null)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                foreach (var file in mediaAttachments)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(uploadDir, fileName);
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                        attachmentPaths.Add($"/uploads/{fileName}");
                    }
                }
            }

            // Create and save report with advanced data structures
            model.Id = _reportCounter++;
            model.SubmissionTimestamp = DateTime.Now;
            model.MediaAttachments = attachmentPaths;

            // Add to all data structures
            _reports.Add(model);
            _reportsBST.Insert(model);
            _reportsAVL.Insert(model);
            _reportsRBT.Insert(model);
            _reportsMinHeap.Insert(model);

            TempData["SuccessMessage"] = "Thank you for your report! Your input helps improve our community.";

            return RedirectToAction("ViewReport", new { id = model.Id });
        }

        public IActionResult ViewReport(int id)
        {
            var model = _reports.FirstOrDefault(r => r.Id == id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Report not found.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // -------------------------
        // Part 2: Local Events (ENHANCED with custom data structures)
        // -------------------------
        public IActionResult LocalEvents(string? searchQuery, string? searchCategory, string? searchDate, string? sortBy, string? eventType = "All")
        {
            // Track event page visit
            _eventHistory.Push($"Viewed Events at {DateTime.Now:HH:mm:ss}");

            var result = _events.AsEnumerable();

            // Filter by type (Events vs Announcements) - SEPARATED AS PER LECTURER FEEDBACK
            if (eventType != "All")
            {
                result = result.Where(e => e.Type == eventType);
            }

            // Filter by name
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                result = result.Where(e => e.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by category using CustomSet
            if (!string.IsNullOrWhiteSpace(searchCategory) && _uniqueCategories.Contains(searchCategory))
            {
                result = result.Where(e => e.Category.Equals(searchCategory, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by date
            if (!string.IsNullOrWhiteSpace(searchDate) && DateTime.TryParse(searchDate, out DateTime parsed))
            {
                result = result.Where(e => e.Date.Date == parsed.Date);
            }

            // Sorting
            result = sortBy switch
            {
                "NameAsc" => result.OrderBy(e => e.Name),
                "NameDesc" => result.OrderByDescending(e => e.Name),
                "DateAsc" => result.OrderBy(e => e.Date),
                "DateDesc" => result.OrderByDescending(e => e.Date),
                "Priority" => result.OrderByDescending(e => e.Priority),
                _ => result.OrderBy(e => e.Date)
            };

            // ViewBag updates for enhanced UI
            ViewBag.Categories = _uniqueCategories; // Using CustomSet
            ViewBag.EventTypes = new List<string> { "All", "Event", "Announcement" };
            ViewBag.SearchCategory = searchCategory;
            ViewBag.SortBy = sortBy;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SearchDate = searchDate;
            ViewBag.SelectedEventType = eventType;

            // ENHANCED RECOMMENDATIONS - MORE PROMINENT AS PER LECTURER FEEDBACK
            var recommended = GetEnhancedRecommendations(searchCategory, searchQuery);
            ViewBag.RecommendedEvents = recommended;

            // Pass custom data structure info to view for demonstration
            ViewBag.EventHistory = _eventHistory;
            ViewBag.RegistrationQueue = _eventRegistrationQueue;
            ViewBag.PriorityEvents = _priorityEvents;

            return View(result.ToList());
        }

        private List<Event> GetEnhancedRecommendations(string category, string searchQuery)
        {
            var recommendations = new List<Event>();

            // Priority-based recommendations (using custom priority queue)
            if (!_priorityEvents.IsEmpty)
            {
                var priorityEvents = new List<Event>();
                var tempQueue = new CustomPriorityQueue<Event>();

                // Dequeue and collect top 3 priority events
                for (int i = 0; i < 3 && !_priorityEvents.IsEmpty; i++)
                {
                    var priorityEvent = _priorityEvents.Dequeue();
                    priorityEvents.Add(priorityEvent);
                    tempQueue.Enqueue(priorityEvent);
                }

                // Restore the priority queue
                while (!tempQueue.IsEmpty)
                {
                    _priorityEvents.Enqueue(tempQueue.Dequeue());
                }

                recommendations.AddRange(priorityEvents);
            }

            // Category-based recommendations
            if (!string.IsNullOrEmpty(category))
            {
                var categoryEvents = _events.Where(e => e.Category == category)
                                           .OrderByDescending(e => e.Priority)
                                           .Take(3);
                recommendations.AddRange(categoryEvents);
            }

            // Ensure uniqueness and limit to 5
            return recommendations.Distinct().Take(5).ToList();
        }

        public IActionResult ViewEvent(int id)
        {
            var ev = _events.FirstOrDefault(e => e.Id == id);
            if (ev == null)
            {
                TempData["ErrorMessage"] = "Event not found.";
                return RedirectToAction("LocalEvents");
            }

            // Track event viewing in history
            _eventHistory.Push($"Viewed event: {ev.Name} at {DateTime.Now:HH:mm:ss}");

            // ENHANCED recommendations - more prominent
            ViewBag.RecommendedEvents = _events
                .Where(e => e.Category == ev.Category && e.Id != ev.Id)
                .OrderByDescending(e => e.Priority)
                .Take(4) // Show more recommendations as per feedback
                .ToList();

            return View(ev);
        }

        // -------------------------
        // POE: Service Request Status (FULLY IMPLEMENTED WITH ADVANCED DATA STRUCTURES)
        // -------------------------
        public IActionResult ServiceRequestStatus(string status = "All")
        {
            var filteredReports = status == "All"
                ? _reports
                : _reports.Where(r => r.Status == status).ToList();

            ViewBag.SelectedStatus = status;
            ViewBag.TotalCount = _reports.Count;
            ViewBag.PendingCount = _reports.Count(r => r.Status == "Pending");
            ViewBag.InProgressCount = _reports.Count(r => r.Status == "In Progress");
            ViewBag.ResolvedCount = _reports.Count(r => r.Status == "Resolved");

            // Demonstrate ALL advanced data structures as per POE requirements
            ViewBag.BSTTraversal = _reportsBST.InOrderTraversal().Take(5).ToList();
            ViewBag.AVLTraversal = _reportsAVL.InOrderTraversal().Take(5).ToList();
            ViewBag.RBTTraversal = _reportsRBT.InOrderTraversal().Take(5).ToList();
            ViewBag.AVLHeight = _reportsAVL.GetTreeHeight();
            ViewBag.AVLBalanced = _reportsAVL.IsBalanced();

            // Demonstrate MinHeap (Heap requirement)
            var topPriorityReports = new List<Report>();
            var tempHeap = new MinHeap<Report>();

            // Extract top 3 priority reports to demonstrate heap functionality
            for (int i = 0; i < 3 && !_reportsMinHeap.IsEmpty; i++)
            {
                var report = _reportsMinHeap.ExtractMin();
                topPriorityReports.Add(report);
                tempHeap.Insert(report);
            }

            // Restore the heap
            while (!tempHeap.IsEmpty)
            {
                _reportsMinHeap.Insert(tempHeap.ExtractMin());
            }
            ViewBag.TopPriorityReports = topPriorityReports;

            // Demonstrate Graph and MST (Graph requirement)
            var primMST = _serviceGraph.PrimMinimumSpanningTree();
            var kruskalMST = _serviceGraph.KruskalMinimumSpanningTree();
            ViewBag.PrimMST = primMST;
            ViewBag.KruskalMST = kruskalMST;
            ViewBag.PrimMSTWeight = _serviceGraph.CalculateTotalWeight(primMST);
            ViewBag.KruskalMSTWeight = _serviceGraph.CalculateTotalWeight(kruskalMST);

            // Graph traversal demonstrations
            ViewBag.BFSTraversal = _serviceGraph.BreadthFirstSearch(1); // Start from vertex 1
            ViewBag.DFSTraversal = _serviceGraph.DepthFirstSearch(1);   // Start from vertex 1

            return View(filteredReports);
        }

        // Error action
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}