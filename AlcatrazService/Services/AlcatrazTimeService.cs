using AlcatrazGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Security.Cryptography.X509Certificates;

using static AlcatrazService.Common.Constants;
using AlcatrazService.Exceptions;

namespace AlcatrazService.Services
{
    public class AlcatrazTimeService(IDB db, ILogger<AlcatrazTimeService> logger) 
        : TimeService.TimeServiceBase
    {
        private readonly IDB db = db;
        private readonly ILogger<AlcatrazTimeService> logger = logger;

        /// <summary>
        /// Get the Time Now
        /// </summary>
        /// <returns>Time Now as Message in TimeReply</returns>
        public override async Task<TimeReply> GetTime(Empty request, ServerCallContext context)
        {
            var nowToString = DateTime.UtcNow.ToString();

            db.LogCallToDatabase(nowToString);

            return await Task.FromResult(new TimeReply
            {
                Message = nowToString,
            });
        }

        /// <summary>
        /// Retrieves time calls from database
        /// </summary>
        /// <exception cref="Exceptions.InvalidCertificateException">Thrown when Invalid Certificate</exception>
        /// <returns>Time Calls as string[]</returns>
        private async Task<string[]?> QueryTimeRetrievalCalls()
        {
            if (!ValidCertificate())
            {
                logger.LogWarning("Invalid certificate");
                throw new InvalidCertificateException("Invalid certificate");
            }

            return await db.GetTimeRetrievalCalls();
        }

        private bool ValidCertificate()
        {
            // Get the certificate to verify
            var cert = GetCertificateFromStore(CERT_NAME);
            if (cert == null)
            {
                logger.LogWarning("Certificate 'CN=www.alcatraz.io' not found.");
                return false;
            }
            return true;
        }

        private X509Certificate2? GetCertificateFromStore(string certName)
        {
            // Get the certificate store for the current user.
            X509Store store = new(StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                // Place all certificates in an X509Certificate2Collection object.
                X509Certificate2Collection certCollection = store.Certificates;
                // If using a certificate with a trusted root you do not need to FindByTimeValid, instead:
                // currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, true);
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                // Return the first certificate in the collection, has the right name and is current.
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}
