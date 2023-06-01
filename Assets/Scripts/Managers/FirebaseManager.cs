using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        private void Awake()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(_ =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }
    }
}
