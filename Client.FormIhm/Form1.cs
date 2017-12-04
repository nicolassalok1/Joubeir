using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace Client.FormIhm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonIATA_Click(object sender, EventArgs e)
        {
            var bagage = MyAirport.Pim.Models.Factory.Model.GetBagage(textBox1.Text);

        }

        private void buttonid_Click(object sender, EventArgs e)
        {
            var bagage = MyAirport.Pim.Models.Factory.Model.GetBagage(textBox1.Text);
        }

    }
}
