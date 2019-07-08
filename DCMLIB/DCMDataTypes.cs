using System.Collections.Generic;

namespace DCMLIB
{
    //组合模式抽象节点
    public abstract class DCMAbstractType
    {
        public ushort gtag;
        public ushort etag;
        public string name;
        public string vr;
        public string vm;
        public uint length;
        public object value;
        public VR vrparser;

        public string Tag
        {
            get
            {
                return "(" + this.gtag.ToString("X4") + "," + this.etag.ToString("X4") + ")";
            }
        }
        public abstract string ToString(string head);
    }

    //组合模式叶子节点
    public class DCMDataElement : DCMAbstractType
    {
        public override string ToString(string head)
        {
            string str = head;
            str += gtag.ToString("X4") + "," + etag.ToString("X4") + "\t";
            str += vr + "\t";
            str += name + "\t";
            if (length == 0xffffffff)
                str += "Undefined\n";
            else
                str += length.ToString() + "\t";
            if (vr == "SQ")
                str += ((DCMDataSequence)value).ToString(head + ">");
            else
                str += vrparser.ToString((byte[])value, 0, length);
            return str;
        }

    }

    //组合模式容器节点
    public class DCMDataSet : DCMAbstractType
    {
        public List<DCMAbstractType> items = new List<DCMAbstractType>();
        protected TransferSyntax syn;

        public DCMDataSet(TransferSyntax syn)
        {
            this.syn = syn;
        }

        public override string ToString(string head)
        {
            string str = "";
            foreach(DCMAbstractType elem in items)
            {
                if (elem != null)
                {
                    if (str != "")
                        str += "\n";  //两个数据元素之间用换行符分割
                    str += elem.ToString(head);
                }
            }
            return str;
        }

        public virtual List<DCMAbstractType> Decode(byte[] data, ref uint idx)
        {
            while (idx < data.Length)
            {
                DCMAbstractType item = syn.Decode(data, ref idx);
                //判断特殊标记
                if (item.gtag == 0xfffe && item.etag == 0xe0dd)
                    break;
                if (item.gtag == 0xfffe && item.etag == 0xe00d)
                    break;

                if (item.vr == "SQ")
                {
                    DCMDataSequence sq = new DCMDataSequence(syn);
                    uint ulidx = 0;
                    byte[] val = (byte[])item.value;
                    sq.Decode(val, ref ulidx);
                    item.value = sq;
                    //todo：修正idx位置
                }
            }
            return items;

        }
    }

    public class DCMDataItem : DCMDataSet
    {
        public DCMDataItem(TransferSyntax syn) : base(syn)
        {
        }
        public override List<DCMAbstractType> Decode(byte[] data, ref uint idx)
        {
            DCMAbstractType item = syn.Decode(data, ref idx);
            if (item.gtag == 0xfffe && item.etag == 0xe000)  //item start
            {
                uint ulidx = 0;
                byte[] val = (byte[])item.value;
                base.Decode(val, ref ulidx);
                //tudo：修正idx位置
            }
            return items;
        }
        public override string ToString(string head)
        {
            string str = "";
            foreach (DCMAbstractType elem in items)
            {
                if (elem != null)
                {
                    if (str != "")
                        str += "\n";  //两个数据元素之间用换行符分割
                    str += elem.ToString(head);
                }
            }
            return str;
        }
    }

    public class DCMDataSequence : DCMDataSet
    {
        public DCMDataSequence(TransferSyntax syn) : base(syn)
        {
        }
        public override List<DCMAbstractType> Decode(byte[] data, ref uint idx)
        {
            while (idx < data.Length)
            {
                DCMDataItem item = new DCMDataItem(syn);
                item.Decode(data, ref idx);  //解码一个item，加入items列表
                if (item.items.Count > 0)
                    items.Add(item);
                else
                    break;
            }
            return items;
        }
        public override string ToString(string head)
        {
            string str = "";
            int i = 1;
            foreach (DCMAbstractType item in items)
            {
                str += "\n" + head + "ITEM" + i.ToString() + "\n";
                str += item.ToString(head);
                i++;
            }
            return str;
        }

    
    }
}
