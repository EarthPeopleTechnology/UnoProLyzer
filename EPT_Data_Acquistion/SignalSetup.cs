using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;


namespace UnoProLyzer
{


    public partial class UnoProLyzer
    {

        private void cmbChannelSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Select the channel from the combo box selection
            ChannelSelectControl = Convert.ToInt32(cmbChannelSelect.Text);

            //Set all of the Channel Indicators to Regular Font
            SetChannelFontToRegular();

            trkVoltageScale.Value = VScaleChannel[ChannelSelectControl];
            trkVoltageScaleControl.Value = VerticalScale[ChannelSelectControl];
            trkHorizScaleControl.Value = HorizontalScale[ChannelSelectControl];
            trkTimeScale.Value = TimeScale[ChannelSelectControl];
            //Set all of the sliders to the previously selected channel positions
            //Set the Select Channel Indicator to Bold
            switch (ChannelSelectControl)
            {
                case 1:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_1_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_1_VerticalLabel.Font = new Font(Channel_1_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 2:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_2_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_2_VerticalLabel.Font = new Font(Channel_2_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 3:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_3_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_3_VerticalLabel.Font = new Font(Channel_3_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 4:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_4_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_4_VerticalLabel.Font = new Font(Channel_4_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    // else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 5:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_5_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_5_VerticalLabel.Font = new Font(Channel_5_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 6:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_6_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_6_VerticalLabel.Font = new Font(Channel_6_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 7:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_7_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_7_VerticalLabel.Font = new Font(Channel_7_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                case 8:
                    if (ActiveChannels[ChannelSelectControl - 1])
                    {
                        Channel_8_VerticalLabel.Font = new Font("Arial", 10);
                        Channel_8_VerticalLabel.Font = new Font(Channel_8_VerticalLabel.Font, FontStyle.Bold);
                        //btnChannelOnOff.Text = "On";
                    }
                    //else
                    //    btnChannelOnOff.Text = "Off";
                    break;
                default:
                    break;
            }


        }

        public int IncrEPTReceiveIndex(int SelectedChannel)
        {

            //Check the Index for a roll over condition, if below
            //rollover point, increment Index
            if (EPTReceiveIndex[SelectedChannel] > 49998)
                EPTReceiveIndex[SelectedChannel] = 0;

            //Check the Index to see if it has passed 450. If it has
            //not passed 450, the graph display will have to pad the 
            //Scope buffer data with zeros. This flag is used to communicate
            //if the Index has passed the initial 450.
            if (EPTReceiveIndex[SelectedChannel] > 450)
                EPTReceiveIndexInitFlag = true;

            if (EPTReceiveIndex[SelectedChannel] % 450 == 0)
                Invalidate();

                return EPTReceiveIndex[SelectedChannel]++;
        }

        public int GetScopeBufferIndex(int SelectedChannel, int IndexOffset)
        {
            if (!EPTReceiveIndexInitFlag)
                return 0;
            else if (EPTReceiveIndex[SelectedChannel] < (451 + IndexOffset))
                return (int)((EPTReceiveIndex[SelectedChannel] + 1) - (451 + IndexOffset)) + 49999;
            else
                return (int)(EPTReceiveIndex[SelectedChannel] - 451 + IndexOffset);
        }

        public void SetChannelFontToRegular()
        {
            //Set all Channel Indicators to Regular
            if (ActiveChannels[0])
            {
                Channel_1_VerticalLabel.Font = new Font(Channel_1_VerticalLabel.Font, FontStyle.Regular);
                Channel_1_VerticalLabel.Font = new Font("Arial", 8);
            }
            if(ActiveChannels[1])
            {
               Channel_2_VerticalLabel.Font = new Font(Channel_2_VerticalLabel.Font, FontStyle.Regular);
               Channel_2_VerticalLabel.Font = new Font("Arial", 8);
            }

            if(ActiveChannels[2])
            {
              Channel_3_VerticalLabel.Font = new Font(Channel_3_VerticalLabel.Font, FontStyle.Regular);
              Channel_3_VerticalLabel.Font = new Font("Arial", 8);
            }

            if(ActiveChannels[3])
            {
               Channel_4_VerticalLabel.Font = new Font(Channel_4_VerticalLabel.Font, FontStyle.Regular);
               Channel_4_VerticalLabel.Font = new Font("Arial", 8);
            }

            if(ActiveChannels[4])
            {
               Channel_5_VerticalLabel.Font = new Font(Channel_5_VerticalLabel.Font, FontStyle.Regular);
               Channel_5_VerticalLabel.Font = new Font("Arial", 8);
            }

            if(ActiveChannels[5])
            {
               Channel_6_VerticalLabel.Font = new Font(Channel_6_VerticalLabel.Font, FontStyle.Regular);
               Channel_6_VerticalLabel.Font = new Font("Arial", 8);
            }

            if(ActiveChannels[6])
            {
               Channel_7_VerticalLabel.Font = new Font(Channel_7_VerticalLabel.Font, FontStyle.Regular);
               Channel_7_VerticalLabel.Font = new Font("Arial", 8);
            }

            if(ActiveChannels[7])
            {
               Channel_8_VerticalLabel.Font = new Font(Channel_8_VerticalLabel.Font, FontStyle.Regular);
               Channel_8_VerticalLabel.Font = new Font("Arial", 8);
            }

        }

       private void trkHorizScaleControl_Scroll(object sender, EventArgs e)
        {
            HorizontalScale[ChannelSelectControl] = trkHorizScaleControl.Value;            
            Invalidate();
        }

        private void trkTimeScale_Scroll(object sender, EventArgs e)
        {
            TimeScale[ChannelSelectControl] = trkTimeScale.Value;
            
            Invalidate();
        }

        private void trkVoltageScaleControl_Scroll(object sender, EventArgs e)
        {
            //Set the Select Channel Indicator to Bold and
            //Set the Y position for the Seleted Channel Indicator.
            VerticalScale[ChannelSelectControl] = trkVoltageScaleControl.Value;
            switch (ChannelSelectControl)
            {
                case 1:
                    Channel_1_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 2:
                       Channel_2_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 3:
                       Channel_3_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 4:
                       Channel_4_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 5:
                       Channel_5_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 6:
                       Channel_6_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 7:
                       Channel_7_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                case 8:
                       Channel_8_VerticalLabel.Top = (250 - VerticalScale[ChannelSelectControl]);
                       break;
                default:
                    break;
            }

            UpdateVoltageScale();

            Invalidate();
        }

        private void trkVoltageScale_Scroll(object sender, EventArgs e)
        {
            //Declare a register to display on the Voltage Scale label
            double VoltageScaleC=0;
            VScaleChannel[ChannelSelectControl] = trkVoltageScale.Value; ;
            if (VScaleChannel[ChannelSelectControl] >= 5)
            {
                //Won't this always be 1?  
                VoltageScaleC = VScaleChannel[ChannelSelectControl] - 1;
                VoltageScaleFactor = (float)VoltageScaleC;
                lblVoltScale.Text = "Vertical Scale:\n 1";
                if (VScaleChannel[ChannelSelectControl] > 5)
                {
                    lblVoltScale.Text += "/" + VoltageScaleC;
                }
            }
            else 
            {
                VoltageScaleC = 6 - VScaleChannel[ChannelSelectControl];
                VoltageScaleFactor = (float)(1 / VoltageScaleC);
                lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
            }

            UpdateVoltageScale();

            Invalidate();

        }

        private void btnHorizFastDown_Click(object sender, EventArgs e)
        {
            if (trkHorizScaleControl.Value == 0)
                HorizontalFastShift -= 1;
            else
                HorizontalFastShift = HorizontalFastShift - Math.Abs(trkHorizScaleControl.Value);

            HorizontalFastShiftArray[ChannelSelectControl] = HorizontalFastShift;
            
            Invalidate();
        }

        private void btnHorizFastUp_Click(object sender, EventArgs e)
        {
                HorizontalFastKeyUp();
        }

        void HorizontalFastKeyUp()
        {
            if (trkHorizScaleControl.Value == 0)
                HorizontalFastShift += 1;
            else
                HorizontalFastShift = HorizontalFastShift + Math.Abs(trkHorizScaleControl.Value);

            HorizontalFastShiftArray[ChannelSelectControl] = HorizontalFastShift;
            
            Invalidate();

        }

        public void UpdateVoltageScale()
        {
            VoltageScaleLabel_P6 = -(float)(trkVoltageScaleControl.Value - 250) / 100;
            VoltageScaleLabel_P5 = -(float)(trkVoltageScaleControl.Value - 200) / 100;
            VoltageScaleLabel_P4 = -(float)(trkVoltageScaleControl.Value - 150) / 100;
            VoltageScaleLabel_P3 = -(float)(trkVoltageScaleControl.Value - 100) / 100;
            VoltageScaleLabel_P2 = -(float)(trkVoltageScaleControl.Value - 50) / 100;
            VoltageScaleLabel_P1 = -(float)(trkVoltageScaleControl.Value - 0) / 100;
            VoltageScaleLabel_M1 = -(float)(trkVoltageScaleControl.Value + 50) / 100;
            VoltageScaleLabel_M2 = -(float)(trkVoltageScaleControl.Value + 100) / 100;
            VoltageScaleLabel_M3 = -(float)(trkVoltageScaleControl.Value + 150) / 100;
            VoltageScaleLabel_M4 = -(float)(trkVoltageScaleControl.Value + 200) / 100;

            lblVoltage_P6.Text = VoltageScaleLabel_P6 * VoltageScaleFactor  + "V";
            lblVoltage_P5.Text = VoltageScaleLabel_P5 * VoltageScaleFactor + "V";
            lblVoltage_P4.Text = VoltageScaleLabel_P4 * VoltageScaleFactor + "V";
            lblVoltage_P3.Text = VoltageScaleLabel_P3 * VoltageScaleFactor + "V";
            lblVoltage_P2.Text = VoltageScaleLabel_P2 * VoltageScaleFactor + "V";
            lblVoltage_P1.Text = VoltageScaleLabel_P1 * VoltageScaleFactor + "V";
            lblVoltage_M1.Text = VoltageScaleLabel_M1 * VoltageScaleFactor + "V";
            lblVoltage_M2.Text = VoltageScaleLabel_M2 * VoltageScaleFactor + "V";
            lblVoltage_M3.Text = VoltageScaleLabel_M3 * VoltageScaleFactor + "V";
            lblVoltage_M4.Text = VoltageScaleLabel_M4 * VoltageScaleFactor + "V";
        }



//End of cs file
    }
}
