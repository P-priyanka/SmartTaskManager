using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SmartTaskManager.Models;
using SmartTaskManager.Services;

namespace SmartTaskManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private TaskStorageService _storageService = new TaskStorageService();

        // All tasks in the app
        public ObservableCollection<TaskItem> AllTasks { get; set; } = new ObservableCollection<TaskItem>();

        // Filtered list shown in the UI
        public ObservableCollection<TaskItem> FilteredTasks { get; set; } = new ObservableCollection<TaskItem>();

        // Priority options for the ComboBox
        public List<string> PriorityOptions { get; } = new List<string> { "High", "Medium", "Low" };

        private string _selectedNewPriority = "Medium";
        public string SelectedNewPriority
        {
            get { return _selectedNewPriority; }
            set { _selectedNewPriority = value; OnPropertyChanged(); }
        }

        //  Form fields for adding a new task 

        private string _newTitle = "";
        public string NewTitle
        {
            get { return _newTitle; }
            set { _newTitle = value; OnPropertyChanged(); }
        }

        private DateTime _newDueDate = DateTime.Today.AddDays(1);
        public DateTime NewDueDate
        {
            get { return _newDueDate; }
            set { _newDueDate = value; OnPropertyChanged(); }
        }

        // Search and filter 

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private string _selectedStatus = "All";
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private string _selectedPriority = "All";
        public string SelectedPriority
        {
            get { return _selectedPriority; }
            set { _selectedPriority = value; OnPropertyChanged(); ApplyFilter(); }
        }

        //  Dashboard stats 

        public int TotalCount => AllTasks.Count;
        public int CompletedCount => AllTasks.Count(t => t.IsCompleted);
        public int PendingCount => AllTasks.Count(t => !t.IsCompleted);
        public int OverdueCount => AllTasks.Count(t => !t.IsCompleted && t.DueDate.Date < DateTime.Today);

        //  Commands 

        public RelayCommand AddTaskCommand { get; set; }
        public RelayCommand ClearCompletedCommand { get; set; }

        // These two take a TaskItem parameter — handled via CommandParameter in XAML
        public RelayCommand<TaskItem> DeleteTaskCommand { get; set; }
        public RelayCommand<TaskItem> ToggleCompleteCommand { get; set; }

        //  Constructor 

        public MainViewModel()
        {
            // Load saved tasks from disk
            var saved = _storageService.LoadTasks();
            foreach (var task in saved)
                AllTasks.Add(task);

            // Add sample data if first run
            if (AllTasks.Count == 0)
                AddSampleData();

            ApplyFilter();

            // Wire up commands
            AddTaskCommand = new RelayCommand(AddTask);

            DeleteTaskCommand = new RelayCommand<TaskItem>(task =>
            {
                AllTasks.Remove(task);
                ApplyFilter();
                RefreshStats();
                _storageService.SaveTasks(AllTasks.ToList());
            });

            ToggleCompleteCommand = new RelayCommand<TaskItem>(task =>
            {
                task.IsCompleted = !task.IsCompleted;
                ApplyFilter();
                RefreshStats();
                _storageService.SaveTasks(AllTasks.ToList());
            });

            ClearCompletedCommand = new RelayCommand(() =>
            {
                var completed = AllTasks.Where(t => t.IsCompleted).ToList();
                foreach (var task in completed)
                    AllTasks.Remove(task);

                ApplyFilter();
                RefreshStats();
                _storageService.SaveTasks(AllTasks.ToList());
            });
        }

        //  Methods 

        private void AddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTitle))
            {
                MessageBox.Show("Please enter a task title.", "Missing Title");
                return;
            }

            var task = new TaskItem
            {
                Title = NewTitle.Trim(),
                Priority = Enum.Parse<Priority>(SelectedNewPriority),
                DueDate = NewDueDate
            };

            AllTasks.Insert(0, task);

            // Reset form
            NewTitle = "";
            SelectedNewPriority = "Medium";
            NewDueDate = DateTime.Today.AddDays(1);

            ApplyFilter();
            RefreshStats();
            _storageService.SaveTasks(AllTasks.ToList());
        }

        private void ApplyFilter()
        {
            FilteredTasks.Clear();

            foreach (var task in AllTasks)
            {
                // Search filter
                if (!string.IsNullOrEmpty(SearchText) &&
                    !task.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Status filter
                if (SelectedStatus == "Pending" && task.IsCompleted) continue;
                if (SelectedStatus == "Completed" && !task.IsCompleted) continue;
                if (SelectedStatus == "Overdue" && (task.IsCompleted || task.DueDate.Date >= DateTime.Today)) continue;

                // Priority filter
                if (SelectedPriority != "All" && task.Priority.ToString() != SelectedPriority) continue;

                FilteredTasks.Add(task);
            }
        }

        private void RefreshStats()
        {
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(PendingCount));
            OnPropertyChanged(nameof(OverdueCount));
        }

        private void AddSampleData()
        {
            AllTasks.Add(new TaskItem { Title = "Review project requirements document", Priority = Priority.High, DueDate = DateTime.Today });
            AllTasks.Add(new TaskItem { Title = "Fix login bug reported by QA team", Priority = Priority.High, DueDate = DateTime.Today.AddDays(-1) });
            AllTasks.Add(new TaskItem { Title = "Write unit tests for payment module", Priority = Priority.Medium, DueDate = DateTime.Today.AddDays(3) });
            AllTasks.Add(new TaskItem { Title = "Update README with setup instructions", Priority = Priority.Low, DueDate = DateTime.Today.AddDays(7), IsCompleted = true });
            AllTasks.Add(new TaskItem { Title = "Schedule team sync for next sprint", Priority = Priority.Medium, DueDate = DateTime.Today.AddDays(2) });
            AllTasks.Add(new TaskItem { Title = "Deploy hotfix to staging environment", Priority = Priority.High, DueDate = DateTime.Today.AddDays(1) });
        }
    }
}