using Infragistics.Controls.Schedules.Primitives;
using Infragistics.Controls.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KB11362_XamScheduleViewScroll_WpfApp1;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        this.xamScheduleView1.TimeslotInterval = new TimeSpan(1, 0, 0);


        var today = DateTime.Today; // 今日の日付
        var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);  // 今月の1日の日付
        int daysThisMonth = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);    // 今月の日数

        // VisibleDates の Clear メソッドを実行後、AddRange で表示期間を設定します。
        this.xamScheduleView1.VisibleDates.Clear();
        this.xamScheduleView1.VisibleDates.AddRange(firstDayOfMonth, firstDayOfMonth.AddDays(daysThisMonth - 1), true);
    }

    private void ScrollToTime(XamScheduleView scheduleView, DateTime dtScrollTo)
    {
        // スクロールバーを取得する。取得できなかったら何もしない。
        // https://jp.infragistics.com/help/wpf/infragisticswpf~infragistics.windows.utilities~getdescendantfromname
        var scrollBar = Infragistics.Windows.Utilities.GetDescendantFromName(scheduleView, "TimeslotScrollBar") as ScrollBar;
        if (scrollBar == null) return;

        // Primaryの時刻表示のヘッダーを取得する。取得できなかったら何もしない。
        var headerArea = Infragistics.Windows.Utilities.GetDescendantFromName(scheduleView, "PrimaryTimeZone") as ScheduleViewTimeslotHeaderArea;
        if (headerArea == null) return;

        // Primaryの時刻表示の最初のIntervalを取得する。取得できなかったら何もしない。
        var header = Infragistics.Windows.Utilities.GetDescendantFromType(scheduleView, typeof(ScheduleViewTimeslotHeader), false) as ScheduleViewTimeslotHeader;
        if (header == null) return;

        // 表示領域のIntervalの数を計算
        double intervalsInView = (double)headerArea.ActualWidth / (double)header.ActualWidth;

        // Primaryの時刻表示のIntervalを時間の単位で取得
        double interval = scheduleView.TimeslotInterval.TotalHours;

        // スクロール先の時間を設定（単位: 時間）
        TimeSpan scrollToTimeSpan = dtScrollTo - scheduleView.VisibleDates.Min();
        double scrollTo = scrollToTimeSpan.TotalHours;

        // スクロールバーの位置(newOffset)を計算
        // 分母(denominator): スクロール領域の全Interval数 - View領域のIntervalの数。
        // 分子(numerator): スクロール先の時間までのInterval数
        // ※Intervalの数は設定によって異なりますので、実際の実装内容に沿った修正が必要になる場合があります。
        double denominator = (scheduleView.VisibleDates.Count * 24.0 / interval) - intervalsInView;    // 1日当たり24時間表示されている（※既定）と仮定。
        double numerator = scrollTo / interval; // 0時から表示されている（※既定）と仮定
        double newOffset = numerator / denominator;
        if (newOffset > 1.0) newOffset = 1.0;

        // スクロールバーの位置を設定
        // ※この計算式自体は、ScrollBar開発元のマイクロソフトのサイトなどでご確認ください。
        scrollBar.Value = newOffset * (scrollBar.Maximum - scrollBar.Minimum);
    }

    private void buttonToYesterday_Click(object sender, RoutedEventArgs e)
    {
        // 昨日の9:00にスクロールする。
        var yesterday = DateTime.Today.AddDays(-1);
        if (!this.xamScheduleView1.VisibleDates.Contains(yesterday))
        {
            this.xamScheduleView1.VisibleDates.Add(yesterday);
        }
        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, () =>
        {
            ScrollToTime(xamScheduleView1, yesterday.AddHours(9));
        });
    }

    private void buttonToToday_Click(object sender, RoutedEventArgs e)
    {
        // 今日の9:00にスクロールする。
        var today = DateTime.Today;
        if (!this.xamScheduleView1.VisibleDates.Contains(today))
        {
            this.xamScheduleView1.VisibleDates.Add(today);
        }
        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, () =>
        {
            ScrollToTime(xamScheduleView1, today.AddHours(9));
        });
    }

    private void buttonToTomorrow_Click(object sender, RoutedEventArgs e)
    {
        // 明日の9:00にスクロールする。
        var tomorrow = DateTime.Today.AddDays(1);
        if (!this.xamScheduleView1.VisibleDates.Contains(tomorrow))
        {
            this.xamScheduleView1.VisibleDates.Add(tomorrow);
        }
        Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, () =>
        {
            ScrollToTime(xamScheduleView1, tomorrow.AddHours(9));
        });
    }
}
