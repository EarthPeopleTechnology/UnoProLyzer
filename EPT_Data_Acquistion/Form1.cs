using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace EPT_Data_Acquisition
{
    public partial class EPT_Data_Acquisition : Form
    {
        #region Constant declaration and variable initialization
        int[] StorageArray = new int[50000];

        public EPT_Data_Acquisition()
        {
            InitializeComponent();


            for (int i = 0; i < EPTTransmitDevice.Length; ++i)
            {
                EPTTransmitDevice[i] = new Transfer();
            }

            InitializeStorageArray();

            // reduce flicker

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }
        //Index to store device selection
        Int32 device_index;

        //Create a Receive object of the Transfer Class.
        Transfer EPTReceiveData = new Transfer();

        //Create an array of the Transfer Class for device
        Transfer[] EPTTransmitDevice = new Transfer[8];

        //Parameters for EPT Receive object
        public const byte TRIGGER_OUT_COMMAND = 0x1;
        public const byte TRANSFER_OUT_COMMAND = 0x2;
        public const byte BLOCK_OUT_COMMAND = 0x4;

        //Parameters for EPT Receive object
        public const byte TRANSFER_OUT_ADDRESS_1 = 0x1;
        public const byte TRANSFER_OUT_ADDRESS_2 = 0x2;
        public const byte TRANSFER_OUT_ADDRESS_3 = 0x3;
        public const byte TRANSFER_OUT_ADDRESS_4 = 0x4;
        public const byte TRANSFER_OUT_ADDRESS_5 = 0x5;
        public const byte TRANSFER_OUT_ADDRESS_6 = 0x6;

        //Address variables to keep track of which Transfer
        //group should store the Display byte from the EPT-570
        public bool DisplayAddress_1 = true;
        public bool DisplayAddress_2 = false;
        public bool DisplayAddress_3 = false;
        public bool DisplayAddress_4 = false;
        public bool DisplayAddress_5 = false;
        public bool DisplayAddress_6 = false;

        //Channel Select flags to determine which channels
        //are in the ADC scan
        public bool Channel1_Select = false;
        public bool Channel2_Select = false;
        public bool Channel3_Select = false;
        public bool Channel4_Select = false;
        public bool Channel5_Select = false;
        public bool Channel6_Select = false;


        //Register to keep track of which byte (Upper or Lower)
        //should be assembled into the AnalogValue
        public bool FirstDisplayByte = true;

        //Address variables to store history of the addresses
        //from the EPT Receive object
        public int FirstPreviousActualAddr = 0;
        public int SecondPreviousActualAddr = 0;

        //Address variables to store history of the addresses
        //determined by the case statements 
        public int FirstPreviousCalcAddr = 0;
        public int SecondPreviousCalcAddr = 0;

        //Variable to store the 10 bit Analog Value
        public int AnalogValue = 0;
        public float FloatValue = 0;


        //Add Oscilloscope values  here
        //Parameters for Voltage Reference Buttons
        public const int SELECT_VREF1 = 1;
        public const int SELECT_VREF2 = 2;
        public const int SELECT_VREF3 = 3;

        //Parameters for Voltage Reference Buttons
        public const int SELECT_TRIG1 = 1;
        public const int SELECT_TRIG2 = 2;
        public const int SELECT_TRIG3 = 3;

        //Parameters for the Fifo hold off
        public const int REFRESH_TIMEOUT = 150;
        public const int FIFO_HOLD_TIME = 1200;

        //Buffer to hold bytes transferred from the EPT Serial Graph Tool
        public int[][] ScopeBuffer = new int[8][];
        public int[] EPTReceiveIndex = new int[8];
        public int OuterDimension = 0;

        // put the a/d readings into repeat mode for 450 readings
        int n = 0;

        //ScopeBuffer Channel Select
        public int ScopeBufferChannelSelect = 0;

        //Flag to indicate if the ScopeBuffer has collected the first
        //450 data points
        public bool EPTReceiveIndexInitFlag = false;

        //ScopeBuffer index for the graph section
        public int ScopeIndex = 0;

        //Offset value to change the Horizontal position
        public int IndexOffset = 0;

        //Flag to prevent BlockOutReceive or TriggerOutRecieve from writing
        // to the ScopeBuffer or initiating RefreshScope() while RefreshScope()
        //is in process
        public bool RefreshBusy = false;

        private bool StorageOn = false;

        //Prescalar value
        public char[] PreScalarValue = new char[10];

        //Voltage Reference value
        public char VoltageReference = '1';


        //Voltage Reference value
        public char TriggerEvent = '1';

        //FifoHold register
        public bool FifoHold = false;
        public bool SetFifoHoldEnable = false;

        //Prescalar value
        public char[] TriggerThreshold = new char[10];

        //ScopeOn register to allow graph to be turned on/off
        bool ScopeOn = false;

        //Scaling to be used on the incoming data
        public int BitScale = 1024;

        //Selected Channel control, used to get the value from the trackbars
        //and place them into the channel values
        public int ChannelSelectControl = 0;

        //Array of active channels, used to determine if a channel
        //memory block has been set up
        public bool[] ActiveChannels = new bool[8];

        //Registers to Determine if the incoming data is two bytes. Determine
        //whether it is the upper byte or lower byte.
        public bool TwoByteData = false;
        public bool UpperByte = true;

        //StreamWrite File
        //StreamWriter writer;

        //Save File string
        public string Saved_File;
        public string previousPath = "";

        //Save Data To File Flag
        public bool SaveDataToFile = false;
        //Voltage Scale registers for each channel
        //Voltage Scale Factor
        public int VoltageScaleValue = 5;
        public int VScaleChannel_1 = 5;
        public int VScaleChannel_2 = 5;
        public int VScaleChannel_3 = 5;
        public int VScaleChannel_4 = 5;
        public int VScaleChannel_5 = 5;
        public int VScaleChannel_6 = 5;
        public int VScaleChannel_7 = 5;
        public int VScaleChannel_8 = 5;

        //Vertical Scale registers for each channel
        public int VerticalScaleValue = 0;
        public int VerticalScale_1 = 0;
        public int VerticalScale_2 = 0;
        public int VerticalScale_3 = 0;
        public int VerticalScale_4 = 0;
        public int VerticalScale_5 = 0;
        public int VerticalScale_6 = 0;
        public int VerticalScale_7 = 0;
        public int VerticalScale_8 = 0;

        //Horizontal Position registers
        public int HorizontalScaleValue = 0;
        public int HorizontalScale_1 = 0;
        public int HorizontalScale_2 = 0;
        public int HorizontalScale_3 = 0;
        public int HorizontalScale_4 = 0;
        public int HorizontalScale_5 = 0;
        public int HorizontalScale_6 = 0;
        public int HorizontalScale_7 = 0;
        public int HorizontalScale_8 = 0;

        //Time Scale registers
        public int TimeScaleValue = 1;
        public int TimeScale_1 = 1;
        public int TimeScale_2 = 1;
        public int TimeScale_3 = 1;
        public int TimeScale_4 = 1;
        public int TimeScale_5 = 1;
        public int TimeScale_6 = 1;
        public int TimeScale_7 = 1;
        public int TimeScale_8 = 1;

        //Horizontal Fast Shift Registers
        public int HorizontalFastShift = 0;
        public int HorizontalFastShift_1 = 0;
        public int HorizontalFastShift_2 = 0;
        public int HorizontalFastShift_3 = 0;
        public int HorizontalFastShift_4 = 0;
        public int HorizontalFastShift_5 = 0;
        public int HorizontalFastShift_6 = 0;
        public int HorizontalFastShift_7 = 0;
        public int HorizontalFastShift_8 = 0;

        //Labels to add
        Label Channel_1_VerticalLabel = null;
        Label Channel_2_VerticalLabel = null;
        Label Channel_3_VerticalLabel = null;
        Label Channel_4_VerticalLabel = null;
        Label Channel_5_VerticalLabel = null;
        Label Channel_6_VerticalLabel = null;
        Label Channel_7_VerticalLabel = null;
        Label Channel_8_VerticalLabel = null;

        //Voltage Scale Labels
        public float VoltageScaleLabel_P6;
        public float VoltageScaleLabel_P5;
        public float VoltageScaleLabel_P4;
        public float VoltageScaleLabel_P3;
        public float VoltageScaleLabel_P2;
        public float VoltageScaleLabel_P1;
        public float VoltageScaleLabel_M1;
        public float VoltageScaleLabel_M2;
        public float VoltageScaleLabel_M3;
        public float VoltageScaleLabel_M4;

        //Voltage Scale Factor
        public float VoltageScaleFactor = 1;

        #endregion

        // Main object loader
        private void EPT_Data_Acquisition_Load(object sender, System.EventArgs e)
        {
            // Call the List Devices function
            ListDevices();
            lblDeviceConnected.Text = "";

            //EPT_AH_SetDebugMode(1);
        }


        private void SetButtonEnables()
        {
            btnOpenDevice.Enabled = true;
            btnCloseDevice.Enabled = false;
        }


        private void btnOpenDevice_Click(object sender, EventArgs e)
        {
            //Open the Device
            OpenDevice();
            lblDeviceConnected.Text = "Device Connected";
            btnOpenDevice.Enabled = false;
            btnCloseDevice.Enabled = true;
        }

        private void btnCloseDevice_Click(object sender, EventArgs e)
        {
            EPT_AH_CloseDeviceByIndex(device_index);
            btnOpenDevice.Enabled = true;
            btnCloseDevice.Enabled = false;

            lblDeviceConnected.Text = "";

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            EPT_AH_CloseDeviceByIndex(device_index);
            btnOpenDevice.Enabled = true;
            btnCloseDevice.Enabled = false;

            lblDeviceConnected.Text = "";
            Application.Exit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EPT_AH_CloseDeviceByIndex(device_index);
            btnOpenDevice.Enabled = true;
            btnCloseDevice.Enabled = false;

            lblDeviceConnected.Text = "";
            Application.Exit();
        }

        //Add Oscilloscope Graph Code here

        void InitializeStorageArray()
        {
            for (int i = 0; i < StorageArray.Length; i++)
            {
                StorageArray[i] = 0;
            }

            //Initialize the first channel of the Serial Graph Tool
            ActiveChannels[0] = true;
            ScopeBuffer[0] = new int[50000];
            cmbChannelSelect.Items.Add((ScopeBufferChannelSelect + 1));
            Channel_1_VerticalLabel = new Label();
            Channel_1_VerticalLabel.ForeColor = Color.DarkGreen;
            Channel_1_VerticalLabel.Location = new Point(480, 250);
            Channel_1_VerticalLabel.Font = new Font("Arial", 8);
            Channel_1_VerticalLabel.Text = "--Ch 1";
            this.Controls.Add(Channel_1_VerticalLabel);
        }

        Random r = new Random();

        private void RefreshScope(Graphics g)
        {
            int xmax = 450; // number of sample points
            int ymax = 256;  // maximum y value
            int xlast = -1;
            int ylast = 0;
            int xDraw = 0;
            int yDraw = 0;

            int nToggleColor = 0;
            int TimeScaleIn = 0;

            // put the a/d readings into repeat mode for 450 readings
            //int n = 0;

            RefreshBusy = true;

            try
            {
                for (int OuterDimension = 0; OuterDimension < 8; OuterDimension++)
                {
                    xlast = -1;
                    ylast = 0;
                    xDraw = 0;
                    yDraw = 0;
                    TimeScaleIn = 0;
                    //n = 0;

                    switch (OuterDimension)
                    {
                        case 0:
                            VoltageScaleValue = VScaleChannel_1;
                            VerticalScaleValue = VerticalScale_1;
                            HorizontalScaleValue = HorizontalScale_1;
                            TimeScaleValue = TimeScale_1;
                            IndexOffset = HorizontalScale_1 + HorizontalFastShift_1;
                            break;
                        case 1:
                            VoltageScaleValue = VScaleChannel_2;
                            VerticalScaleValue = VerticalScale_2;
                            HorizontalScaleValue = HorizontalScale_2;
                            TimeScaleValue = TimeScale_2;
                            IndexOffset = HorizontalScale_2 + HorizontalFastShift_2;
                            break;
                        case 2:
                            VoltageScaleValue = VScaleChannel_3;
                            VerticalScaleValue = VerticalScale_3;
                            HorizontalScaleValue = HorizontalScale_3;
                            TimeScaleValue = TimeScale_3;
                            IndexOffset = HorizontalScale_3 + HorizontalFastShift_3;
                            break;
                        case 3:
                            VoltageScaleValue = VScaleChannel_4;
                            VerticalScaleValue = VerticalScale_4;
                            HorizontalScaleValue = HorizontalScale_4;
                            TimeScaleValue = TimeScale_4;
                            IndexOffset = HorizontalScale_4 + HorizontalFastShift_4;
                            break;
                        case 4:
                            VoltageScaleValue = VScaleChannel_5;
                            VerticalScaleValue = VerticalScale_5;
                            HorizontalScaleValue = HorizontalScale_5;
                            TimeScaleValue = TimeScale_5;
                            IndexOffset = HorizontalScale_5 + HorizontalFastShift_5;
                            break;
                        case 5:
                            VoltageScaleValue = VScaleChannel_6;
                            VerticalScaleValue = VerticalScale_6;
                            HorizontalScaleValue = HorizontalScale_6;
                            TimeScaleValue = TimeScale_6;
                            IndexOffset = HorizontalScale_6 + HorizontalFastShift_6;
                            break;
                        case 6:
                            VoltageScaleValue = VScaleChannel_7;
                            VerticalScaleValue = VerticalScale_7;
                            HorizontalScaleValue = HorizontalScale_7;
                            TimeScaleValue = TimeScale_7;
                            IndexOffset = HorizontalScale_7 + HorizontalFastShift_7;
                            break;
                        case 7:
                            VoltageScaleValue = VScaleChannel_8;
                            VerticalScaleValue = VerticalScale_8;
                            HorizontalScaleValue = HorizontalScale_8;
                            TimeScaleValue = TimeScale_8;
                            IndexOffset = HorizontalScale_8 + HorizontalFastShift_8;
                            break;
                        default:
                            break;
                    }


                    ScopeIndex = GetScopeBufferIndex(OuterDimension, IndexOffset);
                    //if (ScopeBuffer[OuterDimension] != null)
                    if (ActiveChannels[OuterDimension])
                    {
                        // read each value from the buffer and plot the sample on the scope		
                        for (int xpos = 0; xpos < xmax; xpos++)
                        {
                            if (StorageOn)
                                n = StorageArray[xpos];
                            else
                                StorageArray[xpos] = n;

                            // n between 0 and 1023
                            //System.Single y = 0;
                            //System.Single volt = 0;
                            int yint = 0;


                            //Scale the data words to 16 bits
                            double Data;
                            Data = (double)((n * 450) / BitScale);

                            n = (int)Data;
                            ymax = 256;

                            // scale the voltage according to the voltage scale control

                            //if (SaveDataToFile)
                            //    writer.Write("Channel {0} Pre-Calc Value {1:0} \n",OuterDimension, n);

                            if (VoltageScaleValue >= 5)
                            {
                                yint = (int)(n / (VoltageScaleValue - 4));
                            }
                            else
                            {
                                yint = (int)(n * (6 - VoltageScaleValue));
                            }

                            // offset the voltage based on the y position dial
                            yint -= -(VerticalScaleValue);

                            // place the y reading into the correct coordinate system
                            //    by subtracting from the maximum value
                            yDraw = ymax - yint;

                            // don't draw the first sample point, use it as
                            // a starting sample point
                            if (xlast != -1)
                            {
                                // scale the x-value based on the TimeScale control
                                // translate the x-position by the x-position control
                                //xDraw = xpos;
                                //xDraw = xpos * (int)this.TimeScaleControl.Value + ((int)trkTimeScaleControl.Value * (int)TimeScaleControl.Value * 45);
                                //xDraw = xpos * (int)this.trkTimeScale.Value + (HorizontalScaleValue * (int)trkTimeScale.Value * 45);
                                //xDraw = xpos * TimeScaleValue + (HorizontalScaleValue * TimeScaleValue * 5);
                                if (TimeScaleValue == 1)
                                    xDraw = xpos * TimeScaleValue;
                                else if (TimeScaleValue > 1)
                                    xDraw = xpos * (TimeScaleValue * 2);
                                else 
                                    xDraw = xpos;

                                // Draw the scaled sample point by connecting
                                // the previous scaled sample point to the current
                                // scaled sample point
                                if (xDraw < kScopeWidth)
                                {
                                    /*if ((nToggleColor % 2 == 0) || (ShowSamplesOn == false))
                                        g.DrawLine(Pens.LawnGreen, xlast, ylast, xDraw, yDraw);
                                    else
                                        g.DrawLine(Pens.Red, xlast, ylast, xDraw, yint);
                                    */
                                    switch (OuterDimension)
                                    {
                                        case 0:
                                            g.DrawLine(Pens.LawnGreen, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 1:
                                            g.DrawLine(Pens.Red, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 2:
                                            g.DrawLine(Pens.Blue, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 3:
                                            g.DrawLine(Pens.Orange, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 4:
                                            g.DrawLine(Pens.Red, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 5:
                                            g.DrawLine(Pens.DarkOliveGreen, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 6:
                                            g.DrawLine(Pens.DimGray, xlast, ylast, xDraw, yDraw);
                                            break;
                                        case 7:
                                            g.DrawLine(Pens.LightCyan, xlast, ylast, xDraw, yDraw);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }

                            // remember the current sample point to allow for connecting
                            // the next sample point
                            xlast = xDraw;
                            ylast = yDraw;

                            nToggleColor++;

                            // read the next a/d value from the buffer
                            if (xpos <= xmax)
                            {
                                /*
                                if (ScopeIndex < 49999)
                                    n = ScopeBuffer[OuterDimension][ScopeIndex++];
                                else
                                {
                                    ScopeIndex = 0;
                                    n = ScopeBuffer[OuterDimension][ScopeIndex++];
                                }
                                */

                                if (ScopeIndex > 49998)
                                    ScopeIndex = 0;

                                if (TimeScaleValue < 0)
                                {
                                    if (ScopeIndex + (TimeScaleIn * Math.Abs(TimeScaleValue)) < 49999)
                                        n = ScopeBuffer[OuterDimension][ScopeIndex++ + (TimeScaleIn++ * Math.Abs(TimeScaleValue))];
                                    else
                                        n = ScopeBuffer[OuterDimension][(TimeScaleIn++ * Math.Abs(TimeScaleValue))];
                                }
                                else
                                    n = ScopeBuffer[OuterDimension][ScopeIndex++];
                            }
                        }
                    }
                }
                //RefreshBusy is reset to allow the samples to start filling
                //the ScopeBuffer
                RefreshBusy = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("RefreshScope Error:" + ex);
            }
        }

        const int kScopeWidth = 450;
        const int kScopeHeight = 450;
        Rectangle scopeRect = new Rectangle(0, 0, kScopeWidth, kScopeHeight);

        private void DrawGrid(Graphics g)
        {
            // Draw the Whole screen black
            g.FillRectangle(Brushes.Black, scopeRect);

            const int kpadding = 0;
            const int tickheight = 4;
            const int tickheightLarge = 8;
            // draw the Grid Lines
            for (int i = 0; i < 450; i += 50)
            {
                // draw horizontal line
                g.DrawLine(Pens.DarkGray, 0, i + kpadding, kScopeWidth, i + kpadding);

                // draw vertical line
                g.DrawLine(Pens.DarkGray, i + kpadding, 0, i + kpadding, kScopeHeight);

                // draw ticks on each line
                if (i == 200)
                {
                    for (int j = 0; j < kScopeWidth; j += 5)
                    {
                        if (j % 25 != 0)
                        {
                            g.DrawLine(Pens.White, j, (i - tickheight / 2),
                                j, (i + tickheight / 2));

                            g.DrawLine(Pens.White, (i - tickheight / 2), j,
                                (i + tickheight / 2), j);
                        }
                        else
                        {
                            g.DrawLine(Pens.White, j, (i - tickheightLarge / 2) + kpadding,
                                j, (i + tickheightLarge / 2) + kpadding);

                            g.DrawLine(Pens.White, (i - tickheightLarge / 2), j,
                                (i + tickheightLarge / 2), j);
                        }
                    }
                }
            }


            // Draw Scope from a to d
            //if (ScopeOn)
                this.RefreshScope(g);
        }


        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            try
            {
                // Draw the scope Grid
                DrawGrid(g);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Form1_Paint Error:" + ex);
            }

        }

        #region Transfer Data
        private void EPTParseReceive()
        {
            switch (EPTReceiveData.Command)
            {
                case TRANSFER_OUT_COMMAND:
                    TransferOutReceive();
                    break;
                default:
                    break;
            }
        }


        public void TransferOutReceive()
        {

            //Store the address history from the EPT Receive Object
            SecondPreviousActualAddr = FirstPreviousActualAddr;
            FirstPreviousActualAddr = EPTReceiveData.Address;
            //Parse the data from the Active Transfer Library
            //based on the address of the Active EndTerm
            switch (EPTReceiveData.Address)
            {
                case TRANSFER_OUT_ADDRESS_1:
                    //Determine if this is the upper of lower byte
                    if (FirstDisplayByte)
                    {
                        //Shift the byte into the MSB location and set the 
                        //FirstDisplayByte flag to false and save AnalogValue.
                        AnalogValue = 0;
                        FirstDisplayByte = false;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                    }
                    else if (!FirstDisplayByte)
                    {
                        //Add the byte to AnalogValue in the LSB. Set FirstDisplayByte
                        // to true for the next channel scanned.
                        //string WriteRcvChar = "";
                        int WriteRcvChar = 0;
                        FirstDisplayByte = true;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);

                        //Shift AnalogValue to the right two bits to remove the extra bits
                        AnalogValue = AnalogValue >> 2;

                        WriteRcvChar = AnalogValue;

                        //Display the WriteRcvChar 
                        Thread Display1Thread = new Thread(new ParameterizedThreadStart(DisplayValue1));
                        Display1Thread.Start(WriteRcvChar);
                    }
                    break;

                case TRANSFER_OUT_ADDRESS_2:
                    //Determine if this is the upper of lower byte
                    if (FirstDisplayByte)
                    {
                        //Shift the byte into the MSB location and set the 
                        //FirstDisplayByte flag to false and save AnalogValue.
                        AnalogValue = 0;
                        FirstDisplayByte = false;
                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                    }
                    else if (!FirstDisplayByte)
                    {
                        //Add the byte to AnalogValue in the LSB. Set FirstDisplayByte
                        // to true for the next channel scanned.
                        int WriteRcvChar = 0;
                        FirstDisplayByte = true;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);

                        //Shift AnalogValue to the right two bits to remove the extra bits
                        AnalogValue = AnalogValue >> 2;

                        WriteRcvChar = AnalogValue;

                        Thread Display2Thread = new Thread(new ParameterizedThreadStart(DisplayValue2));
                        Display2Thread.Start(WriteRcvChar);
                    }
                    break;
                case TRANSFER_OUT_ADDRESS_3:
                    //Determine if this is the upper of lower byte
                    if (FirstDisplayByte)
                    {
                        //Shift the byte into the MSB location and set the 
                        //FirstDisplayByte flag to false and save AnalogValue.
                        AnalogValue = 0;
                        FirstDisplayByte = false;
                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                    }
                    else if (!FirstDisplayByte)
                    {
                        //Add the byte to AnalogValue in the LSB. Set FirstDisplayByte
                        // to true for the next channel scanned.
                        int WriteRcvChar = 0;
                        FirstDisplayByte = true;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);

                        //Shift AnalogValue to the right two bits to remove the extra bits
                        AnalogValue = AnalogValue >> 2;

                        WriteRcvChar = AnalogValue;

                        Thread Display3Thread = new Thread(new ParameterizedThreadStart(DisplayValue3));
                        Display3Thread.Start(WriteRcvChar);
                    }
                    break;
                case TRANSFER_OUT_ADDRESS_4:
                    //Determine if this is the upper of lower byte
                    if (FirstDisplayByte)
                    {
                        //Shift the byte into the MSB location and set the 
                        //FirstDisplayByte flag to false and save AnalogValue.
                        AnalogValue = 0;
                        FirstDisplayByte = false;
                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                    }
                    else if (!FirstDisplayByte)
                    {
                        //Add the byte to AnalogValue in the LSB. Set FirstDisplayByte
                        // to true for the next channel scanned.
                        int WriteRcvChar = 0;
                        FirstDisplayByte = true;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);

                        //Shift AnalogValue to the right two bits to remove the extra bits
                        AnalogValue = AnalogValue >> 2;

                        WriteRcvChar = AnalogValue;

                        Thread Display4Thread = new Thread(new ParameterizedThreadStart(DisplayValue4));
                        Display4Thread.Start(WriteRcvChar);
                    }
                    break;
                case TRANSFER_OUT_ADDRESS_5:
                    //Determine if this is the upper of lower byte
                    if (FirstDisplayByte)
                    {
                        //Shift the byte into the MSB location and set the 
                        //FirstDisplayByte flag to false and save AnalogValue.
                        AnalogValue = 0;
                        FirstDisplayByte = false;
                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                    }
                    else if (!FirstDisplayByte)
                    {
                        //Add the byte to AnalogValue in the LSB. Set FirstDisplayByte
                        // to true for the next channel scanned.
                        int WriteRcvChar = 0;
                        FirstDisplayByte = true;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);

                        //Shift AnalogValue to the right two bits to remove the extra bits
                        AnalogValue = AnalogValue >> 2;

                        WriteRcvChar = AnalogValue;

                        Thread Display5Thread = new Thread(new ParameterizedThreadStart(DisplayValue5));
                        Display5Thread.Start(WriteRcvChar);
                    }
                    break;
                case TRANSFER_OUT_ADDRESS_6:
                    //Determine if this is the upper of lower byte
                    if (FirstDisplayByte)
                    {
                        //Shift the byte into the MSB location and set the 
                        //FirstDisplayByte flag to false and save AnalogValue.
                        AnalogValue = 0;
                        FirstDisplayByte = false;
                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                    }
                    else if (!FirstDisplayByte)
                    {
                        //Add the byte to AnalogValue in the LSB. Set FirstDisplayByte
                        // to true for the next channel scanned.
                        int WriteRcvChar = 0;
                        FirstDisplayByte = true;

                        AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0]);
                        //AnalogValue = AnalogValue | ((int)EPTReceiveData.cBlockBuf[0] << 8);

                        //Shift AnalogValue to the right two bits to remove the extra bits
                        AnalogValue = AnalogValue >> 2;

                        WriteRcvChar = AnalogValue;

                        Thread Display6Thread = new Thread(new ParameterizedThreadStart(DisplayValue6));
                        Display6Thread.Start(WriteRcvChar);
                    }
                    break;
                default:
                    break;
            }
        }

        #region Display Value/buffer control
        public void DisplayValue1(object WriteRcvChar)
        {
            //this.Invoke(new MethodInvoker(delegate() { tbMonitor1.Text = (string)WriteRcvChar; }));
            ScopeBufferChannelSelect = 0;
            ScopeBuffer[ScopeBufferChannelSelect][IncrEPTReceiveIndex(ScopeBufferChannelSelect)] = (int)WriteRcvChar;

        }

        public void DisplayValue2(object WriteRcvChar)
        {
            ScopeBufferChannelSelect = 1;
            if (ScopeBuffer[ScopeBufferChannelSelect] == null)
            {
                ActiveChannels[ScopeBufferChannelSelect] = true;
                ScopeBuffer[ScopeBufferChannelSelect] = new int[50000];
                this.Invoke(new MethodInvoker(delegate() { cmbChannelSelect.Items.Add((ScopeBufferChannelSelect + 1)); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_2_VerticalLabel = new Label(); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_2_VerticalLabel.ForeColor = Color.Red; }));
                this.Invoke(new MethodInvoker(delegate() { Channel_2_VerticalLabel.Location = new Point(450, 250); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_2_VerticalLabel.Font = new Font("Arial", 8); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_2_VerticalLabel.Text = "--Ch 2"; }));
                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(Channel_2_VerticalLabel); }));
            }
            ScopeBuffer[ScopeBufferChannelSelect][IncrEPTReceiveIndex(ScopeBufferChannelSelect)] = (int)WriteRcvChar;
        }

        public void DisplayValue3(object WriteRcvChar)
        {
            ScopeBufferChannelSelect = 2;
            if (ScopeBuffer[ScopeBufferChannelSelect] == null)
            {
                ActiveChannels[ScopeBufferChannelSelect] = true;
                ScopeBuffer[ScopeBufferChannelSelect] = new int[50000];
                this.Invoke(new MethodInvoker(delegate() { cmbChannelSelect.Items.Add((ScopeBufferChannelSelect + 1)); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_3_VerticalLabel = new Label(); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_3_VerticalLabel.ForeColor = Color.Blue; }));
                this.Invoke(new MethodInvoker(delegate() { Channel_3_VerticalLabel.Location = new Point(450, 250); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_3_VerticalLabel.Font = new Font("Arial", 8); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_3_VerticalLabel.Text = "--Ch 3"; }));
                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(Channel_3_VerticalLabel); }));
            }
            ScopeBuffer[ScopeBufferChannelSelect][IncrEPTReceiveIndex(ScopeBufferChannelSelect)] = (int)WriteRcvChar;
        }

        public void DisplayValue4(object WriteRcvChar)
        {
            ScopeBufferChannelSelect = 3;
            if (ScopeBuffer[ScopeBufferChannelSelect] == null)
            {
                ActiveChannels[ScopeBufferChannelSelect] = true;
                ScopeBuffer[ScopeBufferChannelSelect] = new int[50000];
                this.Invoke(new MethodInvoker(delegate() { cmbChannelSelect.Items.Add((ScopeBufferChannelSelect + 1)); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_4_VerticalLabel = new Label(); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_4_VerticalLabel.ForeColor = Color.Orange; }));
                this.Invoke(new MethodInvoker(delegate() { Channel_4_VerticalLabel.Location = new Point(450, 250); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_4_VerticalLabel.Font = new Font("Arial", 8); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_4_VerticalLabel.Text = "--Ch 4"; }));
                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(Channel_4_VerticalLabel); }));
            }
            ScopeBuffer[ScopeBufferChannelSelect][IncrEPTReceiveIndex(ScopeBufferChannelSelect)] = (int)WriteRcvChar;
        }

        public void DisplayValue5(object WriteRcvChar)
        {
            ScopeBufferChannelSelect = 4;
            if (ScopeBuffer[ScopeBufferChannelSelect] == null)
            {
                ActiveChannels[ScopeBufferChannelSelect] = true;
                ScopeBuffer[ScopeBufferChannelSelect] = new int[50000];
                this.Invoke(new MethodInvoker(delegate() { cmbChannelSelect.Items.Add((ScopeBufferChannelSelect + 1)); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_5_VerticalLabel = new Label(); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_5_VerticalLabel.ForeColor = Color.Black; }));
                this.Invoke(new MethodInvoker(delegate() { Channel_5_VerticalLabel.Location = new Point(450, 250); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_5_VerticalLabel.Font = new Font("Arial", 8); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_5_VerticalLabel.Text = "--Ch 5"; }));
                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(Channel_5_VerticalLabel); }));
            }
            ScopeBuffer[ScopeBufferChannelSelect][IncrEPTReceiveIndex(ScopeBufferChannelSelect)] = (int)WriteRcvChar;
        }

        public void DisplayValue6(object WriteRcvChar)
        {
            ScopeBufferChannelSelect = 5;
            if (ScopeBuffer[ScopeBufferChannelSelect] == null)
            {
                ActiveChannels[ScopeBufferChannelSelect] = true;
                ScopeBuffer[ScopeBufferChannelSelect] = new int[50000];
                this.Invoke(new MethodInvoker(delegate() { cmbChannelSelect.Items.Add((ScopeBufferChannelSelect + 1)); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_6_VerticalLabel = new Label(); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_6_VerticalLabel.ForeColor = Color.DarkOliveGreen; }));
                this.Invoke(new MethodInvoker(delegate() { Channel_6_VerticalLabel.Location = new Point(450, 250); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_6_VerticalLabel.Font = new Font("Arial", 8); }));
                this.Invoke(new MethodInvoker(delegate() { Channel_6_VerticalLabel.Text = "--Ch 6"; }));
                this.Invoke(new MethodInvoker(delegate() { this.Controls.Add(Channel_6_VerticalLabel); }));
            }
            ScopeBuffer[ScopeBufferChannelSelect][IncrEPTReceiveIndex(ScopeBufferChannelSelect)] = (int)WriteRcvChar;
        }
        #endregion


        private void btnStart_Click(object sender, EventArgs e)
        {
            ScopeOn = true;

            btnStart.Enabled = false;
            btnStop.Enabled = true;

            SetupRegister();

            Thread.Sleep(1);

            EPT_AH_SendTransferControlByte((char)2, (char)4);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //timer1.Stop();
            ScopeOn = false;

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            EPT_AH_SendTransferControlByte((char)2, (char)0);

            Invalidate();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetADCFifo();

            Thread.Sleep(1);

            EPT_AH_SendTransferControlByte((char)2, (char)4);

            Thread.Sleep(1);

            EPT_AH_SendTransferControlByte((char)2, (char)0);
        }

        private void btnConvRegister_Click(object sender, EventArgs e)
        {

        }

        private void btnSetupRegister_Click(object sender, EventArgs e)
        {
            SetupRegister();
        }

        public void SetupRegister()
        {
            ResetADCAll();

            EPT_AH_SendByte((char)2, (byte)0x48);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);

        }

        private void btnAverageRegister_Click(object sender, EventArgs e)
        {

        }

        private void btnResetRegister_Click(object sender, EventArgs e)
        {
            ResetADCAll();
        }

        public void ResetADCAll()
        {
            EPT_AH_SendByte((char)2, (byte)0x10);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);

        }

        public void ResetADCFifo()
        {
            EPT_AH_SendByte((char)2, (byte)0x18);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);


        }

        private void btnScanChannel_1_Click(object sender, EventArgs e)
        {

            EPT_AH_SendByte((char)2, (byte)0x80);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);

        }

        private void btnScanChannels_1_2_Click(object sender, EventArgs e)
        {
            EPT_AH_SendByte((char)2, (byte)0x88);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);

        }

        private void btnScanChannels_1_2_3_Click(object sender, EventArgs e)
        {
            EPT_AH_SendByte((char)2, (byte)0x90);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);

        }

        private void btnScanChannels_1_2_3_4_Click(object sender, EventArgs e)
        {
            EPT_AH_SendByte((char)2, (byte)0x98);

            EPT_AH_SendTrigger((byte)2);

            Thread.Sleep(1);

            EPT_AH_SendTrigger((byte)1);

        }

        #endregion
        //private void groupBox3_Enter(object sender, EventArgs e)
        //{

        //}

        private void btnCursor1_Click(object sender, EventArgs e)
        {

        }

        private void lblVoltage_P1_Click(object sender, EventArgs e)
        {

        }



        }

        public class Transfer
        {
            public int Command;
            public int Address;
            public int Length;
            public int Payload;
            public byte[] cBlockBuf;
            public bool TransferPending;
            public uint Repititions;

            public Transfer()
            {
                Command = 0;
                Address = 0;
                Length = 0;
                Payload = 0;
                TransferPending = false;
                Repititions = 0;
            }

        }


}
