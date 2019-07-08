using System.Text;
using System.IO;
using System.Collections.Generic;

namespace DCMLIB
{
    public abstract class TransferSyntax
    {
        public bool isBE;
        public bool isExplicit;
        public string uid;
        public string name;
        protected VRFactory vrfactory;
        protected MemoryStream ms;
        protected BinaryReader reader;

        public TransferSyntax(bool isBE, bool isExplicit)
        {
            this.isBE = isBE;
            this.isExplicit = isExplicit;
            vrfactory = new VRFactory(isBE);
            ms = null;
            reader = null;
        }
        ~TransferSyntax()
        {
            if (ms != null)
            {
                reader.Close();
                ms.Close();
            }
        }
        protected void LookupDictionary(DCMDataElement element)
        {
            //查数据字典得到VR,Name,VM
            DicomDictionaryEntry entry = DicomDictionary.find(element.gtag, element.etag);
            if (entry != null)
            {
                if (element.vr == "" || element.vr == null) element.vr = entry.VR;
                element.name = entry.Name;
                element.vm = entry.VM;
            }
            else if (element.vr == "" && element.etag == 0)
                element.vr = "UL";
            //得到VR对象
            element.vrparser = vrfactory.GetVR(element.vr);
        }
        public DCMAbstractType Decode(byte[] data, ref uint idx)
        {
            if (ms != null)
            {
                if (data.Length != ms.Length || ms.Position != idx)   //有数据更改就关闭流
                {
                    reader.Close();
                    ms.Close();
                    ms = null;
                }
            }

            if (ms == null)                 //流为空则初始化流
            {
                ms = new MemoryStream(data);
                ms.Position = idx;
                if (isBE)
                    reader = new BinaryReaderBE(ms);
                else
                    reader = new BinaryReader(ms);
            }
            DCMAbstractType element = this.Decode(reader);
            idx = (uint)ms.Position;
            return element;
        }
        protected virtual DCMAbstractType Decode(BinaryReader reader)
        {
            DCMDataElement element = new DCMDataElement();
            //读取TAG
            element.gtag = reader.ReadUInt16();
            element.etag = reader.ReadUInt16();

            if (element.gtag == 0xfffe) //处理SQ的三个特殊标记
            {
                element.length = reader.ReadUInt32();
            }
            //读取VR
            element.vr = Encoding.Default.GetString(reader.ReadBytes(2));
            LookupDictionary(element);
            //读取值长度
            if (element.vrparser.isLongVR)
            {
                reader.ReadBytes(2);  //skip two bytes
                element.length = reader.ReadUInt32();
            }
            else
                element.length = reader.ReadUInt16();
            //读取值
            element.value = reader.ReadBytes((int)element.length);
            return element;
        }
    }
    public class implicitVRLittleEndian : TransferSyntax
    {
        public implicitVRLittleEndian() : base(false, false)
        {
            uid = "1.2.840.10008.1.2";
            name = "implicitVRLittleEndian";
        }
        protected override DCMAbstractType Decode(BinaryReader reader)
        {
            DCMDataElement element = new DCMDataElement();
            //读取TAG
            element.gtag = reader.ReadUInt16();
            element.etag = reader.ReadUInt16();
            LookupDictionary(element);
            //读取值长度
            element.length = reader.ReadUInt32();
            //读取值
            element.value = reader.ReadBytes((int)element.length);
            return element;
        }
    }
    public class explicitVRLittleEndian : TransferSyntax
    {
        public explicitVRLittleEndian() : base(false, true)
        {
            uid = "1.2.840.10008.1.2.1";
            name = "explicitVRLittleEndian";
        }
    }
    public class explicitVRBigEndian : TransferSyntax
    {
        public explicitVRBigEndian() : base(true, true)
        {
            uid = "1.2.840.10008.1.2.2";
            name = "explicitVRBigEndian";
        }
    }
    public static class TransferSyntaxs
    {
        static Dictionary<string, TransferSyntax> TSs = null;
        public static Dictionary<string, TransferSyntax> All
        {
            get
            {
                if (TSs == null)
                {
                    TSs = new Dictionary<string, TransferSyntax>();
                    TransferSyntax ts = new implicitVRLittleEndian();
                    TSs.Add(ts.uid, ts);
                    ts = new explicitVRLittleEndian();
                    TSs.Add(ts.uid, ts);
                    ts = new explicitVRBigEndian();
                    TSs.Add(ts.uid, ts);
                }
                return TSs;
            }
        }
    }
}
