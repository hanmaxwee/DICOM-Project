using System;
using System.Text;
using System.Collections;

namespace DCMLIB
{
    public abstract class VR
    {
        public bool isBE = false;
        public bool isLongVR = false;       //指示显式VR时VR后面是否有预留扩展的2字节的0
        protected byte padChar = 0x20;      //填充字符,默认0x20
        public VR(bool isBE, bool isLongVR)
        {
            this.isBE = isBE;
            this.isLongVR = isLongVR;
        }

        //值域解码模板方法
        public virtual T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(string))         //支持string值类型
            {
                if (data[startIndex + length - 1] == padChar) //去除填充字节
                    length--;
                return (T)(object)Encoding.Default.GetString(data, startIndex, (int)length);
            }
            else
                throw new NotSupportedException();   //不支持其他值类型
        }
         public virtual string ToString(byte[] data, int startIndex, uint length)
        {
            return GetValue<string>(data, startIndex, length);
        }
    }
    public class SS : VR
    {
        public SS(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(Int16) && length == 2)
            {
                byte[] val = data;
                int idx;
                if (isBE)
                {
                    val = data.ReverseForBigEndian(startIndex, 2);
                    idx = 0;
                }
                else
                    idx = startIndex;
                return (T)(object)BitConverter.ToInt16(val, idx);
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            Int16 value = GetValue<Int16>(data, startIndex, length);
            return value.ToString();
        }
    }
    public class US : VR
    {
        public US(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(UInt16) && length == 2)
            {
                byte[] val = data;
                int idx;
                if (isBE)
                {
                    val = data.ReverseForBigEndian(startIndex, 2);
                    idx = 0;
                }
                else
                    idx = startIndex;
                return (T)(object)BitConverter.ToUInt16(val, idx);
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            UInt16 value = GetValue<UInt16>(data, startIndex, length);
            return value.ToString();
        }
    }
    public class SL : VR
    {
        public SL(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(Int32) && length == 4)
            {
                byte[] val = data;
                int idx;
                if (isBE)
                {
                    val = data.ReverseForBigEndian(startIndex, 4);
                    idx = 0;
                }
                else
                    idx = startIndex;
                return (T)(object)BitConverter.ToInt32(val, idx);
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            Int32 value = GetValue<Int32>(data, startIndex, length);
            return value.ToString();
        }
    }
    public class UL : VR
    {
        public UL(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(UInt32) && length == 4)
            {
                byte[] val = data;
                int idx;
                if (isBE)
                {
                    val = data.ReverseForBigEndian(startIndex, 4);
                    idx = 0;
                }
                else
                    idx = startIndex;
                return (T)(object)BitConverter.ToUInt32(val, idx);
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            UInt32 value = GetValue<UInt32>(data, startIndex, 4);
            return value.ToString();
        }
    }
    public class IS : VR
    {
        public IS(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(Int32))                         //支持Int32值类型
            {
                if (data[startIndex + length - 1] == padChar)       //去除填充字符
                    length--;
                string str = Encoding.Default.GetString(data, startIndex, (int)length);  //解码为字符串
                Int32 intVal = Int32.Parse(str);                    //转换为Int32值
                return (T)(object)intVal;
            }
            else
                throw new NotSupportedException();              //不支持其他值类型
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            Int32 value = GetValue<Int32>(data, startIndex, length);
            return value.ToString();
        }
    }
    public class DA : VR
    {
        public DA(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(DateTime) && length == 8)               //支持DateTime类型的值，长度固定为8
            {
                string sd = Encoding.Default.GetString(data, startIndex, 8);//解码为字符串yyyyMMdd
                int year = int.Parse(sd.Substring(0, 4));
                int month = int.Parse(sd.Substring(4, 2));
                int day = int.Parse(sd.Substring(6, 2));
                DateTime date = new DateTime(year, month, day);             //转换为DateTime
                return (T)(object)date;
            }
            else
                throw new NotSupportedException();                          //不支持其他类型值
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            DateTime value = GetValue<DateTime>(data, startIndex, length);
            return value.ToShortDateString();
        }
    }
    public class TM : VR
    {
        public TM(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(DateTime))              //支持DateTime类型值
            {
                if (data[startIndex + length - 1] == padChar)   //去除填充字符
                    length--;
                string sd = Encoding.Default.GetString(data, startIndex, (int)length);  //解码为字符串
                for (int i = 0; i < 13 - sd.Length; i++) sd += "0";  //“HHmmss.ffffff”不足13字符补“0”
                int hour = int.Parse(sd.Substring(0, 2));           //时HH   
                int minute = int.Parse(sd.Substring(2, 2));        //分mm
                int second = int.Parse(sd.Substring(4, 2));        //秒ss
                int milli = int.Parse(sd.Substring(7, 6));         //.毫秒ffffff
                DateTime time = new DateTime(0, 0, 0, hour, minute, second, milli, DateTimeKind.Local);   //本地时间
                return (T)(object)time;
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            DateTime dt = GetValue<DateTime>(data, startIndex, length);
            return dt.ToString("HH:mm:ss.") + dt.Millisecond.ToString("000000");
        }
    }
    public class DT : VR
    {
        public DT(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(DateTime))
            {
                if (data[startIndex + length - 1] == padChar)   //去除填充字符
                    length--;
                string sd = Encoding.Default.GetString(data, startIndex, (int)length);  //解码为字符串
                while (sd.Length<26) sd += "0";  //“yyyyMMddHHmmss.ffffff+zzzz”不足26字符补“0”
                int year = int.Parse(sd.Substring(0, 4));           //年yyyy
                int month = int.Parse(sd.Substring(4, 2));          //月MM
                int day = int.Parse(sd.Substring(6, 2));            //日dd
                int hour = int.Parse(sd.Substring(8, 2));           //时HH   
                int minute = int.Parse(sd.Substring(10, 2));        //分mm
                int second = int.Parse(sd.Substring(12, 2));        //秒ss
                int milli = int.Parse(sd.Substring(15, 6));         //.毫秒ffffff
                int zonehh = int.Parse(sd.Substring(22, 2));        //时区zzzz
                int zonemm = int.Parse(sd.Substring(24, 2));        //时区zzzz
                DateTime date; 
                if (zonehh != 0 || zonemm != 0)
                {
                    if (sd[21] == '+')                                  //反向调整时区偏移
                    {
                        zonehh = -zonehh;
                        zonemm = -zonemm;
                    }
                    date = new DateTime(year, month, day, hour, minute, second, milli, DateTimeKind.Utc);   //utc时间
                    date.AddHours(zonehh);
                    date.AddMinutes(zonemm);
                    date = date.ToLocalTime();            //转换为本地时间
                }
                else
                    date = new DateTime(year, month, day, hour, minute, second, milli, DateTimeKind.Local);   //local时间
                return (T)(object)date;              
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            DateTime dt = GetValue<DateTime>(data, startIndex, length);
            string str = dt.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            string zone = dt.ToString("zzz");                  //得到时区
            str += zone.Substring(0, 3) + zone.Substring(4, 2);
            return str;
        }
    }
    public class FL : VR
    {
        public FL(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(Single) && length==4)               //支持float值类型， 长度固定为4
            {
                byte[] val = data;
                int idx;
                if (isBE)
                {
                    val = data.ReverseForBigEndian(startIndex, 4);      //4字节BE编码转换为LE
                    idx = 0;
                }
                else
                    idx = startIndex;
                return (T)(object)BitConverter.ToSingle(val, idx);      //解码为Float
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            Single value = GetValue<Single>(data, startIndex, length);  //解码为float
            return value.ToString();
        }
    }
    public class FD : VR
    {
        public FD(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(double) && length == 8)             //支持double值类型， 长度固定为8
            {
                byte[] val = data;
                int idx;
                if (isBE)
                {
                    val = data.ReverseForBigEndian(startIndex, 8);      //8字节BE编码转换为LE
                    idx = 0;
                }
                else
                    idx = startIndex;
                return (T)(object)BitConverter.ToDouble(val, idx);      //解码为Double
            }
            else
                throw new NotSupportedException();                      //不支持其他值类型
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            double value = GetValue<double>(data, startIndex, length);
            return value.ToString();
        }
    }
    public class DS : VR
    {
        public DS(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(double))
            {
                if (data[startIndex + length - 1] == padChar)  //去除填充
                    length--;
                string str = Encoding.Default.GetString(data, startIndex, (int)length);
                double dblVal = double.Parse(str);
                return (T)(object)dblVal;
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            double value = GetValue<double>(data, startIndex, length);
            return value.ToString();
        }
    }
    public class OB : VR
    {
        public OB(bool isBE) : base(isBE,true)
        {
            padChar = 0x00;
        }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(byte[]))                    //支持byte数组值类型
            {
                if (data[startIndex + length - 1] == padChar)  //去除填充字符
                    length--;
                byte[] val = new byte[length];
                Array.Copy(data, startIndex, val, 0, length);  //复制
                return (T)(object)val;
            }
            else
                throw new NotSupportedException();              //不支持其他值类型
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            string str = "";
            byte[] value = GetValue<byte[]>(data, startIndex, length);
            int cnt = (int)length;
            if (cnt > 10) cnt = 10;   //限制长度为10
            for (int i = 0; i < cnt; i++)
                str += value[i].ToString("X2") + " ";      //每个数组元素显示为2位16进制数
            return str;
        }
    }
    public class OF : VR
    {
        public OF(bool isBE) : base(isBE, true)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(Single[]))                  //支持Single数组值类型
            {
                Single[] fltVal = new Single[length / 4];       //每个Single值占4字节
                for (int idx = 0; idx < fltVal.Length; idx++)   //循环解码每个Single值
                {
                    if (isBE)
                    {
                        byte[]val = data.ReverseForBigEndian(startIndex+idx*4, 4);  //如为BE则先转换为对应的4字节LE编码
                        fltVal[idx] = BitConverter.ToSingle(val, 0);                //解码为Single值
                    }
                    else
                        fltVal[idx] = BitConverter.ToSingle(data, startIndex+idx*4);//LE则直接解码为Single值
                }
                return (T)(object)fltVal;
            }
            else
                throw new NotSupportedException();              //不支持其他值类型
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            string str = "";
            Single[] value = GetValue<Single[]>(data, startIndex, length);  //值域解码为Single数组
            int cnt = value.Length;
            if (cnt > 10) cnt = 10;   //限制长度为10
            for (int i = 0; i < cnt; i++)
                str += value[i].ToString("X8") + " ";   //每个数组元素显示为8位16进制数
            return str;
        }
    }
    public class OW : VR
    {
        public OW(bool isBE) : base(isBE, true)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(Int16[]))               //支持Int16数组值类型
            {
                Int16[] intVal = new Int16[length / 2];       //每个Int16值占2字节
                for (int idx = 0; idx < intVal.Length; idx++)   //循环解码每个Int16值
                {
                    if (isBE)
                    {
                        byte[] val = data.ReverseForBigEndian(startIndex + idx * 2, 2);  //如为BE则先转换为对应的2字节LE编码
                        intVal[idx] = BitConverter.ToInt16(val, 0);                //解码为Int16值
                    }
                    else
                        intVal[idx] = BitConverter.ToInt16(data, startIndex + idx * 2);//LE则直接解码为Int16值
                }
                return (T)(object)intVal;
            }
            else
                throw new NotSupportedException();          //不支持其他值类型
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            string str = "";
            Int16[] value = GetValue<Int16[]>(data, startIndex, length);  //值域解码为Int16数组
            int cnt = value.Length;
            if (cnt > 10) cnt = 10;   //限制长度为10
            for (int i = 0; i < cnt; i++)
                str += value[i].ToString("X4") + " ";   //每个数组元素显示为4位16进制数
            return str;
        }
    }
    public class SQ : VR
    {
        public SQ(bool isBE) : base(isBE, true)
        { }

        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            throw new NotSupportedException();   //不在该类处理
        }

        public override string ToString(byte[] data, int startIndex, uint length)
        {
            return data.ToString();  //不具体处理，留待以后扩展
        }
    }
    public class UT : VR
    {
        public UT(bool isBE) : base(false, true)
        { }
    }
    public class UN : VR
    {
        public UN(bool isBE) : base(isBE, true)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(byte[]))
            {
                return (T)(object)data;
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            string str = "";
            byte[] value = GetValue<byte[]>(data, startIndex, length);
            int cnt = (int)length;
            if (cnt > 10) cnt = 10;   //限制长度为10
            for (int i = 0; i < cnt; i++)
                str += value[i].ToString("X2") + " ";
            return str;
        }
    }
    public class SH : VR
    {
        public SH(bool isBE) : base(isBE, false)
        { }
    }
    public class ST : VR
    {
        public ST(bool isBE) : base(isBE, false)
        { }
    }
    public class LO : VR
    {
        public LO(bool isBE) : base(isBE, false)
        { }
    }
    public class LT : VR
    {
        public LT(bool isBE) : base(isBE, false)
        { }
    }
    public class AT : VR
    {
        public AT(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(UInt16[]) && length == 4)  //支持UInt16数组值类型，长度为4
            {
                UInt16[] tag = new UInt16[2];
                if (isBE)
                {
                    tag[0] = (UInt16)(data[startIndex + 0] * 256 + data[startIndex + 1]);
                    tag[1] = (UInt16)(data[startIndex + 2] * 256 + data[startIndex + 3]);
                }
                else
                {
                    tag[0] = (UInt16)(data[startIndex + 0] + data[startIndex + 1] * 256);
                    tag[1] = (UInt16)(data[startIndex + 2] + data[startIndex + 3] * 256);
                }
                return (T)(object)tag;
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            UInt16[] value = GetValue<UInt16[]>(data, startIndex, length);
            return value[0].ToString("X4") + "," + value[1].ToString("X4");
        }
    }
    public class CS : VR
    {
        public CS(bool isBE) : base(isBE, false)
        { }
    }
    public class UI : VR
    {
        public UI(bool isBE) : base(isBE, false)
        {
            padChar = 0x00;   //UI的填充字符为0
        }
    }
    public class AE : VR
    {
        public AE(bool isBE) : base(isBE, false)
        { }
    }
    public class AS : VR
    {
        public AS(bool isBE) : base(isBE, false)
        { }
        public override T GetValue<T>(byte[] data, int startIndex, uint length)
        {
            if (typeof(T) == typeof(DateTime) && length == 4)  //支持DateTime值类型,固定4字节长
            {
                string val = Encoding.Default.GetString(data, startIndex, 4);  //解码为字符串
                int num = int.Parse(val.Substring(0, 3));      //前3字符转换为数值
                DateTime dt = new DateTime();
                if (val[3] == 'Y')                  //第4位字符为单位
                    dt.AddYears(num);
                else if (val[3] == 'M')
                    dt.AddMonths(num);
                else if (val[3] == 'W')
                    dt.AddDays(num * 7);
                else if (val[3] == 'D')
                    dt.AddDays(num);
                else
                    throw new FormatException();  //格式错误
                return (T)(object)dt;
            }
            else if (typeof(T) == typeof(double) && length == 4)  //支持double值类型,固定4字节长
            {
                string val = Encoding.Default.GetString(data, startIndex, 4);  //解码为字符串
                double dblVal= double.Parse(val.Substring(0, 3));      //前3字符转换为数值
                if (val[3] == 'Y')                  //第4位字符为单位,据此转换为年数
                    dblVal /= 1;
                else if (val[3] == 'M')
                    dblVal /= 12;
                else if (val[3] == 'W')
                    dblVal /= 52;
                else if (val[3] == 'D')
                    dblVal /= 365;
                else
                    throw new FormatException();  //格式错误
                return (T)(object)dblVal;
            }
            else
                throw new NotSupportedException();
        }
        public override string ToString(byte[] data, int startIndex, uint length)
        {
            double age = GetValue<double>(data, startIndex, length);  //验证
            return Encoding.Default.GetString(data,startIndex,(int)length);
        }
    }
    public class PN : VR
    {
        public PN(bool isBE) : base(isBE, false)
        { }
    }
    public class VRFactory
    {
        bool isBE;
        //定义一个Hashtable用于存储享元对象，实现享元池
        private Hashtable VRs = new Hashtable();
        public VRFactory(bool isBE)
        {
            this.isBE = isBE;
        }
        public VR GetVR(string key)
        {
            //如果对象存在，则直接从享元池获取
            if (VRs.ContainsKey(key))
            {
                return (VR)VRs[key];
            }
            //如果对象不存在，先创建一个新的对象添加到享元池中，然后返回
            else
            {
                VR fw = null;
                switch (key)
                {
                    case "SS": fw = new SS(isBE); break;
                    case "US": fw = new US(isBE); break;
                    case "SL": fw = new SL(isBE); break;
                    case "UL": fw = new UL(isBE); break;
                    case "IS": fw = new IS(isBE); break;
                    case "FL": fw = new FL(isBE); break;
                    case "FD": fw = new FD(isBE); break;
                    case "DS": fw = new DS(isBE); break;
                    case "DA": fw = new DA(isBE); break;
                    case "TM": fw = new TM(isBE); break;
                    case "DT": fw = new DT(isBE); break;
                    case "UI": fw = new UI(isBE); break;
                    case "PN": fw = new PN(isBE); break;
                    case "AS": fw = new AS(isBE); break;
                    case "AT": fw = new AT(isBE); break;
                    case "CS": fw = new CS(isBE); break;
                    case "AE": fw = new AE(isBE); break;
                    case "SH": fw = new SH(isBE); break;
                    case "LO": fw = new LO(isBE); break;
                    case "ST": fw = new ST(isBE); break;
                    case "LT": fw = new LT(isBE); break;
                    case "OB": fw = new OB(isBE); break;
                    case "OF": fw = new OF(isBE); break;
                    case "OW": fw = new OW(isBE); break;
                    case "SQ": fw = new SQ(isBE); break;
                    case "UT": fw = new UT(isBE); break;
                    case "UN": fw = new UN(isBE); break;
                    //default for text
                    default: throw new FormatException();  //格式错误
                }
                VRs.Add(key, fw);
                return fw;
            }
        }
    }
}
