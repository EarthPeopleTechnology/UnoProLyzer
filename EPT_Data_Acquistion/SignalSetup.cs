using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;


namespace EPT_Data_Acquistion
{


    public partial class EPT_Data_Acquistion
    {

        private void cmbChannelSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Select the channel from the combo box selection
            ChannelSelectControl = Convert.ToInt32(cmbChannelSelect.Text);

            //Set all of the Channel Indicators to Regular Font
            SetChannelFontToRegular();

            //Set all of the sliders to the previously selected channel positions
            //Set the Select Channel Indicator to Bold
            switch (ChannelSelectControl)
            {
                case 1:
                    trkVoltageScale.Value = VScaleChannel_1;
                    trkVoltageScaleControl.Value = VerticalScale_1;
                    trkHorizScaleControl.Value = HorizontalScale_1;
                    trkTimeScale.Value = TimeScale_1;
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
                    trkVoltageScale.Value = VScaleChannel_2;
                    trkVoltageScaleControl.Value = VerticalScale_2;
                    trkHorizScaleControl.Value = HorizontalScale_2;
                    trkTimeScale.Value = TimeScale_2;
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
                    trkVoltageScale.Value = VScaleChannel_3;
                    trkVoltageScaleControl.Value = VerticalScale_3;
                    trkHorizScaleControl.Value = HorizontalScale_3;
                    trkTimeScale.Value = TimeScale_3;
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
                    trkVoltageScale.Value = VScaleChannel_4;
                    trkVoltageScaleControl.Value = VerticalScale_4;
                    trkHorizScaleControl.Value = HorizontalScale_4;
                    trkTimeScale.Value = TimeScale_4;
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
                    trkVoltageScale.Value = VScaleChannel_5;
                    trkVoltageScaleControl.Value = VerticalScale_5;
                    trkHorizScaleControl.Value = HorizontalScale_5;
                    trkTimeScale.Value = TimeScale_5;
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
                    trkVoltageScale.Value = VScaleChannel_6;
                    trkVoltageScaleControl.Value = VerticalScale_6;
                    trkHorizScaleControl.Value = HorizontalScale_6;
                    trkTimeScale.Value = TimeScale_6;
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
                    trkVoltageScale.Value = VScaleChannel_7;
                    trkVoltageScaleControl.Value = VerticalScale_7;
                    trkHorizScaleControl.Value = HorizontalScale_7;
                    trkTimeScale.Value = TimeScale_7;
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
                    trkVoltageScale.Value = VScaleChannel_8;
                    trkVoltageScaleControl.Value = VerticalScale_8;
                    trkHorizScaleControl.Value = HorizontalScale_8;
                    trkTimeScale.Value = TimeScale_8;
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
            switch (ChannelSelectControl)
            {
                case 1:
                    HorizontalScale_1 = trkHorizScaleControl.Value;
                    break;
                case 2:
                    HorizontalScale_2 = trkHorizScaleControl.Value;
                    break;
                case 3:
                    HorizontalScale_3 = trkHorizScaleControl.Value;
                    break;
                case 4:
                    HorizontalScale_4 = trkHorizScaleControl.Value;
                    break;
                case 5:
                    HorizontalScale_5 = trkHorizScaleControl.Value;
                    break;
                case 6:
                    HorizontalScale_6 = trkHorizScaleControl.Value;
                    break;
                case 7:
                    HorizontalScale_7 = trkHorizScaleControl.Value;
                    break;
                case 8:
                    HorizontalScale_8 = trkHorizScaleControl.Value;
                    break;

                default:
                    break;
            }
            Invalidate();
        }

                private void trkTimeScale_Scroll(object sender, EventArgs e)
        {
            switch (ChannelSelectControl)
            {
                case 1:
                    TimeScale_1 = trkTimeScale.Value;
                    break;
                case 2:
                    TimeScale_2 = trkTimeScale.Value;
                    break;
                case 3:
                    TimeScale_3 = trkTimeScale.Value;
                    break;
                case 4:
                    TimeScale_4 = trkTimeScale.Value;
                    break;
                case 5:
                    TimeScale_5 = trkTimeScale.Value;
                    break;
                case 6:
                    TimeScale_6 = trkTimeScale.Value;
                    break;
                case 7:
                    TimeScale_7 = trkTimeScale.Value;
                    break;
                case 8:
                    TimeScale_8 = trkTimeScale.Value;
                    break;

                default:
                    break;
            }
            Invalidate();

        }

        private void trkVoltageScaleControl_Scroll(object sender, EventArgs e)
        {
            //Set the Select Channel Indicator to Bold and
            //Set the Y position for the Seleted Channel Indicator.
            switch (ChannelSelectControl)
            {
                case 1:
                       VerticalScale_1 = trkVoltageScaleControl.Value;
                       Channel_1_VerticalLabel.Top = (250 - VerticalScale_1);
                       break;
                case 2:
                       VerticalScale_2 = trkVoltageScaleControl.Value;
                       Channel_2_VerticalLabel.Top = (250 - VerticalScale_2);
                       break;
                case 3:
                       VerticalScale_3 = trkVoltageScaleControl.Value;
                       Channel_3_VerticalLabel.Top = (250 - VerticalScale_3);
                       break;
                case 4:
                       VerticalScale_4 = trkVoltageScaleControl.Value;
                       Channel_4_VerticalLabel.Top = (250 - VerticalScale_4);
                       break;
                case 5:
                       VerticalScale_5 = trkVoltageScaleControl.Value;
                       Channel_5_VerticalLabel.Top = (250 - VerticalScale_5);
                       break;
                case 6:
                       VerticalScale_6 = trkVoltageScaleControl.Value;
                       Channel_6_VerticalLabel.Top = (250 - VerticalScale_6);
                       break;
                case 7:
                       VerticalScale_7 = trkVoltageScaleControl.Value;
                       Channel_7_VerticalLabel.Top = (250 - VerticalScale_7);
                       break;
                case 8:
                       VerticalScale_8 = trkVoltageScaleControl.Value;
                       Channel_8_VerticalLabel.Top = (250 - VerticalScale_8);
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

            switch (ChannelSelectControl)
            {
                case 1:
                    VScaleChannel_1 = trkVoltageScale.Value;
                    if (VScaleChannel_1 == 5)
                    {
                        VoltageScaleC = VScaleChannel_1 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_1 > 5)
                    {
                        VoltageScaleC = VScaleChannel_1 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_1 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_1;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 2:
                    VScaleChannel_2 = trkVoltageScale.Value;
                    if(VScaleChannel_2 == 5)
                    {
                        VoltageScaleC = VScaleChannel_2 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_2 > 5)
                    {
                        VoltageScaleC = VScaleChannel_2 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_2 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_2;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 3:
                    VScaleChannel_3 = trkVoltageScale.Value;
                    if (VScaleChannel_3 == 5)
                    {
                        VoltageScaleC = VScaleChannel_3 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_3 > 5)
                    {
                        VoltageScaleC = VScaleChannel_3 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_3 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_3;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 4:
                    VScaleChannel_4 = trkVoltageScale.Value;
                    if (VScaleChannel_4 == 5)
                    {
                        VoltageScaleC = VScaleChannel_4 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_4 > 5)
                    {
                        VoltageScaleC = VScaleChannel_4 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_4 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_4;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 5:
                    VScaleChannel_5 = trkVoltageScale.Value;
                    if (VScaleChannel_5 == 5)
                    {
                        VoltageScaleC = VScaleChannel_5 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_5 > 5)
                    {
                        VoltageScaleC = VScaleChannel_5 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_5 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_5;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 6:
                    VScaleChannel_6 = trkVoltageScale.Value;
                    if (VScaleChannel_6 == 5)
                    {
                        VoltageScaleC = VScaleChannel_6 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_6 > 5)
                    {
                        VoltageScaleC = VScaleChannel_6 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_6 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_6;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 7:
                    VScaleChannel_7 = trkVoltageScale.Value;
                    if (VScaleChannel_7 == 5)
                    {
                        VoltageScaleC = VScaleChannel_7 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_7 > 5)
                    {
                        VoltageScaleC = VScaleChannel_7 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_7 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_7;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                case 8:
                    VScaleChannel_8 = trkVoltageScale.Value;
                    if (VScaleChannel_8 == 5)
                    {
                        VoltageScaleC = VScaleChannel_8 - 4;
                        VoltageScaleFactor = (float)VoltageScaleC;
                        lblVoltScale.Text = "Vertical Scale:\n 1";
                    }
                    else if (VScaleChannel_8 > 5)
                    {
                        VoltageScaleC = VScaleChannel_8 - 4;
                        VoltageScaleFactor = (float)(VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n 1/" + VoltageScaleC;
                    }
                    else
                    {
                        //VoltageScaleC = (double)(VScaleChannel_8 / 5.0);
                        //lblVoltScale.Text = VoltageScaleC.ToString() + " Volt\n Per Division";
                        VoltageScaleC = 6 - VScaleChannel_8;
                        VoltageScaleFactor = (float)(1 / VoltageScaleC);
                        lblVoltScale.Text = "Vertical Scale:\n * " + VoltageScaleC;
                    }
                    break;
                default:
                    break;
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


            switch (ChannelSelectControl)
            {
                case 1:
                    HorizontalFastShift_1 = HorizontalFastShift;
                    break;
                case 2:
                    HorizontalFastShift_2 = HorizontalFastShift;
                    break;
                case 3:
                    HorizontalFastShift_3 = HorizontalFastShift;
                    break;
                case 4:
                    HorizontalFastShift_4 = HorizontalFastShift;
                    break;
                case 5:
                    HorizontalFastShift_5 = HorizontalFastShift;
                    break;
                case 6:
                    HorizontalFastShift_6 = HorizontalFastShift;
                    break;
                case 7:
                    HorizontalFastShift_7 = HorizontalFastShift;
                    break;
                case 8:
                    HorizontalFastShift_8 = HorizontalFastShift;
                    break;

                default:
                    break;
            }
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


            switch (ChannelSelectControl)
            {
                case 1:
                    HorizontalFastShift_1 = HorizontalFastShift;
                    break;
                case 2:
                    HorizontalFastShift_2 = HorizontalFastShift;
                    break;
                case 3:
                    HorizontalFastShift_3 = HorizontalFastShift;
                    break;
                case 4:
                    HorizontalFastShift_4 = HorizontalFastShift;
                    break;
                case 5:
                    HorizontalFastShift_5 = HorizontalFastShift;
                    break;
                case 6:
                    HorizontalFastShift_6 = HorizontalFastShift;
                    break;
                case 7:
                    HorizontalFastShift_7 = HorizontalFastShift;
                    break;
                case 8:
                    HorizontalFastShift_8 = HorizontalFastShift;
                    break;

                default:
                    break;
            }
            Invalidate();

        }

        public void UpdateVoltageScale()
        {
/*            VoltageScaleLabel_P6 = -(float)(VerticalScale_1 - 250) / 50;
            VoltageScaleLabel_P5 = -(float)(VerticalScale_1 - 200) / 50;
            VoltageScaleLabel_P4 = -(float)(VerticalScale_1 - 150) / 50;
            VoltageScaleLabel_P3 = -(float)(VerticalScale_1 - 100) / 50;
            VoltageScaleLabel_P2 = -(float)(VerticalScale_1 - 50) / 50;
            VoltageScaleLabel_P1 = -(float)(VerticalScale_1 - 0) / 50;
            VoltageScaleLabel_M1 = -(float)(VerticalScale_1 + 50) / 50;
            VoltageScaleLabel_M2 = -(float)(VerticalScale_1 + 100) / 50;
            VoltageScaleLabel_M3 = -(float)(VerticalScale_1 + 150) / 50;
            VoltageScaleLabel_M4 = -(float)(VerticalScale_1 + 200) / 50;
*/

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
