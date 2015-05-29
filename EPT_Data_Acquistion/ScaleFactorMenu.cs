using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;


namespace EPT_Data_Acquisition
{


    public partial class EPT_Data_Acquisition
    {

        //Textboxes to add
        TextBox ScaleFactor1TextBox = null;
        TextBox ScaleFactor2TextBox = null;
        TextBox ScaleFactor3TextBox = null;
        TextBox ScaleFactor4TextBox = null;
        TextBox ScaleFactor5TextBox = null;
        TextBox ScaleFactor6TextBox = null;

        //Buttons to add
        Button ScaleFactorExitButton = null;

        //Labels to add
        Label ScaleFactor1Label = null;
        Label ScaleFactor2Label = null;
        Label ScaleFactor3Label = null;
        Label ScaleFactor4Label = null;
        Label ScaleFactor5Label = null;
        Label ScaleFactor6Label = null;
        Label ScaleFactorStatus = null;

        //Repeat and delay for Send File
        public float ScaleFactor1 = 0.0f;
        public float ScaleFactor2 = 0.0f;
        public float ScaleFactor3 = 0.0f;
        public float ScaleFactor4 = 0.0f;
        public float ScaleFactor5 = 0.0f;
        public float ScaleFactor6 = 0.0f;

        public void ScaleFactorMenuOpenWindow()
         {
             this.AutoSize = false;


             //======================================
             // Scale Factor Menu Add TextBoxes
             //======================================

             //Textbox to Enter Path and Filename of File to Send
             ScaleFactor1TextBox = new TextBox();
             ScaleFactor1TextBox.Location = new System.Drawing.Point(420, 100);
             ScaleFactor1TextBox.Size = new System.Drawing.Size(50, 20);
             ScaleFactor1TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             ScaleFactor1TextBox.Visible = true;
             ScaleFactor1TextBox.Text = "0.0";
             this.ScaleFactor1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleFactor1TextBox_KeyPress);
             this.Controls.Add(ScaleFactor1TextBox);
             //Resize the window for both textboxes.
             this.Size = new Size(600, 400);


             //Text box to display number of times to repeat file send
             ScaleFactor2TextBox = new TextBox();
             ScaleFactor2TextBox.Location = new System.Drawing.Point(520, 100);
             ScaleFactor2TextBox.Size = new System.Drawing.Size(50, 20);
             ScaleFactor2TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             ScaleFactor2TextBox.Visible = true;
             ScaleFactor2TextBox.Text = "0.0";
             //this.toolTip1.SetToolTip(this.ScaleFactor2TextBox, "Enter 0 to repeat forever");
             this.ScaleFactor2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleFactor2TextBox_KeyPress);
             this.Controls.Add(ScaleFactor2TextBox);

             //Text box to display delay to add file send
             ScaleFactor3TextBox = new TextBox();
             ScaleFactor3TextBox.Location = new System.Drawing.Point(420, 150);
             ScaleFactor3TextBox.Size = new System.Drawing.Size(50, 20);
             ScaleFactor3TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             ScaleFactor3TextBox.Visible = true;
             ScaleFactor3TextBox.Text = "0.0";
             this.ScaleFactor3TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleFactor3TextBox_KeyPress);
             this.Controls.Add(ScaleFactor3TextBox);

             //Textbox for Adding Characters to send
             ScaleFactor4TextBox = new TextBox();
             ScaleFactor4TextBox.Location = new System.Drawing.Point(520, 150);
             ScaleFactor4TextBox.Size = new System.Drawing.Size(50, 20);
             ScaleFactor4TextBox.Visible = true;
             ScaleFactor4TextBox.Text = "0.0";
             //this.toolTip1.SetToolTip(this.ScaleFactor4TextBox, "Enter Hex Values to Send Seperated by spaces or commas");
             this.ScaleFactor4TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleFactor4TextBox_KeyPress);
             this.Controls.Add(ScaleFactor4TextBox);

             //Text box to display delay to add Hex Value send
             ScaleFactor5TextBox = new TextBox();
             ScaleFactor5TextBox.Location = new System.Drawing.Point(420, 200);
             ScaleFactor5TextBox.Size = new System.Drawing.Size(50, 20);
             ScaleFactor5TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             ScaleFactor5TextBox.Visible = true;
             ScaleFactor5TextBox.Text = "0.0";
             this.ScaleFactor5TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleFactor5TextBox_KeyPress);
             this.Controls.Add(ScaleFactor5TextBox);

             //Text box to display number of times to repeat Hex Value send
             ScaleFactor6TextBox = new TextBox();
             ScaleFactor6TextBox.Location = new System.Drawing.Point(520, 200);
             ScaleFactor6TextBox.Size = new System.Drawing.Size(50, 20);
             ScaleFactor6TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
             ScaleFactor6TextBox.Visible = true;
             ScaleFactor6TextBox.Text = "0.0";
             //this.toolTip1.SetToolTip(this.ScaleFactor6TextBox, "Enter 0 to repeat forever");
             this.ScaleFactor6TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ScaleFactor6TextBox_KeyPress);
             this.Controls.Add(ScaleFactor6TextBox);

             //======================================
             // Scale Factor Menu Add Buttons
             //======================================


             //Exit button to end SendMenu
             ScaleFactorExitButton = new Button();
             ScaleFactorExitButton.Location = new Point(540, 10);
             ScaleFactorExitButton.FlatAppearance.BorderSize = 1;
             ScaleFactorExitButton.Text = "Exit";
             ScaleFactorExitButton.Size = new System.Drawing.Size(40, 20);
             //this.toolTip1.SetToolTip(this.ScaleFactorExitButton, "Exit");
             this.Controls.Add(ScaleFactorExitButton);
             ScaleFactorExitButton.Click += new EventHandler(ScaleFactorExitButton_Click);

             //======================================
             // Scale Factor Menu Add Labels
             //======================================

             //Add File to send Label to Send Textbox
             ScaleFactor1Label = new Label();
             ScaleFactor1Label.Location = new Point(400, 80);
             ScaleFactor1Label.Font = new Font("Arial", 8);
             ScaleFactor1Label.Text = "ScaleFactor 1";
             this.Controls.Add(ScaleFactor1Label);

             //Add Label to Characters to send Textbox
             ScaleFactor2Label = new Label();
             ScaleFactor2Label.Location = new Point(500, 80);
             ScaleFactor2Label.Font = new Font("Arial", 8);
             ScaleFactor2Label.Text = "ScaleFactor 2";
             this.Controls.Add(ScaleFactor2Label);

             //Add Label to File Repeat Textbox
             ScaleFactor3Label = new Label();
             ScaleFactor3Label.Location = new Point(400, 130);
             ScaleFactor3Label.Font = new Font("Arial", 8);
             ScaleFactor3Label.Text = "ScaleFactor 3";
             this.Controls.Add(ScaleFactor3Label);

             //Add Label for File Delay Textbox
             ScaleFactor4Label = new Label();
             ScaleFactor4Label.Location = new Point(500, 130);
             ScaleFactor4Label.Font = new Font("Arial", 8);
             ScaleFactor4Label.Text = "ScaleFactor 4";
             this.Controls.Add(ScaleFactor4Label);

             //Add Label for File Status
             ScaleFactor5Label = new Label();
             ScaleFactor5Label.Location = new Point(400, 180);
             ScaleFactor5Label.Font = new Font("Arial", 8);
             ScaleFactor5Label.Text = "ScaleFactor 5";
             this.Controls.Add(ScaleFactor5Label);

            
             //Add Label to Hex Value Repeat Textbox
             ScaleFactor6Label = new Label();
             ScaleFactor6Label.Location = new Point(500, 180);
             ScaleFactor6Label.Font = new Font("Arial", 8);
             ScaleFactor6Label.Text = "ScaleFactor 6";
             this.Controls.Add(ScaleFactor6Label);

             //Add Label for Hex Status
             ScaleFactorStatus = new Label();
             ScaleFactorStatus.Location = new Point(420, 50);
             ScaleFactorStatus.Font = new Font("Arial", 8);
             ScaleFactorStatus.Text = "Set the Scale Factor for \nEach Individual Measurement";
             this.Controls.Add(ScaleFactorStatus);

             //Set the anchors for the SendMenu Controls
             ScaleFactorMenuSetAnchor();

         }

        public void ScaleFactorMenuSetAnchor()
         {
             //Set the anchor style for the Textboxes
             this.ScaleFactor1TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left );
             this.ScaleFactor2TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor4TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor3TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor5TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor6TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

             //Set the anchor for the buttons
             this.ScaleFactorExitButton.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

             //Set the anchor for the labels
             this.ScaleFactor1Label.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor2Label.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor3Label.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor4Label.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor5Label.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor6Label.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactorStatus.Anchor = (AnchorStyles.Top | AnchorStyles.Left);

         }

        public void ScaleFactorMenuResetAnchor()
         {
             //Set the anchor style of textbox1 to accomodate the multi window
             //Set the anchor style of ScaleFactor1TextBox window
             this.ScaleFactor1TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left );
             this.ScaleFactor2TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor3TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor4TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor5TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
             this.ScaleFactor6TextBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
         }


         private void ScaleFactor1TextBox_KeyPress(object sender, EventArgs e)
         {
             //Read the scale factor 1 from text box.
             ScaleFactor1 = (float)Convert.ToDouble(ScaleFactor1TextBox.Text);

          }

         public void ScaleFactor2TextBox_KeyPress(object sender, EventArgs e)
        {
            //Read the scale factor 2 from text box.
            ScaleFactor2 = (float)Convert.ToDouble(ScaleFactor2TextBox.Text);
            
         }

         private void ScaleFactor3TextBox_KeyPress(object sender, EventArgs e)
        {

            //Read the scale factor 3 from text box.
            ScaleFactor3 = (float)Convert.ToDouble(ScaleFactor3TextBox.Text);
        }

         private void ScaleFactor4TextBox_KeyPress(object sender, EventArgs e)
        {

            //Read the scale factor 4 from text box.
            ScaleFactor4 = (float)Convert.ToDouble(ScaleFactor4TextBox.Text);
        }
         private void ScaleFactor5TextBox_KeyPress(object sender, EventArgs e)
        {

            //Read the scale factor 5 from text box.
            ScaleFactor5 = (float)Convert.ToDouble(ScaleFactor5TextBox.Text);

        }
         private void ScaleFactor6TextBox_KeyPress(object sender, EventArgs e)
        {

            //Read the scale factor 6 from text box.
            ScaleFactor6 = (float)Convert.ToDouble(ScaleFactor6TextBox.Text);
        }
 
            private void ScaleFactorExitButton_Click(object sender, EventArgs e)
         {
             //TextBox to Dispose
             ScaleFactor1TextBox.Dispose();
             ScaleFactor2TextBox.Dispose();
             ScaleFactor4TextBox.Dispose();
             ScaleFactor5TextBox.Dispose();
             ScaleFactor6TextBox.Dispose();

             //Buttons to Dispose
             ScaleFactorExitButton.Dispose();

             //Labels to Dispose
             ScaleFactor3Label.Dispose();
             ScaleFactor4Label.Dispose();
             ScaleFactor1Label.Dispose();
             ScaleFactor5Label.Dispose();
             ScaleFactor2Label.Dispose();
             ScaleFactor6Label.Dispose();
             ScaleFactorStatus.Dispose();

             //Size the Window to its original size
             this.Size = new System.Drawing.Size(400, 400);
             //Anchor the textbox to the Windows Form.
             this.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);

         }


    }
}
