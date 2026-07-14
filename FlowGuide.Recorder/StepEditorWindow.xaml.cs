using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using FlowGuide.Core.Models;

namespace FlowGuide.Recorder;

public partial class StepEditorWindow : Window
{
    private readonly FlowGuide.Core.Models.FlowGuide _guide;
    private readonly string _guideDir;
    private ObservableCollection<StepViewModel> _stepViewModels;

    public StepEditorWindow(FlowGuide.Core.Models.FlowGuide guide, string guideDir)
    {
        InitializeComponent();
        _guide = guide;
        _guideDir = guideDir;
        _stepViewModels = new ObservableCollection<StepViewModel>();
        LoadSteps();
    }

    private void LoadSteps()
    {
        _stepViewModels.Clear();
        for (int i = 0; i < _guide.Steps.Count; i++)
        {
            _stepViewModels.Add(new StepViewModel
            {
                Step = _guide.Steps[i],
                StepNumber = i + 1,
                InstructionText = _guide.Steps[i].InstructionText ?? "No instruction",
                ActionType = _guide.Steps[i].ActionType.ToString(),
                SystemActionTypeText = _guide.Steps[i].SystemActionType?.ToString() ?? ""
            });
        }
        StepsListBox.ItemsSource = _stepViewModels;
        StepCountText.Text = $"{_guide.Steps.Count} steps recorded";
    }

    private void EditStep_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button button || button.Tag is not StepViewModel vm)
            return;

        var dialog = new StepEditDialog(vm.Step, _guideDir);
        if (dialog.ShowDialog() == true)
        {
            LoadSteps(); // Refresh display
        }
    }

    private void DeleteStep_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button button || button.Tag is not StepViewModel vm)
            return;

        var result = MessageBox.Show(
            $"Delete step {vm.StepNumber}?\n\n{vm.InstructionText}",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _guide.Steps.Remove(vm.Step);
            LoadSteps();
        }
    }

    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button button || button.Tag is not StepViewModel vm)
            return;

        int index = _guide.Steps.IndexOf(vm.Step);
        if (index > 0)
        {
            _guide.Steps.RemoveAt(index);
            _guide.Steps.Insert(index - 1, vm.Step);
            LoadSteps();
        }
    }

    private void MoveDown_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button button || button.Tag is not StepViewModel vm)
            return;

        int index = _guide.Steps.IndexOf(vm.Step);
        if (index < _guide.Steps.Count - 1)
        {
            _guide.Steps.RemoveAt(index);
            _guide.Steps.Insert(index + 1, vm.Step);
            LoadSteps();
        }
    }

    // Drag and Drop implementation
    private Point _startPoint;
    private StepViewModel? _draggedItem;

    private void StepsListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        _startPoint = e.GetPosition(null);
    }

    private void StepsListBox_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ListViewItem
                var listBox = sender as ItemsControl;
                var listBoxItem = FindAncestor<FrameworkElement>((DependencyObject)e.OriginalSource);
                
                if (listBoxItem == null) return;
                
                // Find the data context (StepViewModel)
                var contact = listBoxItem.DataContext as StepViewModel;
                if (contact != null)
                {
                    _draggedItem = contact;
                    DragDrop.DoDragDrop(listBoxItem, contact, DragDropEffects.Move);
                }
            }
        }
    }

    private void StepsListBox_Drop(object sender, DragEventArgs e)
    {
        if (_draggedItem == null) return;

        var target = ((FrameworkElement)e.OriginalSource).DataContext as StepViewModel;
        if (target == null || target == _draggedItem) return;

        int oldIndex = _stepViewModels.IndexOf(_draggedItem);
        int newIndex = _stepViewModels.IndexOf(target);

        if (oldIndex != -1 && newIndex != -1)
        {
            // Move in ObservableCollection
            _stepViewModels.Move(oldIndex, newIndex);
            
            // Move in underlying Guide Steps
            var step = _guide.Steps[oldIndex];
            _guide.Steps.RemoveAt(oldIndex);
            _guide.Steps.Insert(newIndex, step);

            // Renumber steps
            for (int i = 0; i < _stepViewModels.Count; i++)
            {
                _stepViewModels[i].StepNumber = i + 1;
            }
            
            // Refresh list to update numbers
            StepsListBox.Items.Refresh();
        }
    }

    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        do
        {
            if (current is T)
            {
                return (T)current;
            }
            current = System.Windows.Media.VisualTreeHelper.GetParent(current);
        }
        while (current != null);
        return null;
    }

    private void SaveGuide_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}

public class StepViewModel
{
    public Step Step { get; set; } = null!;
    public int StepNumber { get; set; }
    public string InstructionText { get; set; } = "";
    public string ActionType { get; set; } = "";
    public string SystemActionTypeText { get; set; } = "";
}
