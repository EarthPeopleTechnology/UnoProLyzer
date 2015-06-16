using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnoProLyzer
{
    public partial class DebugConsole : Form
    {
        public DebugConsole()
        {
            InitializeComponent();
        }

        private void DebugConsole_Load(object sender, EventArgs e)
        {

        }

        public void log_text(String log )
        {
            this.rt_debug_text.AppendText(log + '\u2028');
        }
    }
}
