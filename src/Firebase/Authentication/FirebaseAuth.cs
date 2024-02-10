using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

namespace GamifyingTasks.Firebase.Authentication
{
    public static class FirebaseAuth
    {
        private static FirebaseAuthClient m_client;
        private static FirebaseAuthClient init()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyCvAMO3R39WnJmTlyHiRyUyCMRy0uiGejc",
                AuthDomain = "hons-project-f5a1e.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]{
                new EmailProvider()
            }
            };
            return new FirebaseAuthClient(config);

        }

        public static FirebaseAuthClient GetClient()
        {

            if (m_client == null)
                m_client = init();
            return m_client;
        }
    }
}
