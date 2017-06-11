using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ShadowVpnSharp.Helper {
    /// <summary>
    /// IPAddress Wrapper for ipv4
    /// </summary>
    public class v4addr {
        public IPAddress Addr { get; }

        public v4addr(string a) {
            if (a == "localhost") {
                Addr = IPAddress.Loopback;
            } else if (a == "any") {
                Addr = IPAddress.Any;
            } else {
                try {
                    if (Regex.IsMatch(a, @"(\d{1,3}\.){3}\d{1,3}")) {
                        Addr = IPAddress.Parse(a);
                        if (Addr.AddressFamily == AddressFamily.InterNetworkV6) throw new Exception();
                    } else {
                        IPAddress valid = null;
                        foreach (var ip in Dns.GetHostAddresses(a)) {
                            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                                valid = ip;
                                break;
                            }
                        }
                        if (valid == null) throw new Exception();
                        Addr = valid;
                    }
                } catch(SocketException e) { 
                    Logger.Error(e.Message);
                    throw;
                } catch {
                    Logger.Error($"Unsupported address string: {a}");
                    throw new ArgumentException("Unsupported string");
                }
            }
        }

        public v4addr(uint i) {
            Addr = new IPAddress(i);
        }

        public static v4addr GetMaskAddr(int maskbits) {
            if (maskbits > 32 || maskbits < 0) return null;
            uint mask = 0;
            for (int i = 0; i < maskbits; i++) {
                mask |= 1u << (31 - i);
            }
            mask = (mask & 0x000000FF) << 24 | (mask & 0xFF000000) >> 24 | (mask & 0x00FF0000) >> 8 |
                   (mask & 0x0000FF00) << 8;
            return new v4addr(mask);
        }

        public v4addr(IPAddress a) {
            Addr = a;
        }

        public uint ToUInt32() {
            return BitConverter.ToUInt32(Addr.GetAddressBytes(), 0);
        }

        public override string ToString() {
            return Addr.ToString();
        }

        public override bool Equals(object obj) {
            var value = obj as v4addr;
            if (value == null) return false;
            return value.ToUInt32() == ToUInt32();
        }

        protected bool Equals(v4addr other) {
            return Equals(Addr, other.Addr);
        }

        public override int GetHashCode() {
            return Addr?.GetHashCode() ?? 0;
        }

        public static v4addr operator &(v4addr a, v4addr b) {
            return new v4addr(a.ToUInt32() & b.ToUInt32());
        }


        public static implicit operator string(v4addr value) {
            return value?.ToString();
        }
    }
}