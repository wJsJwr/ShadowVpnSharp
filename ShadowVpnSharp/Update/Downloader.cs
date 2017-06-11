using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShadowVpnSharp.Update {
    class Downloader:WebClient {
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";

        public Downloader():base() {
            this.Headers.Add("User-Agent", UserAgent);
        }

    }
}
