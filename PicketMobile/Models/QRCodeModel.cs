using System;
using System.Collections.Generic;
using System.Text;

namespace PicketMobile.Models
{
    public class QRCodeModel
    {
        public string Type { get; set; }
        public string? SessionId { get; set; }
        public string Endpoint { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
