using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance { get; private set;}

    public FirebaseFirestore db { get; private set; }
    public FirebaseAuth auth { get; private set; }

    FirebaseApp app;
    public CollectionReference playerCol { get; private set; }

    IEnumerator Start()
    {
        if (instance != null && instance!= this)
        {
            Destroy(gameObject);
            yield break;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        var checkTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => checkTask.IsCompleted);

#if UNITY_EDITOR
        app = FirebaseApp.Create(FirebaseApp.DefaultInstance.Options, Random.Range(-100f,100f).ToString());
        db = FirebaseFirestore.GetInstance(app);
        auth = FirebaseAuth.GetAuth(app);
#else
        app = FirebaseApp.DefaultInstance;
        db = FirebaseFirestore.GetInstance(app);
        auth = FirebaseAuth.GetAuth(app);
#endif

#if UNITY_EDITOR
        playerCol = db.Collection("Player-Test");
#else
        playerCol = db.Collection("Player");
#endif

    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.I))
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "nama", "Budi" },
                { "Email", "Budi@gmail.com" }
            };
            db.Collection("Player-Test").Document("Budi").SetAsync(data, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Data Berhasil Diinput");
                Debug.Log("Exception: " + (task.Exception != null ? task.Exception.Message + "\n" + task.Exception.StackTrace : ""));
            });
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(TestLoadCor());
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            db.Collection("Player-Test").Document("Budi").DeleteAsync();
            Debug.Log("Data Berhasil Dihapus");
        }*/
    }

    IEnumerator TestLoadCor()
    {
        var getTask = db.Collection("Player-Test").Document("Budi").GetSnapshotAsync();
        yield return new WaitUntil(() => getTask.IsCompleted);

        DocumentSnapshot snapshot = getTask.Result;
        PlayerData playerData = new PlayerData();

        snapshot.TryGetValue(nameof(PlayerData.nama), out playerData.nama);
        snapshot.TryGetValue(nameof(PlayerData.Email), out playerData.Email);
        Debug.Log("Resut Player Data: " + JsonUtility.ToJson(playerData));

        //foreach (var c in getTask.Result.ToDictionary())
        //{
        //    Debug.Log(c.Key + ": " + c.Value);
        //}

        Debug.Log("Data Berhasil Diambil");
    }

    [System.Serializable]
    public class PlayerData
    {
        public string nama;
        public string Email;
        public int Score;
    }

}
