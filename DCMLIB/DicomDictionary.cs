using System;
using System.Collections.Generic;
using System.IO;

namespace DCMLIB
{
    public class DicomDictionaryEntry
    {
        string groupId;
        string elementId;
        string name;
        string keyword;
        string vr;
        string vm;

        public DicomDictionaryEntry(string line)
        {
            string[] entry = line.Split('\t');

            this.groupId = entry[0].Substring(2, 4);
            this.elementId = entry[0].Substring(7, 4);
            this.name = entry[1];
            this.keyword = entry[2];
            this.vr = entry[3];
            this.vm = entry[4];
        }

        public string Tag
        {
            get
            {
                return "(" + this.groupId + "," + this.elementId + ")";
            }
        }
        public string GroupId
        {
            get
            {
                return this.groupId;
            }
        }
        public string ElementId
        {
            get
            {
                return this.elementId;
            }
        }
        public string Name
        {
            get
            {
                return this.name;
            }
        }
        public string VR
        {
            get
            {
                return vr;
            }
        }

        public string VM
        {
            get
            {
                return vm;
            }
        }
    }

    public static class DicomDictionary
    {
        private static List<DicomDictionaryEntry> dict = new List<DicomDictionaryEntry>();
        static DicomDictionary()
        {
            StreamReader reader=null;
            string line ;
            DicomDictionaryEntry dde;
            try
            {
                reader = new StreamReader("dicom.dic");
            }
            catch(Exception ex)
            {
                Console.WriteLine("异常：" + ex.Message);
            }

            if (reader != null)
            {
                while (reader.Peek() > -1)
                {
                    line = reader.ReadLine();
                    dde = new DicomDictionaryEntry(line);
                    dict.Add(dde);
                }
                reader.Close();
            }
        }
        public static DicomDictionaryEntry find(string tag)
        {
            return dict.Find((DicomDictionaryEntry dde) =>
                        {
                            for(int i=0; i<11;i++)        //逐位比较，处理字典tag中通配符x，比如"(xxxx,0000)"
                                if (dde.Tag[i] != tag[i] && dde.Tag[i] != 'x' ) 
                                    return false;
                            return true;
                        }
                );
        }
        public static DicomDictionaryEntry find(UInt16 GroupID, UInt16 ElementID)
        {
            string tag = "(" + GroupID.ToString("X4") + "," + ElementID.ToString("X4") + ")";
            return find(tag);
        }
    }
}
