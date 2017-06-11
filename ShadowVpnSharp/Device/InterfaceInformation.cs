using System.Text;

namespace ShadowVpnSharp.Device {
    internal class InterfaceInformation {
        #region System.Net.NetworkInterface

        public bool Net_NetworkInterface_Exist = false;
        public string Net_ID = "";
        public string Net_Description = "";
        public string Net_Name = "";
        public string Net_OperationalStatus = "";
        public bool Net_SupportIPv4 = false;
        public string Net_NetworkInterfaceType = "";
        public int Net_IPv4Index = 0;
        public int Net_IPv4MTU = 0;
        public string Net_DnsAddresses = "";
        public string Net_GatewayAddresses = "";

        #endregion

        #region WMI_Win32_NetworkAdapterConfiguration 

        public bool WMI_Win32_NetworkAdapterConfiguration_Exist = false;
        public string WMI_NAC_Caption = "";
        public string WMI_NAC_Description = "";
        public string WMI_NAC_DefaultIPGateway = "";
        public uint WMI_NAC_Index = 0;
        public uint WMI_NAC_InterfaceIndex = 0;
        public string WMI_NAC_DNSServerSearchOrder = "";
        public bool WMI_NAC_DHCPEnabled = false;
        public bool WMI_NAC_IPEnabled = false;
        public uint WMI_NAC_IPConnectionMetric = 0;
        public string WMI_NAC_IPAddress = "";
        public uint WMI_NAC_MTU = 0;
        public string WMI_NAC_MACAddress = "";

        #endregion

        #region WMI_Win32_NetworkAdapter ON WIN7 or lower

        public bool WMI_Win32_NetworkAdapter_Exist = false;
        public string WMI_WNA_AdapterType = "";
        public ushort WMI_WNA_Availability = 0;
        public string WMI_WNA_Caption = "";
        public string WMI_WNA_Description = "";
        public string WMI_WNA_DeviceID = "";
        public string WMI_WNA_GUID = "";
        public uint WMI_WNA_Index = 0;
        public uint WMI_WNA_InterfaceIndex = 0;
        public string WMI_WNA_Name = "";
        public string WMI_WNA_NetConnectionID = "";
        public ushort WMI_WNA_NetConnectionStatus = 0;
        public bool WMI_WNA_PhysicalAdapter = false;
        public string WMI_WNA_Status = "";

        #endregion

        #region WMI_MSFT_NetAdapter ON Win8 or newer

        public bool WMI_MSFT_NetAdapter_Exist = false;
        public string WMI_MSFT_NA_Caption = "";
        public string WMI_MSFT_NA_Description = "";
        public string WMI_MSFT_NA_Name = "";
        public string WMI_MSFT_NA_Status = "";
        public ushort WMI_MSFT_NA_Availability = 0;
        public string WMI_MSFT_NA_DeviceID = "";
        public string WMI_MSFT_NA_PNPDeviceID = "";
        public string WMI_MSFT_NA_InterfaceDescription = "";
        public string WMI_MSFT_NA_InterfaceName = "";
        public ulong WMI_MSFT_NA_NetLuid = 0;
        public string WMI_MSFT_NA_InterfaceGuid = "";
        public uint WMI_MSFT_NA_InterfaceIndex = 0;
        public string WMI_MSFT_NA_DeviceName = "";
        public uint WMI_MSFT_NA_MediaConnectState = 0;
        public bool WMI_MSFT_NA_ConnectorPresent = false;
        public bool WMI_MSFT_NA_Virtual = false;
        public bool WMI_MSFT_NA_HardwareInterface = false;
        public uint WMI_MSFT_NA_MtuSize = 0;

        #endregion

        #region IPHLPAPI_IFINTERFACE_ROW

        public bool IPHLPAPI_IFINTERFACE_ROW_Exist = false;
        public ulong IP_IFR_InterfaceLuid = 0;
        public uint IP_IFR_InterfaceIndex = 0;
        public bool IP_IFR_UseAutomaticMetric = false;
        public uint IP_IFR_Metric = 0;
        public uint IP_IFR_NlMtu = 0;
        public bool IP_IFR_Connected = false;
        public bool IP_IFR_DisableDefaultRoutes = false;

        #endregion

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("SECTION: System.Net.NetworkInterface");
            if (Net_NetworkInterface_Exist == false) {
                sb.AppendLine(": Not Exist");
            } else {
                sb.AppendLine();
                sb.AppendLine($"\tNet_ID..................: {Net_ID}");
                sb.AppendLine($"\tNet_Description.........: {Net_Description}");
                sb.AppendLine($"\tNet_Name................: {Net_Name}");
                sb.AppendLine($"\tNet_OperationalStatus...: {Net_OperationalStatus}");
                sb.AppendLine($"\tNet_SupportIPv4.........: {Net_SupportIPv4}");
                sb.AppendLine($"\tNet_NetworkInterfaceType: {Net_NetworkInterfaceType}");
                sb.AppendLine($"\tNet_DnsAddresses........: {Net_DnsAddresses}");
                sb.AppendLine($"\tNet_GatewayAddresses....: {Net_GatewayAddresses}");
                if (Net_SupportIPv4) {
                    sb.AppendLine($"\tNet_IPv4Index...........: {Net_IPv4Index}");
                    sb.AppendLine($"\tNet_IPv4MTU.............: {Net_IPv4MTU}");
                }
            }

            sb.Append("SECTION: WMI_Win32_NetworkAdapterConfiguration");
            if (WMI_Win32_NetworkAdapterConfiguration_Exist == false) {
                sb.AppendLine(": Not Exist");
            } else {
                sb.AppendLine();
                sb.AppendLine($"\tWMI_NAC_Caption.............: {WMI_NAC_Caption}");
                sb.AppendLine($"\tWMI_NAC_Description.........: {WMI_NAC_Description}");
                sb.AppendLine($"\tWMI_NAC_DefaultIPGateway....: {WMI_NAC_DefaultIPGateway}");
                sb.AppendLine($"\tWMI_NAC_Index...............: {WMI_NAC_Index}");
                sb.AppendLine($"\tWMI_NAC_InterfaceIndex......: {WMI_NAC_InterfaceIndex}");
                sb.AppendLine($"\tWMI_NAC_DNSServerSearchOrder: {WMI_NAC_DNSServerSearchOrder}");
                sb.AppendLine($"\tWMI_NAC_DHCPEnabled.........: {WMI_NAC_DHCPEnabled}");
                sb.AppendLine($"\tWMI_NAC_IPEnabled...........: {WMI_NAC_IPEnabled}");
                sb.AppendLine($"\tWMI_NAC_IPConnectionMetric..: {WMI_NAC_IPConnectionMetric}");
                sb.AppendLine($"\tWMI_NAC_IPAddress...........: {WMI_NAC_IPAddress}");
                sb.AppendLine($"\tWMI_NAC_MTU.................: {WMI_NAC_MTU}");
                sb.AppendLine($"\tWMI_NAC_MACAddress..........: {WMI_NAC_MACAddress}");
            }

            sb.Append("SECTION: WMI_Win32_NetworkAdapter");
            if (WMI_Win32_NetworkAdapter_Exist == false) {
                sb.AppendLine(": Not Exist");
            } else {
                sb.AppendLine();
                sb.AppendLine($"\tWMI_WNA_AdapterType........: {WMI_WNA_AdapterType}");
                sb.AppendLine($"\tWMI_WNA_Availability.......: {WMI_WNA_Availability}");
                sb.AppendLine($"\tWMI_WNA_Caption............: {WMI_WNA_Caption}");
                sb.AppendLine($"\tWMI_WNA_Description........: {WMI_WNA_Description}");
                sb.AppendLine($"\tWMI_WNA_DeviceID...........: {WMI_WNA_DeviceID}");
                sb.AppendLine($"\tWMI_WNA_GUID...............: {WMI_WNA_GUID}");
                sb.AppendLine($"\tWMI_WNA_Index..............: {WMI_WNA_Index}");
                sb.AppendLine($"\tWMI_WNA_InterfaceIndex.....: {WMI_WNA_InterfaceIndex}");
                sb.AppendLine($"\tWMI_WNA_Name...............: {WMI_WNA_Name}");
                sb.AppendLine($"\tWMI_WNA_NetConnectionID....: {WMI_WNA_NetConnectionID}");
                sb.AppendLine($"\tWMI_WNA_NetConnectionStatus: {WMI_WNA_NetConnectionStatus}");
                sb.AppendLine($"\tWMI_WNA_PhysicalAdapter....: {WMI_WNA_PhysicalAdapter}");
                sb.AppendLine($"\tWMI_WNA_Status.............: {WMI_WNA_Status}");
            }

            sb.Append("SECTION: WMI_MSFT_NetAdapter");
            if (WMI_MSFT_NetAdapter_Exist == false) {
                sb.AppendLine(": Not Exist");
            } else {
                sb.AppendLine();
                sb.AppendLine($"\tWMI_MSFT_NA_Caption.............: {WMI_MSFT_NA_Caption}");
                sb.AppendLine($"\tWMI_MSFT_NA_Description.........: {WMI_MSFT_NA_Description}");
                sb.AppendLine($"\tWMI_MSFT_NA_Name................: {WMI_MSFT_NA_Name}");
                sb.AppendLine($"\tWMI_MSFT_NA_Status..............: {WMI_MSFT_NA_Status}");
                sb.AppendLine($"\tWMI_MSFT_NA_Availability........: {WMI_MSFT_NA_Availability}");
                sb.AppendLine($"\tWMI_MSFT_NA_DeviceID............: {WMI_MSFT_NA_DeviceID}");
                sb.AppendLine($"\tWMI_MSFT_NA_PNPDeviceID.........: {WMI_MSFT_NA_PNPDeviceID}");
                sb.AppendLine($"\tWMI_MSFT_NA_InterfaceDescription: {WMI_MSFT_NA_InterfaceDescription}");
                sb.AppendLine($"\tWMI_MSFT_NA_InterfaceName.......: {WMI_MSFT_NA_InterfaceName}");
                sb.AppendLine($"\tWMI_MSFT_NA_NetLuid.............: {WMI_MSFT_NA_NetLuid}");
                sb.AppendLine($"\tWMI_MSFT_NA_InterfaceGuid.......: {WMI_MSFT_NA_InterfaceGuid}");
                sb.AppendLine($"\tWMI_MSFT_NA_InterfaceIndex......: {WMI_MSFT_NA_InterfaceIndex}");
                sb.AppendLine($"\tWMI_MSFT_NA_DeviceName..........: {WMI_MSFT_NA_DeviceName}");
                sb.AppendLine($"\tWMI_MSFT_NA_MediaConnectState...: {WMI_MSFT_NA_MediaConnectState}");
                sb.AppendLine($"\tWMI_MSFT_NA_ConnectorPresent....: {WMI_MSFT_NA_ConnectorPresent}");
                sb.AppendLine($"\tWMI_MSFT_NA_DeviceName..........: {WMI_MSFT_NA_DeviceName}");
                sb.AppendLine($"\tWMI_MSFT_NA_Virtual.............: {WMI_MSFT_NA_Virtual}");
                sb.AppendLine($"\tWMI_MSFT_NA_HardwareInterface...: {WMI_MSFT_NA_HardwareInterface}");
                sb.AppendLine($"\tWMI_MSFT_NA_MtuSize.............: {WMI_MSFT_NA_MtuSize}");
            }


            sb.Append("SECTION: IPHLPAPI_IFINTERFACE_ROW");
            if (IPHLPAPI_IFINTERFACE_ROW_Exist == false) {
                sb.AppendLine(": Not Exist");
            } else {
                sb.AppendLine();
                sb.AppendLine($"\tIP_IFR_InterfaceLuid.......: {IP_IFR_InterfaceLuid}");
                sb.AppendLine($"\tIP_IFR_InterfaceIndex......: {IP_IFR_InterfaceIndex}");
                sb.AppendLine($"\tIP_IFR_UseAutomaticMetric..: {IP_IFR_UseAutomaticMetric}");
                sb.AppendLine($"\tIP_IFR_Metric..............: {IP_IFR_Metric}");
                sb.AppendLine($"\tIP_IFR_NlMtu...............: {IP_IFR_NlMtu}");
                sb.AppendLine($"\tIP_IFR_Connected...........: {IP_IFR_Connected}");
                sb.AppendLine($"\tIP_IFR_DisableDefaultRoutes: {IP_IFR_DisableDefaultRoutes}");
            }

            return sb.ToString();
        }
    }
}
