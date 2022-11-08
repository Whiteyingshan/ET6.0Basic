using UnityEngine.Networking;

namespace VEngine
{
    internal class AcceptAllCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}