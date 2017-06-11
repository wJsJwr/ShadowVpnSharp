using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowVpnSharp.Helper {
    class Counter {
        private ulong _value;
        private ulong _prev;

        public Counter(ulong start) {
            _value = start;
            _prev = start;
        }


        public ulong Value {
            get {
                _prev = _value;
                return _value;
            }
            set { _value = value; }
        }

        public void Reset(ulong value = 0) {
            _value = value;
            _prev = value;
        }

        public ulong Offset {
            get {
                var offset = _value - _prev;
                _prev = _value;
                return offset;
            }
        }

    }
}
