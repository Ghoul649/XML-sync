using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml;

namespace XML_sync
{
    /// <summary>
    /// Логика взаимодействия для SyncDialog.xaml
    /// </summary>
    public partial class SyncDialog : Window
    {
        TreeElement TreeElement;
        bool fromR = false;
        bool fromL = false;
        bool Unequal = false;
        bool UnequalFromLeft = false;
        public SyncDialog(TreeElement toSave)
        {
            InitializeComponent();
            TreeElement = toSave;
        }

        private void SyncB_Click(object sender, RoutedEventArgs e)
        {
            fromL = (bool)FromLeftCb.IsChecked;
            fromR = (bool)FromRightCB.IsChecked;
            Unequal = (bool)UnequalCB.IsChecked;
            UnequalFromLeft = UnequalCombo.SelectedIndex == 0 ? true : false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            if (!(bool)saveFileDialog.ShowDialog())
            {
                return;
            }
            var path = saveFileDialog.FileName;

            XmlDocument xDoc = new XmlDocument();
            var xE = xDoc.CreateElement(TreeElement.Name);
            xDoc.AppendChild(xE);
            sync(xDoc, xE, TreeElement);
            xDoc.Save(path);

        }
        private void insertChildNodes(XmlDocument xDoc, XmlNode node, TreeElement element)
        {
            foreach (var treeChild in element.ChildNodes)
            {
                if (fromL && !fromR) 
                    if (treeChild.Equality == Equality.ToLeft)
                        continue;
                if (fromR && !fromL)
                    if (treeChild.Equality == Equality.ToRight)
                        continue;
                if (!Unequal)
                    if (treeChild.Equality == Equality.NotEqual)
                        continue;
                var child = xDoc.CreateElement(treeChild.Name);
                node.AppendChild(child);
                sync(xDoc, child, treeChild);
            }
        }
        private void sync(XmlDocument xDoc, XmlNode node, TreeElement element)
        {

            if (element.Equality == Equality.ToLeft)
            {
                if (fromR)
                {
                    insertChildNodes(xDoc, node, element);
                    foreach (var treeAtr in element.Attributes)
                    {
                        var atr = xDoc.CreateAttribute(treeAtr.Name);
                        atr.Value = treeAtr.Right;
                        node.Attributes.Append(atr);
                    }
                    if (element.RightText != null)
                    {
                        var textChild = xDoc.CreateTextNode(element.RightText);
                        node.AppendChild(textChild);
                    }
                    
                }
                return;
            }


            if (element.Equality == Equality.ToRight)
            {
                if (fromL)
                {
                    insertChildNodes(xDoc, node, element);
                    foreach (var treeAtr in element.Attributes)
                    {
                        var atr = xDoc.CreateAttribute(treeAtr.Name);
                        atr.Value = treeAtr.Left;
                        node.Attributes.Append(atr);
                    }
                    if (element.LeftText != null)
                    {
                        var textChild = xDoc.CreateTextNode(element.LeftText);
                        node.AppendChild(textChild);
                    }
                }
                return;

            }
            if (element.Equality != Equality.NotEqual)
            {
                insertChildNodes(xDoc, node, element);
                foreach (var treeAtr in element.Attributes)
                {
                    var atr = xDoc.CreateAttribute(treeAtr.Name);
                    atr.Value = treeAtr.Left;
                    node.Attributes.Append(atr);
                }
                if (element.LeftText != null)
                {
                    var textChild = xDoc.CreateTextNode(element.LeftText);
                    node.AppendChild(textChild);
                }
                return;
            }
            if (!Unequal)
                return;

            insertChildNodes(xDoc, node, element);
            foreach (var treeAtr in element.Attributes)
            {
                if (treeAtr.Equality == Equality.ToLeft && UnequalFromLeft)
                    continue;
                if (treeAtr.Equality == Equality.ToRight && !UnequalFromLeft)
                    continue;
                var atr = xDoc.CreateAttribute(treeAtr.Name);
                atr.Value = UnequalFromLeft ? treeAtr.Left : treeAtr.Right;
                node.Attributes.Append(atr);
            }
            if (element.LeftText != null)
            {
                var textChild = xDoc.CreateTextNode(UnequalFromLeft ? element.LeftText : element.RightText);
                node.AppendChild(textChild);
            }
            return;
        }
    }
}
