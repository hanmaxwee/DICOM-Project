using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DCMLIB;

namespace testDCMLIB
{
    public partial class DicomParser : Form
    {
        public DicomParser()
        {
            InitializeComponent();
        }

        private void DicomParser_Load(object sender, EventArgs e)
        {
            cbTransferSyntax.Items.Clear();
            foreach (KeyValuePair<string, TransferSyntax> syntax in TransferSyntaxs.All)
                cbTransferSyntax.Items.Add(syntax.Value);
            cbTransferSyntax.SelectedIndex = 0;

        }

        private byte[] HexStringToByteArray(string hexs)
        {
            string str = hexs.Replace(" ", "");
            byte[] data = new byte[str.Length/2];
            for(int i=0; i<str.Length; i+=2)
            {
                data[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
            }
            return data;
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            string hex = txtInput.Text.Trim();
            byte[] data;
            uint idx;
            string res;
            data = HexStringToByteArray(hex);
            DCMDataSet dds = new DCMDataSet((TransferSyntax)cbTransferSyntax.SelectedItem);
            idx = 0;
            dds.Decode(data, ref idx);
            res = dds.ToString("");
            string[] lines = res.Split('\n');
            lvOutput.Items.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                ListViewItem item = new ListViewItem(lines[i].Split('\t'));
                lvOutput.Items.Add(item);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtInput.Clear();
            lvOutput.Items.Clear();
        }
    }
}
