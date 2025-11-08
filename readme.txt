Municipal Services Application

Project Overview
Hello I'm Ahmed Kader, and this is my final POE submission for PROG7312 Advanced Application Development.  
I’ve built a comprehensive ASP.NET Core MVC web application that streamlines municipal services for South African communities.  
This platform allows citizens to report issues, track service requests, and stay informed about local events all through a modern, user-friendly interface.

What This Application Does

For Citizens
- Report Issues: Submit service requests with location details, categories, descriptions, and the option to attach photos or documents.  
- Track Requests: Monitor your service request status with real-time updates and priority tracking.  
- Discover Events: Browse local events and announcements with smart recommendations.  
- Stay Informed: Get personalized event suggestions based on your interests.

For Municipalities
- Efficient Management: Advanced data structures ensure optimal request processing.  
- Priority Handling: Automatic prioritization of critical service requests.  
- Data Insights: Built-in analytics and reporting capabilities.  
- Scalable Architecture: Designed to handle the needs of growing communities.

My Technical Implementation

Custom Data Structures (Built from Scratch)
All the data structures in this project were implemented manually without using built-in collections.

Advanced Trees (20 Marks Requirement)
- Binary Search Tree: Used for efficient searching and sorting of service requests.  
- AVL Tree: A self-balancing tree that maintains consistent performance.  
- Red-Black Tree: A color-balanced tree structure that guarantees O(log n) operations.

Heaps & Graphs (30 Marks Requirement)
- Min Heap: Manages service requests based on priority.  
- Graph: Models relationships between service areas using nodes and edges.  
- Minimum Spanning Tree: Implements Prim’s and Kruskal’s algorithms for efficient routing.  
- Graph Traversal: Includes BFS and DFS algorithms for area analysis.

Core Collections
- Custom Stack: Tracks user event history using LIFO operations.  
- Custom Queue: Handles event registrations using FIFO operations.  
- Custom Set: Manages unique categories and tags.  
- Custom Priority Queue: Handles priority-based event management.

How to Run My Project

Quick Start
```bash
1. Clone or download the project
git clone https://github.com/VCWVL/prog7312-poe-ST10266284.git
cd MunicipalServicesApp

2. Build the application
dotnet build

3. Run the application
dotnet run

4. Open your browser and go to the link vs code provides

What You’ll See
-Home Page: A clean dashboard showing service statistics.
-Report Issues: A professional form with file uploads and progress tracking.
-Local Events: Advanced filtering and smart recommendations.
-Service Status: Comprehensive tracking with visual charts and data structure demonstrations.

Data Structure Demonstrations
In the Service Request Status Page, I demonstrate:
-Tree Traversals: Real examples of Binary Search Tree, AVL, and Red-Black Tree operations.
-Heap Operations: Priority-based request extraction and management.
-Graph Algorithms: Minimum Spanning Tree calculations and traversal patterns.
-Visual Charts: Interactive doughnut and bar charts showing request distributions.

Why I Chose These Structures

Each data structure was chosen for a specific reason.
Trees handle searching and sorting, heaps manage priority requests, and graphs model service area relationships just like a real municipal system would.
This approach makes the application efficient, realistic, and scalable.

My Design Philosophy
User Experience First
I focused on creating an interface that’s both beautiful and functional. The application features:
Responsive Design: Works seamlessly on phones, tablets, and desktops.
Smooth Animations: Professional transitions and loading states.
Intuitive Navigation: Clear menus and a consistent visual hierarchy.
Accessibility: Proper contrast ratios and full keyboard navigation.

Professional Styling

I used Bootstrap 5 as a foundation and added extensive custom CSS to give the application a unique, professional appearance that stands out from generic templates.

Implementation Report
Data Structure Explanations
Binary Search Tree (BST)
Role: Efficient searching and sorting of service requests by various criteria.
Contribution: Reduces search time from O(n) to O(log n) for large datasets.
Example: Finding all requests in a specific category or area becomes much faster.
AVL Tree
Role: A self-balancing tree that maintains consistent performance.
Contribution: Guarantees O(log n) operations even with uneven data.
Example: When requests are submitted in sequence, the tree automatically balances itself.
Red-Black Tree
Role: Maintains balance using color properties and rotation rules.
Contribution: More efficient for write-heavy operations.
Example: Ideal for frequently updated request statuses.
Min Heap
Role: Handles service requests based on priority.
Contribution: Ensures urgent issues are addressed first.
Example: Critical infrastructure issues are automatically prioritized.
Graph and Minimum Spanning Tree
Role: Models relationships and optimizes routing between service areas.
Contribution: Reduces travel time and resource usage for service teams.
Example: Finds the most efficient route for multiple service calls in a region.

Architecture Decisions

I chose ASP.NET Core MVC because it provides:
-Separation of Concerns: Clear distinction between models, views, and controllers.
-Performance: A high-performance framework with minimal overhead.
-Ecosystem: A rich library base and strong community support.
-Career Relevance: Widely used in enterprise-level software development.

GitHub Repository

You can view or download the full project here:
https://github.com/VCWVL/prog7312-poe-ST10266284.git

