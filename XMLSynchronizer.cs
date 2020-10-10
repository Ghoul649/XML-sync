using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_sync
{
    public class XMLSynchronizer
    {
        public XmlDocument Left;
        public XmlDocument Right;
        public TreeElement Result;
        public void Compare()
        {
            if (Left == null) 
            {
                throw new Exception("Left can not be null.");
            }
            if (Right == null)
            {
                throw new Exception("Right can not be null.");
            }

            var left = Left.DocumentElement;
            if (left == null) 
            {
                throw new Exception("Left does not have root.");
            }
            var right = Right.DocumentElement;
            if (right == null)
            {
                throw new Exception("Right does not have root.");
            }

           Result = new TreeElement(left,right);

        }
    }
}
