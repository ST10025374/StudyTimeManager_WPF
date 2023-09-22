**Weekly Self-Study Time Management App**

---

**Overview**  
The ProgPoe application is designed to help students efficiently manage and track their self-study hours throughout a semester. This WPF application seamlessly integrates with a distinct class library to deliver an organized and intuitive user experience.

---

**Features**
- **Semester Management:**  
  - Define academic semesters with specified start and end dates.

- **Module Management:**  
  - Add and manage individual modules, complete with specific codes and associated data.
  
- **Self-Study Tracker:**  
  - Log daily study sessions, ensuring you stay on track with your academic goals.
  
- **Calculation Utilities:**  
  - Intuitive utilities for operations such as calculating remaining study hours.

---

**Getting Started**

**Prerequisites:**  
- .NET Core or .NET 5+.
- Visual Studio (recommended version or higher).

**How to Run:**  
1. Open the solution file `ProgPoe_WPF.sln` in Visual Studio.
2. Build the solution.
3. Run the application.

---

**Usage Guide**

- **Starting Up:**  
  - Upon launching the application, you will be greeted with the `MainWindow`. This serves as your primary navigation hub.

- **Managing Semesters:**  
  - Head to the `SemesterWindow` from the main menu.
  - Create a new semester by choosing a start date and defining the semester's duration (in weeks).
  - After creation, the semester is listed among active semesters.

- **Handling Modules:**  
  - From the `SemesterWindow`, access the `ModulesWindow`.
  - Here, manage individual modules associated with the chosen semester.
  - Define module attributes such as `ModuleCode`, `ModuleHoursPerWeek`, `SelfStudyHoursPerWeek`, and `CurrentWeek`.
  - Each module can house records of study sessions, detailing study dates and durations.

- **Tracking Self-Study Sessions:**  
  - The `SelfStudyWindow` is your go-to for logging study hours for specific modules.
  - Simply input the study date and specify study duration in hours.
  - This window offers an at-a-glance view of the module's ongoing week, total weekly study hour requirements, hours remaining for self-study, and a cumulative record of hours studied throughout the semester.
  - Already logged hours for a particular date? No worries - the system will send a reminder alert.
  - For ease of navigation, the `SelfStudyWindow` features a "Go Back" button, letting you return to the previous window.

---

Stay on top of your academic goals with ProgPoe - your self-study companion!
