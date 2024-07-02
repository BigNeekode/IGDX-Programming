#define EMAIL_VERIFICATION

using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AuthUI : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] TMP_InputField emailLogin;
    [SerializeField] TMP_InputField passwordLogin;

    [Header("Register")]
    [SerializeField] TMP_InputField emailRegister;
    [SerializeField] TMP_InputField usernameRegister;
    [SerializeField] TMP_InputField passwordRegister;

    [Header("Info")]
    [SerializeField] TextMeshProUGUI infoText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        FirebaseManager.instance.auth.SignInWithEmailAndPasswordAsync(emailLogin.text, passwordLogin.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log("Gagal Login");
                Debug.Log("Exception: " + task.Exception.Message + "\n" + task.Exception.StackTrace);
                return;
            }
#if EMAIL_VERIFICATION
            if (!FirebaseManager.instance.auth.CurrentUser.IsEmailVerified)
            {
                infoText.text = "Email Belum Terverifikasi";
                return;
            }
            infoText.text = "Berhasil Login \n" + FirebaseManager.instance.auth.CurrentUser.Email + "\n Username : " + FirebaseManager.instance.auth.CurrentUser.DisplayName;
#else
            infoText.text = "Berhasil Login \n" + FirebaseManager.instance.auth.CurrentUser.Email + "\n Username : " + FirebaseManager.instance.auth.CurrentUser.DisplayName;
#endif
        });
    }

    public void LoginGuest()
    {
        FirebaseManager.instance.auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log("Gagal Login Guest");
                Debug.Log("Exception: " + task.Exception.Message + "\n" + task.Exception.StackTrace);
                return;
            }

            PlayerPrefs.SetString("GuestId", task.Result.User.UserId);
            Debug.Log("creation time : " + task.Result.User.Metadata.CreationTimestamp + " ** Last Login : " + task.Result.User.Metadata.LastSignInTimestamp);

            FirebaseManager.instance.playerCol.Document(task.Result.User.UserId).GetSnapshotAsync().ContinueWithOnMainThread(task2 =>
            {
                if (!task2.Result.Exists)
                {
                    Dictionary<string, object> data = new Dictionary<string, object>
                    {
                        { "nama", task.Result.User.UserId },
                        { "Score", 0 }
                    };
                    FirebaseManager.instance.playerCol.Document(task.Result.User.UserId).SetAsync(data, Firebase.Firestore.SetOptions.MergeAll);

                    task.Result.User.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
                    {
                        DisplayName = task.Result.User.UserId
                    });
                    
                    infoText.text = "Berhasil Login Guest \n" + "Username : " + FirebaseManager.instance.auth.CurrentUser.DisplayName;

                }
            });

            
        });
    }

    public void Register()
    {
        FirebaseManager.instance.auth.CreateUserWithEmailAndPasswordAsync(emailRegister.text, passwordRegister.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.Log("Gagal Register");
                Debug.LogError("Exception: " + task.Exception.Message + "\n" + task.Exception.StackTrace);
                return;
            }
            FirebaseManager.instance.auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
            {
                DisplayName = usernameRegister.text
            }).ContinueWithOnMainThread(task2 =>
            {
                if (task2.Exception != null)
                {
                    Debug.Log("Gagal Update Username");
                    Debug.LogError("Exception: " + task2.Exception.Message + "\n" + task2.Exception.StackTrace);
                    return;
                }
                
            });

            Dictionary<string, object> data = new ()
            {
                { "nama",  usernameRegister.text },
                { "Email", FirebaseManager.instance.auth.CurrentUser.Email },
                { "Score", 0 }
            };
            FirebaseManager.instance.playerCol.Document(FirebaseManager.instance.auth.CurrentUser.UserId).SetAsync(data, Firebase.Firestore.SetOptions.MergeAll);

#if EMAIL_VERIFICATION
            task.Result.User.SendEmailVerificationAsync();
            infoText.text = "Berhasil Register \n" + FirebaseManager.instance.auth.CurrentUser.Email + "\n Username : " + FirebaseManager.instance.auth.CurrentUser.DisplayName + "\n Please Check Your Email!";
#else
            infoText.text = "Berhasil Register \n" + FirebaseManager.instance.auth.CurrentUser.Email + "\n Username : " + FirebaseManager.instance.auth.CurrentUser.DisplayName;
#endif

        });
    }
}
