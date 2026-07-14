using System.Windows;
using FlowGuide.Core.Models;

namespace FlowGuide.Recorder;

public partial class StepEditDialog : Window
{
    private readonly Step _step;
    private readonly string _guideDir;

    public StepEditDialog(Step step, string guideDir)
    {
        InitializeComponent();
        _step = step;
        _guideDir = guideDir;
        LoadStepData();
    }

    private void LoadStepData()
    {
        InstructionTextBox.Text = _step.InstructionText ?? "";

        if (_step.ActionType == ActionType.SystemAction)
        {
            SystemActionRadio.IsChecked = true;
            SystemActionPanel.Visibility = Visibility.Visible;

            if (_step.SystemActionType.HasValue)
            {
                SystemActionTypeCombo.SelectedIndex = (int)_step.SystemActionType.Value;
            }

            MsSettingsCommandTextBox.Text = _step.MsSettingsCommand ?? "";
            ShowAutoExecuteCheckBox.IsChecked = _step.ShowAutoExecuteButton;
        }
        else
        {
            NormalClickRadio.IsChecked = true;
            SystemActionPanel.Visibility = Visibility.Collapsed;
        }
    }

    private void ActionTypeChanged(object sender, RoutedEventArgs e)
    {
        if (SystemActionRadio.IsChecked == true)
        {
            SystemActionPanel.Visibility = Visibility.Visible;
        }
        else
        {
            SystemActionPanel.Visibility = Visibility.Collapsed;
        }
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        // Save instruction text
        _step.InstructionText = InstructionTextBox.Text;

        // Save action type
        if (SystemActionRadio.IsChecked == true)
        {
            _step.ActionType = ActionType.SystemAction;

            if (SystemActionTypeCombo.SelectedIndex >= 0)
            {
                _step.SystemActionType = (SystemActionType)SystemActionTypeCombo.SelectedIndex;
            }

            _step.MsSettingsCommand = string.IsNullOrWhiteSpace(MsSettingsCommandTextBox.Text) 
                ? null 
                : MsSettingsCommandTextBox.Text;

            _step.ShowAutoExecuteButton = ShowAutoExecuteCheckBox.IsChecked == true;
        }
        else
        {
            _step.ActionType = ActionType.LeftClick;
            _step.SystemActionType = null;
            _step.MsSettingsCommand = null;
        }

        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
