using System.Windows;
using System.Windows.Threading;

namespace NewBeehiveManagementSystem;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Queen queen = new Queen();
    private DispatcherTimer timer = new DispatcherTimer();

    public MainWindow()
    {
        InitializeComponent();
        statusReport.Text = queen.StatusReport;
        timer.Tick += Timer_Tick;
        timer.Interval = TimeSpan.FromSeconds(1.5);
        timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        WorkShift_Click(this, new RoutedEventArgs());
    }

    private void WorkShift_Click(object sender, RoutedEventArgs e)
    {
        queen.WorkTheNextShift();
        statusReport.Text = queen.StatusReport;
    }

    private void AssignJob_Click(object sender, RoutedEventArgs e)
    {
        queen.AssignBee(jobSelector.Text);
        statusReport.Text = queen.StatusReport;
    }
}