using System.IO;
using System.Windows;
using Microsoft.Win32;
using FlowGuide.Core.Services;
using FlowGuide.Core.Models;

namespace FlowGuide.Player;

public partial class MainWindow : Window
{
    private readonly StorageService _storageService;

    public MainWindow()
    {
        InitializeComponent();
        _storageService = new StorageService();
    }

    private async void LoadGuideBtn_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "FlowGuide Files (*.json)|*.json|All Files (*.*)|*.*",
            InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FlowGuide")
        };

        if (dialog.ShowDialog() == true)
        {
            try
            {
                string filePath = dialog.FileName;
                string guideDir = Path.GetDirectoryName(filePath)!;
                
                // Load the guide from the directory, not just the file path
                // StorageService expects the directory path
                var guide = await _storageService.LoadGuideAsync(guideDir);
                
                if (guide != null)
                {
                    StatusText.Text = $"Loaded: {guide.Title} ({guide.Steps.Count} steps)";
                    
                    // Launch Overlay with user preference
                    bool overlayEnabled = EnableOverlayCheckbox.IsChecked ?? true;
                    var overlay = new PlaybackOverlay(guide, guideDir, overlayEnabled);
                    overlay.Show();
                    
                    this.Hide();
                    overlay.Closed += (s, args) => this.Show();
                }
                else
                {
                    MessageBox.Show("Could not load guide. File 'guide.json' not found in directory.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading guide: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}