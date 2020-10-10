using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_sync
{
    public enum Equality 
    {
        Equal,
        NotEqual,
        ToRight,
        ToLeft,
        ContainsUnequality
        
    }
    public class TreeElement
    {
        public static string EqualityToString(Equality e) 
        {
            if (e == Equality.Equal)
                return "=";
            if (e == Equality.ToRight)
                return "->";
            if (e == Equality.ToLeft)
                return "<-";
            return "≠";
        }
        public List<TreeElement> ChildNodes = new List<TreeElement>();
        public string Name;
        public string RightText;
        public string LeftText;
        public Equality Equality;
        public List<TreeAttribute> Attributes = new List<TreeAttribute>();
        public override string ToString()
        {
            return $"{Name} ({EqualityToString(Equality)})";
        }
        public TreeElement(XmlNode left, XmlNode right) 
        {
            if (right == null) 
            {
                Name = left.Name;
                Equality = Equality.ToRight;
                if (left.NodeType == XmlNodeType.Text) 
                {
                    LeftText = left.InnerText;
                    return;
                }
                foreach (XmlNode la in left.Attributes) 
                {
                    Attributes.Add(new TreeAttribute(la.Name, la.Value, null));
                }
                
                foreach (XmlNode lnode in left.ChildNodes) 
                {
                    if (lnode.NodeType == XmlNodeType.Text)
                    {
                        LeftText = lnode.InnerText;
                        continue;
                    }
                    ChildNodes.Add(new TreeElement(lnode, null));
                }
                return;
            }
            if (left == null)
            {
                Name = right.Name;
                Equality = Equality.ToLeft;
                if (right.NodeType == XmlNodeType.Text)
                {
                    RightText = right.InnerText;
                    return;
                }
                foreach (XmlNode ra in right.Attributes)
                {
                    Attributes.Add(new TreeAttribute(ra.Name, null, ra.Value));
                }
                
                foreach (XmlNode rnode in right.ChildNodes)
                {
                    if (rnode.NodeType == XmlNodeType.Text) 
                    {
                        RightText = rnode.InnerText;
                        continue;
                    }
                    ChildNodes.Add(new TreeElement(null, rnode));
                }
                return;
            }
            if (left.Name != right.Name) 
            {
                throw new Exception("Root nodes are different.");
            }
            Name = left.Name;
            foreach (XmlNode la in left.Attributes) 
            {
                var ra = right.Attributes.GetNamedItem(la.Name);
                if (ra == null) 
                {
                    var atr = new TreeAttribute(la.Name, la.Value, null);
                    if (atr.Equality != Equality.Equal)
                        Equality = Equality.NotEqual;
                    Attributes.Add(atr);
                    continue;
                }

                Attributes.Add(new TreeAttribute(la.Name, la.Value, ra.Value));
            }
            
            foreach (XmlNode ra in right.Attributes) 
            {
                if (left.Attributes.GetNamedItem(ra.Name) == null) 
                {
                    var atr = new TreeAttribute(ra.Name, null, ra.Value);
                    if (atr.Equality != Equality.Equal)
                        Equality = Equality.NotEqual;
                    Attributes.Add(atr);
                }
            }

            foreach (XmlNode lnode in left.ChildNodes) 
            {
                if (lnode.NodeType == XmlNodeType.Text) 
                {
                    LeftText = lnode.InnerText;
                    continue;
                }
                var rnodes = right.SelectNodes(lnode.Name);
                if (rnodes.Count < 1) 
                {
                    ChildNodes.Add(new TreeElement(lnode, null));
                    continue;
                }
                ChildNodes.Add(new TreeElement(lnode, rnodes.Item(0)));
            }
            foreach (XmlNode rnode in right.ChildNodes) 
            {
                if (rnode.NodeType == XmlNodeType.Text)
                {
                    RightText = rnode.InnerText;
                    continue;
                }
                var lnodes = left.SelectNodes(rnode.Name);
                if (lnodes.Count < 1)
                {
                    ChildNodes.Add(new TreeElement(null, rnode));
                    continue;
                }
            }
            if (RightText != LeftText) 
            {
                Equality = Equality.NotEqual;
            }
            foreach (var cn in ChildNodes) 
            {
                if (cn.Equality != Equality.Equal) 
                {
                    if (Equality == Equality.Equal)
                        Equality = Equality.ContainsUnequality;
                }
            }
        }
    }
}
