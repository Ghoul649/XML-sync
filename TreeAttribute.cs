using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_sync
{
    public class TreeAttribute
    {
        public string Name;
        public string Left;
        public string Right;
        public Equality Equality;

        public TreeAttribute(string name,string value) 
        {
            Equality = Equality.Equal;
            Name = name;
            Left = value;
        }
        public TreeAttribute(string name, string left, string right) 
        {
            if (right == null)
                Equality = Equality.ToRight;
            else if (left == null)
                Equality = Equality.ToLeft;
            else if (left != right)
                Equality = Equality.NotEqual;
            else Equality = Equality.Equal;
            Name = name;
            Left = left;
            Right = right;
        }
    }
}
