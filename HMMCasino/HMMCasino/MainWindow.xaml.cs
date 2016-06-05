using System.Windows;

namespace HMMCasino
{
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public BindingList<DiceSide> DiceSides; 

        public MainWindow()
        {
            InitializeComponent();
            DiceSides = new BindingList<DiceSide>
                            {
                                new DiceSide(),
                                new DiceSide(),
                                new DiceSide()
            };
            this.dataGrid.ItemsSource = DiceSides;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
        }
    }
}
