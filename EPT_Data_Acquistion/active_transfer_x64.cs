using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace UnoProLyzer 
{
    public partial class UnoProLyzer 
    {
        const string activehost_dll = "ActiveHost64.dll";
        [DllImport(activehost_dll)]
        static extern char EPT_AH_GetName();
        [DllImport(activehost_dll)]
        static extern char EPT_AH_GetVersionString();
        [DllImport(activehost_dll)]
        static extern unsafe void EPT_AH_GetVersionControl(short* v_major, short* v_minor, short* v_revision, short* v_debug);
        [DllImport(activehost_dll)]
        static extern unsafe char EPT_AH_GetInterfaceVersion();
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_CheckCompatibility(char* version, char* interface_version);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_Open(void* in_display_function, void* in_progress_bar_range_function, void* in_progress_bar_value_function);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_Close();
        [DllImport(activehost_dll)]
        static extern unsafe bool EPT_AH_Initialize();
        [DllImport(activehost_dll)]
        static extern unsafe void EPT_AH_Release();
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_QueryDevices();
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SelectActiveDeviceByName(char* device_name);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SelectActiveDeviceByIndex(Int32 device_index);
        [DllImport(activehost_dll)]
        static extern unsafe char* EPT_AH_GetDeviceName(int device_index);
        [DllImport(activehost_dll)]
        static extern unsafe char* EPT_AH_GetDeviceSerial(Int32 device_index);
        [DllImport(activehost_dll)]
        static extern unsafe int EPT_AH_OpenDeviceByIndex(Int32 device_index);
        [DllImport(activehost_dll)]
        static extern unsafe int EPT_AH_OpenDeviceByName(char* name);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_CloseDeviceByIndex(int device_index);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_CloseDeviceByName(char* name);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SendTrigger(byte trigger_value);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SendByte(Int32 device_channel, byte data_byte);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SendBlock(Int32 device_channel, void* data, UInt32 data_size);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SendTransferControlByte(char address_to_device, char payload);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_RegisterReadCallback(IntPtr read_callback);
        [DllImport(activehost_dll)]
        static extern unsafe char* EPT_AH_GetLastError();
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_PerformSelfTest();
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_LEDBlinky(Int32 milliseconds, Int32 count, byte* sequence, Int32 sequence_size);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_SetDebugMode(Int32 debug_mode);
        [DllImport(activehost_dll)]
        static extern unsafe Int32 EPT_AH_RegisterReadCallbackForChannel(IntPtr *read_callback, Int32 channel_index);
        [DllImport(activehost_dll)]
        static extern unsafe int EPT_AH_FlushDeviceChannelBuffer(Int32 device_index, Int32 channel_index);
        [DllImport(activehost_dll)]
        static extern unsafe UInt32 EPT_AH_GetDeviceChannelFreeBufferBytes(Int32 device_index, Int32 channel_index);
        [DllImport(activehost_dll)]
        static extern unsafe UInt32 EPT_AH_GetDeviceChannelPendingBufferBytes(Int32 device_index, Int32 channel_index);
        [DllImport(activehost_dll)]
        static extern unsafe bool EPT_AH_SetChannelConnectionFlag(Int32 device_index, Int32 channel_index, UInt32 flag, bool value);
        [DllImport(activehost_dll)]
        static extern unsafe bool EPT_AH_GetChannelConnectionFlag(Int32 device_index, Int32 channel_index, UInt32 flag);

        //Parameters
      public byte COMMAND_DECODE = 0x38;
      public const int AH_CS_UNRELIABLE_DROP = 0;                       // Flag of dropped data
      public const int AH_CS_UNRELIABLE_FLUSH = 1;                      // Flag of dropped and flushed data

      //Start of Class Methods
      [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
      unsafe delegate void MyEPTReadFunction(Int32 device_id, Int32 device_channel, byte command, byte payload, byte* data, byte data_size);
      MyEPTReadFunction MyEPTReadFunctionPTR;

      
        // Actual callback function which will read messages coming from the EPT device
        unsafe void EPTReadFunction(Int32 device_id, Int32 device_channel, byte command, byte payload, byte* data, byte data_size)
        {
            byte* message = data;

            // Select current device
            EPT_AH_SelectActiveDeviceByIndex(device_id);

            //Add command and device_channel to the receive object
            EPTReceiveData.Command = ((command & COMMAND_DECODE) >> 3);
            EPTReceiveData.Address = device_channel;

            //Check if the command is Block Receive. If so,
            //use Marshalling to copy the buffer into the receive
            //object
            if (EPTReceiveData.Command == BLOCK_OUT_COMMAND)
            {
                EPTReceiveData.Length = data_size;
                EPTReceiveData.cBlockBuf = new Byte[data_size];

                Marshal.Copy(new IntPtr(message), EPTReceiveData.cBlockBuf, 0, data_size);
            }
            else
            {
               EPTReceiveData.Length = data_size;
               EPTReceiveData.Payload = payload;
               EPTReceiveData.cBlockBuf = new Byte[data_size];
               Marshal.Copy(new IntPtr(message), EPTReceiveData.cBlockBuf, 0, data_size);
            }
            EPTParseReceive();
        }

        // **************** START OF CALLBACK REGISTRATION CODE *****************
        // Register the callback function
        // Declare pointer type of the callback function
        unsafe Int32 RegisterCallBack()
        {
            // Function declaration pointer (includes parameters so C++ knows what to send this function)
            // Grab the C# pointer to the local function: EPTReadFunction
            MyEPTReadFunctionPTR = new MyEPTReadFunction(EPTReadFunction);
            // Grab the actual C++ translation of the pointer of the function:
            IntPtr FctPtr = Marshal.GetFunctionPointerForDelegate(MyEPTReadFunctionPTR);

            // Sent the C++ translation
            if (EPT_AH_RegisterReadCallback(FctPtr) == 0)
            {
                String message = "Error registering callback function: " + Marshal.PtrToStringAnsi((IntPtr)EPT_AH_GetLastError());
                MessageBox.Show(message);
                return 0;
            }

            // Return success
            return 1;
        }
        // **************** END OF CALLBACK REGISTRATION CODE *******************

        // Main connection function
        private unsafe Int32 ListDevices()
        {
            Int32 result;
            Int32 num_devices;
            Int32 iCurrentIndex;

            // Open the DLL
            result = EPT_AH_Open(null, null, null);
            if (result != 0)
            {
                MessageBox.Show("Could not attach to the ActiveHost library");
                return 0;
            }

            // Query connected devices
            num_devices = EPT_AH_QueryDevices();

            //Prepare the Combo box for population
            iCurrentIndex = cmbDevList.SelectedIndex;
            cmbDevList.Items.Clear();

            // Go through all available devices
            for (device_index = 0; device_index < num_devices; device_index++)
            {
                String str;
                str = Marshal.PtrToStringAnsi((IntPtr)EPT_AH_GetDeviceName(device_index));
                cmbDevList.Items.Add(str);
            }
            return 0;
        }

        // Open the device
        public unsafe Int32 OpenDevice()
        {
            device_index = (int)cmbDevList.SelectedIndex;
            if (EPT_AH_OpenDeviceByIndex(device_index) == 0)
            {
                String message = "Could not open device " + Marshal.PtrToStringAnsi((IntPtr)EPT_AH_GetDeviceName(device_index))
                                                          + ", "
                                                          + Marshal.PtrToStringAnsi((IntPtr)EPT_AH_GetDeviceSerial(device_index));
                MessageBox.Show(message);
                return 0;
            }

            // Make the opened device the active device
            if (EPT_AH_SelectActiveDeviceByIndex(device_index) == 0)
            {
                String message = "Error selecting device: %s " + Marshal.PtrToStringAnsi((IntPtr)EPT_AH_GetLastError());
                MessageBox.Show(message);
                return 0;
            }

            // Register the read callback function
            RegisterCallBack();
            SetButtonEnables();

            // Set channel 0 to be dropped when data is overflowed
            //EPT_AH_SetChannelConnectionFlag(device_index, 0, AH_CS_UNRELIABLE_DROP, true);
            // Set channel 1 to completely flush the buffer when data is overflowed
            //EPT_AH_SetChannelConnectionFlag(device_index, 0, AH_CS_UNRELIABLE_FLUSH, true);

            return 0;
        }


    }
}
