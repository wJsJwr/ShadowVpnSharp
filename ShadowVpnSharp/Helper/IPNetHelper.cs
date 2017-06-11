using System;
using System.Runtime.InteropServices;

/*********
 * 重要：
 * (C++)An int and a long are 32-bit values on 64-bit Windows operating systems. 
 * For programs that you plan to compile for 64-bit platforms, you should be careful not to assign pointers to 32-bit variables. 
 * Pointers are 64-bit on 64-bit platforms, and you will truncate the pointer value if you assign it to a 32-bit variable.
 * ********/

namespace ShadowVpnSharp.Helper {
    
    internal static class IPNetHelper {
        internal const ushort AF_INET = 2; // internetwork: UDP, TCP, etc.
        internal const ushort AF_INET6 = 23;

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int GetIpForwardTable(IntPtr /*PMIB_IPFORWARDTABLE*/ pIpForwardTable,
            ref int /*PULONG*/ pdwSize, bool bOrder);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int CreateIpForwardEntry(IntPtr /*PMIB_IPFORWARDROW*/ pRoute);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int SetIpForwardEntry(IntPtr /*PMIB_IPFORWARDROW*/ pRoute);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int DeleteIpForwardEntry(IntPtr /*PMIB_IPFORWARDROW*/ pRoute);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern uint GetIpInterfaceEntry(IntPtr /*PMIB_IPINTERFACE_ROW*/ pRoute);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern uint GetIpInterfaceTable(ushort /*ADDRESS_FAMILY*/ Family,
            ref IntPtr /*PMIB_IPINTERFACE_TABLE*/ Table);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern void FreeMibTable(IntPtr /*PVOID*/ Memory);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int GetBestInterface(UInt32 DestAddr, out UInt32 BestIfIndex);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int SetIpInterfaceEntry(IntPtr /* PMIB_IPINTERFACE_ROW */ Row);

        [DllImport("iphlpapi", CharSet = CharSet.Auto)]
        private static extern int InitializeIpInterfaceEntry(IntPtr /*PMIB_IPINTERFACE_ROW*/ Row);

        public static uint GetBestIntf(uint dest) {
            uint intf = 0;
            int result = GetBestInterface(dest, out intf);
            if (result == 0) {
                return intf;
            }
            Logger.Error($"GetBestInterface Error, code={result}, dest={dest}");
            return uint.MaxValue;
        }

        public static MIB_IPFORWARDROW[] GetCurrentForwardTable() {
            var fwdTable = IntPtr.Zero;
            var size = 0;
            var result = GetIpForwardTable(fwdTable, ref size, true);
            fwdTable = Marshal.AllocHGlobal(size);

            result = GetIpForwardTable(fwdTable, ref size, true);
            var fib = Marshal.PtrToStructure<MIB_IPFORWARDTABLE>(fwdTable);
            var rows = new MIB_IPFORWARDROW[fib.dwNumEntries];
            var p = fwdTable + Marshal.SizeOf(fib.dwNumEntries);
            for (var i = 0; i < fib.dwNumEntries; i++) {
                rows[i] = Marshal.PtrToStructure<MIB_IPFORWARDROW>(p);
                p += Marshal.SizeOf<MIB_IPFORWARDROW>();
            }
            Marshal.FreeHGlobal(fwdTable);
            return rows;
        }

        public static MIB_IPINTERFACE_ROW[] GetInterfaceTable() {
            var pit = Marshal.AllocHGlobal(Marshal.SizeOf<MIB_IPINTERFACE_TABLE>());
            var oldpit = pit;
            var result = GetIpInterfaceTable(AF_INET, ref pit);
            if (result == 0) {
                var iftable = Marshal.PtrToStructure<MIB_IPINTERFACE_TABLE>(pit);
                var rows = new MIB_IPINTERFACE_ROW[iftable.NumEntries];
                var ptr = pit + 8; //Marshal.SizeOf(iftable.NumEntries)
                for (var i = 0; i < iftable.NumEntries; i++) {
                    rows[i] = Marshal.PtrToStructure<MIB_IPINTERFACE_ROW>(ptr);
                    ptr += Marshal.SizeOf<MIB_IPINTERFACE_ROW>();
                }
                FreeMibTable(pit);
                try {
                    Marshal.FreeHGlobal(oldpit);
                } catch (Exception) {
                    // suppress
                }
                return rows;
            } else {
                Logger.Error($"GetIpInterfaceEntry Failed, error_code={result}");
                throw new ExternalException("GetInterfaceTable Failed");
            }
        }

        public static void CreateIPForwardEntry(uint destIPAddress, uint destMask, uint nextHopIPAddress, uint ifIndex,
            int metric, uint policy = 0, uint type = 4, uint proto = 3) {
            var ptr = GenerateIpForwardEntry(destIPAddress, destMask, nextHopIPAddress, ifIndex, metric, policy, type,
                proto);
            int result = CreateIpForwardEntry(ptr);
            if (result != 0) {
                Logger.Error(
                    $"CreateIPForwardEntry Error, code={result}, dest={destIPAddress}, mask={destMask}, hop={nextHopIPAddress}, intf={ifIndex}, metric={metric}");
            }
            Marshal.FreeHGlobal(ptr);
        }

        public static void DeleteIPForwardEntry(uint destIPAddress, uint destMask, uint nextHopIPAddress, uint ifIndex,
            uint policy = 0, uint type = 4, uint proto = 3) {
            var ptr = GenerateIpForwardEntry(destIPAddress, destMask, nextHopIPAddress, ifIndex, -1, policy, type, proto);
            int result = DeleteIpForwardEntry(ptr);
            if (result != 0) {
                Logger.Error(
                    $"DeleteIPForwardEntry Error, code={result}, dest={destIPAddress}, mask={destMask}, hop={nextHopIPAddress}, intf={ifIndex}");
            }
            Marshal.FreeHGlobal(ptr);
        }

        private static IntPtr GenerateIpForwardEntry(uint destIPAddress, uint destMask, uint nextHopIPAddress,
            uint ifIndex,
            int metric, uint policy, uint type, uint proto) {
            MIB_IPFORWARDROW row = new MIB_IPFORWARDROW {
                dwForwardDest = destIPAddress,
                dwForwardMask = destMask,
                dwForwardPolicy = policy,
                dwForwardNextHop = nextHopIPAddress,
                dwForwardIfIndex = ifIndex,
                dwForwardType = type,
                dwForwardProto = proto,
                dwForwardAge = 0,
                dwForwardNextHopAS = 0,
                dwForwardMetric1 = metric,
                dwForwardMetric2 = 0,
                dwForwardMetric3 = 0,
                dwForwardMetric4 = 0,
                dwForwardMetric5 = 0
            };
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MIB_IPFORWARDROW>());
            Marshal.StructureToPtr(row, ptr, false);
            return ptr;
        }

        public static MIB_IPINTERFACE_ROW InitInterface(uint intfIndex) {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MIB_IPINTERFACE_ROW>());
            InitializeIpInterfaceEntry(ptr);
            var nif = Marshal.PtrToStructure<MIB_IPINTERFACE_ROW>(ptr);
            nif.Family = AF_INET;
            nif.InterfaceIndex = intfIndex;
            Marshal.FreeHGlobal(ptr);
            return nif;
        }

        public static MIB_IPINTERFACE_ROW GetInterface(uint intfIndex) {
            MIB_IPINTERFACE_ROW nif = new MIB_IPINTERFACE_ROW();
            nif.Family = AF_INET;
            nif.InterfaceIndex = intfIndex;

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MIB_IPINTERFACE_ROW>());
            Marshal.StructureToPtr(nif, ptr, false);
            uint r = GetIpInterfaceEntry(ptr);
            if (r == 0) {
                var result = Marshal.PtrToStructure<MIB_IPINTERFACE_ROW>(ptr);
                Marshal.FreeHGlobal(ptr);
                return result;
            } else {
                Marshal.FreeHGlobal(ptr);
                Logger.Error($"GetIpInterfaceEntry Failed, intf_index={intfIndex}, error_code={r}");
                throw new ExternalException("GetIpInterfaceEntry Failed");
            }
        }

        /// <summary>
        /// An application would typically call the GetIpInterfaceTable function to retrieve the IP interface entries on the local computer or 
        /// call the GetIpInterfaceEntry function to retrieve just the IP interface entry to modify. The MIB_IPINTERFACE_ROW structure for the 
        /// specific IP interface entry could then be modified and a pointer to this structure passed to the SetIpInterfaceEntry function in 
        /// the Row parameter. However for IPv4, an application must not try to modify the SitePrefixLength member of the MIB_IPINTERFACE_ROW 
        /// structure. For IPv4, the SitePrefixLength member must be set to 0.
        /// 
        /// Another possible method to modify an existing IP interface entry is to use InitializeIpInterfaceEntry function to initialize the 
        /// fields of a MIB_IPINTERFACE_ROW structure entry with default values.Then set the Family member and either the InterfaceIndex or 
        /// InterfaceLuid members in the MIB_IPINTERFACE_ROW structure pointed to by the Row parameter to match the IP interface to change.An 
        /// application can then change the fields in the MIB_IPINTERFACE_ROW entry it wishes to modify, and then call the SetIpInterfaceEntry 
        /// function.However for IPv4, an application must not try to modify the SitePrefixLength member of the MIB_IPINTERFACE_ROW structure.
        /// For IPv4, the SitePrefixLength member must be set to 0. Caution must be used with this approach because the only way to determine 
        /// all of the fields being changed would be to compare the fields in the MIB_IPINTERFACE_ROW of the specific IP interface entry with \
        /// fields set by the InitializeIpInterfaceEntry function when a MIB_IPINTERFACE_ROW is initialized to default values.
        /// </summary>
        /// <param name="r"></param>
        public static void SetInterface(MIB_IPINTERFACE_ROW r) {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<MIB_IPINTERFACE_ROW>());
            Marshal.StructureToPtr(r, ptr, false);
            int result = SetIpInterfaceEntry(ptr);
            if (result != 0) {
                Logger.Error($"SetInterface Failed, intf_index={r.InterfaceIndex}, error_code={result}");
                Logger.Debug($"Family: {r.Family}");
            }
            Marshal.FreeHGlobal(ptr);
        }

        [ComVisible(false)]
        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_IPFORWARDTABLE {
            internal readonly uint dwNumEntries;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            internal readonly MIB_IPFORWARDROW[] table;
        }

        [ComVisible(false)]
        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_IPFORWARDROW {
            internal uint /*DWORD*/ dwForwardDest;
            internal uint /*DWORD*/ dwForwardMask;
            internal uint /*DWORD*/ dwForwardPolicy;
            internal uint /*DWORD*/ dwForwardNextHop;
            internal uint /*DWORD*/ dwForwardIfIndex;
            internal uint /*DWORD*/ dwForwardType;
            internal uint /*DWORD*/ dwForwardProto;
            internal uint /*DWORD*/ dwForwardAge;
            internal uint /*DWORD*/ dwForwardNextHopAS;
            internal int /*DWORD*/ dwForwardMetric1;
            internal int /*DWORD*/ dwForwardMetric2;
            internal int /*DWORD*/ dwForwardMetric3;
            internal int /*DWORD*/ dwForwardMetric4;
            internal int /*DWORD*/ dwForwardMetric5;
        }


        internal enum INTERNAL_IF_OPER_STATUS : int {
            IF_OPER_STATUS_NON_OPERATIONAL = 0,
            IF_OPER_STATUS_UNREACHABLE = 1,
            IF_OPER_STATUS_DISCONNECTED = 2,
            IF_OPER_STATUS_CONNECTING = 3,
            IF_OPER_STATUS_CONNECTED = 4,
            IF_OPER_STATUS_OPERATIONAL = 5
        }

        /// <summary>
        ///  WCHAR wszName[MAX_INTERFACE_NAME_LEN];
        ///  IF_INDEX dwIndex;
        ///  IFTYPE dwType;
        ///  DWORD dwMtu;
        ///  DWORD dwSpeed;
        ///  DWORD dwPhysAddrLen;
        ///  UCHAR bPhysAddr[MAXLEN_PHYSADDR];
        ///  DWORD dwAdminStatus;
        ///  INTERNAL_IF_OPER_STATUS dwOperStatus;
        ///  DWORD dwLastChange;
        ///  DWORD dwInOctets;
        ///  DWORD dwInUcastPkts;
        ///  DWORD dwInNUcastPkts;
        ///  DWORD dwInDiscards;
        ///  DWORD dwInErrors;
        ///  DWORD dwInUnknownProtos;
        ///  DWORD dwOutOctets;
        ///  DWORD dwOutUcastPkts;
        ///  DWORD dwOutNUcastPkts;
        ///  DWORD dwOutDiscards;
        ///  DWORD dwOutErrors;
        ///  DWORD dwOutQLen;
        ///  DWORD dwDescrLen;
        ///  UCHAR bDescr[MAXLEN_IFDESCR];
        /// </summary>
        [ComVisible(false)]
        [StructLayout(LayoutKind.Sequential)]
        internal struct MIB_IFROW {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal char[] wszName;

            internal uint dwIndex;
            internal uint dwType;
            internal int dwMtu;
            internal int dwSpeed;
            internal int dwPhysAddrLen;

            [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)]
            internal byte[] bPhysAddr;

            internal int dwAdminStatus;
            internal INTERNAL_IF_OPER_STATUS dwOperStatus;
            internal int dwLastChange;
            internal int dwInOctets;
            internal int dwInUcastPkts;
            internal int dwInNUcastPkts;
            internal int dwInDiscards;
            internal int dwInErrors;
            internal int dwInUnknownProtos;
            internal int dwOutOctets;
            internal int dwOutUcastPkts;
            internal int dwOutNUcastPkts;
            internal int dwOutDiscards;
            internal int dwOutErrors;
            internal int dwOutQLen;
            internal int dwDescrLen;

            [MarshalAs(UnmanagedType.LPArray, SizeConst = 256)]
            internal byte[] bDescr;
        }

        [ComVisible(false)]
        internal enum NL_ROUTER_DISCOVERY_BEHAVIOR : int {
            RouterDiscoveryDisabled = 0,
            RouterDiscoveryEnabled,
            RouterDiscoveryDhcp,
            RouterDiscoveryUnchanged = -1
        }

        [ComVisible(false)]
        internal enum NL_LINK_LOCAL_ADDRESS_BEHAVIOR : int {
            LinkLocalAlwaysOff = 0, // Never use link locals.
            LinkLocalDelayed, // Use link locals only if no other addresses.
            // (default for IPv4).
            // Legacy mapping: IPAutoconfigurationEnabled.
            LinkLocalAlwaysOn, // Always use link locals (default for IPv6).
            LinkLocalUnchanged = -1
        }

        /// <summary>
        /// //
        /// // Key Structure;
        /// //
        /// ADDRESS_FAMILY Family;
        /// NET_LUID InterfaceLuid;
        /// NET_IFINDEX InterfaceIndex;
        /// //
        /// // Read-Write fields.
        /// //
        /// //
        /// // Fields currently not exposed.
        /// //
        /// ULONG MaxReassemblySize;
        /// ULONG64 InterfaceIdentifier;
        /// ULONG MinRouterAdvertisementInterval;
        /// ULONG MaxRouterAdvertisementInterval;
        /// //
        /// // Fileds currently exposed.
        /// //
        /// BOOLEAN AdvertisingEnabled;
        /// BOOLEAN ForwardingEnabled;
        /// BOOLEAN WeakHostSend;
        /// BOOLEAN WeakHostReceive;
        /// BOOLEAN UseAutomaticMetric;
        /// BOOLEAN UseNeighborUnreachabilityDetection;
        /// BOOLEAN ManagedAddressConfigurationSupported;
        /// BOOLEAN OtherStatefulConfigurationSupported;
        /// BOOLEAN AdvertiseDefaultRoute;
        /// 
        /// NL_ROUTER_DISCOVERY_BEHAVIOR RouterDiscoveryBehavior;
        /// ULONG DadTransmits; // DupAddrDetectTransmits in RFC 2462.
        /// ULONG BaseReachableTime;
        /// ULONG RetransmitTime;
        /// ULONG PathMtuDiscoveryTimeout; // Path MTU discovery timeout (in ms).
        /// 
        /// NL_LINK_LOCAL_ADDRESS_BEHAVIOR LinkLocalAddressBehavior;
        /// ULONG LinkLocalAddressTimeout; // In ms.
        /// ULONG ZoneIndices[ScopeLevelCount]; // Zone part of a SCOPE_ID.
        /// ULONG SitePrefixLength;
        /// ULONG Metric;
        /// ULONG NlMtu;
        /// 
        /// //
        /// // Read Only fields.
        /// //
        /// BOOLEAN Connected;
        /// BOOLEAN SupportsWakeUpPatterns;
        /// BOOLEAN SupportsNeighborDiscovery;
        /// BOOLEAN SupportsRouterDiscovery;
        /// 
        /// ULONG ReachableTime;
        /// 
        /// NL_INTERFACE_OFFLOAD_ROD TransmitOffload;
        /// NL_INTERFACE_OFFLOAD_ROD ReceiveOffload;
        /// 
        /// //
        /// // Disables using default route on the interface. This flag
        /// // can be used by VPN clients to restrict Split tunnelling.
        /// //
        /// BOOLEAN DisableDefaultRoutes;
        /// </summary>
        [ComVisible(false)]
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto, Pack = 8)]
        internal struct MIB_IPINTERFACE_ROW {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.U2)]
            internal ushort Family;

            [FieldOffset(8)]
            [MarshalAs(UnmanagedType.U8)]
            internal ulong InterfaceLuid;

            [FieldOffset(16)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint InterfaceIndex;

            [FieldOffset(20)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint MaxReassemblySize;

            [FieldOffset(24)]
            [MarshalAs(UnmanagedType.U8)]
            internal ulong InterfaceIdentifier;

            [FieldOffset(32)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint MinRouterAdvertisementInterval;

            [FieldOffset(36)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint MaxRouterAdvertisementInterval;

            [FieldOffset(40)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte AdvertisingEnabled;

            [FieldOffset(41)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte ForwardingEnabled;

            [FieldOffset(42)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte WeakHostSend;

            [FieldOffset(43)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte WeakHostReceive;

            [FieldOffset(44)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte UseAutomaticMetric;

            [FieldOffset(45)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte UseNeighborUnreachabilityDetection;

            [FieldOffset(46)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte ManagedAddressConfigurationSupported;

            [FieldOffset(47)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte OtherStatefulConfigurationSupported;

            [FieldOffset(48)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte AdvertiseDefaultRoute;

            [FieldOffset(52)]
            [MarshalAs(UnmanagedType.I4)]
            internal NL_ROUTER_DISCOVERY_BEHAVIOR RouterDiscoveryBehavior;

            [FieldOffset(56)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint DadTransmits; // DupAddrDetectTransmits in RFC 2462.

            [FieldOffset(60)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint BaseReachableTime;

            [FieldOffset(64)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint RetransmitTime;

            [FieldOffset(68)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint PathMtuDiscoveryTimeout; // Path MTU discovery timeout (in ms).

            [FieldOffset(72)]
            [MarshalAs(UnmanagedType.I4)]
            internal NL_LINK_LOCAL_ADDRESS_BEHAVIOR LinkLocalAddressBehavior;

            [FieldOffset(76)]
            [MarshalAs(UnmanagedType.I4)]
            internal uint LinkLocalAddressTimeout; // In ms.

            [FieldOffset(80)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U4)]
            internal uint[] ZoneIndices; // Zone part of a SCOPE_ID.

            // + 16 * 4 = 64
            [FieldOffset(144)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint SitePrefixLength;

            [FieldOffset(148)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint Metric;

            [FieldOffset(152)]
            [MarshalAs(UnmanagedType.U4)]
            internal uint NlMtu;

            [FieldOffset(156)]
            [MarshalAs(UnmanagedType.U1)]
            internal readonly byte Connected;

            [FieldOffset(157)]
            [MarshalAs(UnmanagedType.U1)]
            internal readonly byte SupportsWakeUpPatterns;

            [FieldOffset(158)]
            [MarshalAs(UnmanagedType.U1)]
            internal readonly byte SupportsNeighborDiscovery;

            [FieldOffset(159)]
            [MarshalAs(UnmanagedType.U1)]
            internal readonly byte SupportsRouterDiscovery;

            [FieldOffset(160)]
            [MarshalAs(UnmanagedType.U4)]
            internal readonly uint ReachableTime;

            /*typedef struct _NL_INTERFACE_OFFLOAD_ROD {
                            BOOLEAN NlChecksumSupported  :1;
                            BOOLEAN NlOptionsSupported  :1;
                            BOOLEAN TlDatagramChecksumSupported  :1;
                            BOOLEAN TlStreamChecksumSupported  :1;
                            BOOLEAN TlStreamOptionsSupported  :1;
                            BOOLEAN TlStreamFastPathCompatible  :1;
                            BOOLEAN TlDatagramFastPathCompatible  :1;
                            BOOLEAN TlLargeSendOffloadSupported  :1;
                            BOOLEAN TlGiantSendOffloadSupported  :1;
                        }*/

            [FieldOffset(164)]
            [MarshalAs(UnmanagedType.U1)]
            internal readonly byte TransmitOffload;

            [FieldOffset(165)]
            [MarshalAs(UnmanagedType.U1)]
            internal readonly byte ReceiveOffload;

            [FieldOffset(166)]
            [MarshalAs(UnmanagedType.U1)]
            internal byte DisableDefaultRoutes;
        }

        [ComVisible(false)]
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 8)]
        internal struct MIB_IPINTERFACE_TABLE {
            internal uint NumEntries;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.Struct)]
            internal MIB_IPINTERFACE_ROW[] Table;
        }
    }
}