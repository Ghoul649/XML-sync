using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace XML_sync
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XMLSynchronizer synchronizer = new XMLSynchronizer();
        public MainWindow()
        {
            InitializeComponent();

        }
        private XmlDocument loadXML() 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (!(bool)openFileDialog.ShowDialog()) 
            {
                return null;
            }
            string file = openFileDialog.FileName;
            if (File.Exists(file))
            {
                XmlDocument xDoc = new XmlDocument();
                try
                {
                    xDoc.Load(file);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return null;
                }
                return xDoc;
            }
            else
            {
                MessageBox.Show($"File {file} does not exist.");
            }
            return null;
        }

        private void OpenLeftB_Click(object sender, RoutedEventArgs e)
        {
            var xDoc = loadXML();
            if (xDoc != null) 
            {
                synchronizer.Left = xDoc;
            }
            if (synchronizer.Left != null && synchronizer.Right != null) 
            {
                CompareB.IsEnabled = true;
            }
        }

        private void OpenRightB_Click(object sender, RoutedEventArgs e)
        {
            var xDoc = loadXML();
            if (xDoc != null)
            {
                synchronizer.Right = xDoc;
            }
            if (synchronizer.Left != null && synchronizer.Right != null)
            {
                CompareB.IsEnabled = true;
            }
        }

        private void CompareB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                synchronizer.Compare();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Viewer.Items.Clear();
            Viewer.Items.Add(buildTree(synchronizer.Result));
            if (synchronizer.Result != null)
                SyncB.IsEnabled = true;
        }
        private TreeViewItem buildTree(TreeElement element) 
        {
            var item = new TreeViewItem();
            item.Header = element.ToString();
            foreach (var cn in element.ChildNodes) 
            {
                item.Items.Add(buildTree(cn));
            }
            return item;
        }

        private void SyncB_Click(object sender, RoutedEventArgs e)
        {
            new SyncDialog(synchronizer.Result).ShowDialog();
        }
    }
}
