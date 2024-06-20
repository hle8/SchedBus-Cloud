using Google.Cloud.Firestore;
using MvvmHelpers;
using Newtonsoft.Json;
using SchedBus.Models.FirestoreDocuments;

namespace SchedBus.Services;

public class FirestoreService
{
    private FirestoreDb? _firestoreDb;

    private async Task FirestoreInit()
    {
        if (_firestoreDb != null)
            return;

        var filepath = Path.Combine(FileSystem.CacheDirectory, "schedbus.json");

        using var json = await FileSystem.OpenAppPackageFileAsync("schedbus.json");
        using var dest = File.Create(filepath);

        await json.CopyToAsync(dest);

        json.Close();
        dest.Close();

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

        _firestoreDb = FirestoreDb.Create("schedbus-8560b");
    }

    public async Task<List<PlanDocument>?> GetAllPlans(string userEmail)
    {
        await FirestoreInit();

        try
        {
            var planQuery = _firestoreDb.Collection(userEmail);
            var planQuerySnapshot = await planQuery.GetSnapshotAsync();

            List<PlanDocument> LstPlan = [];

            foreach (DocumentSnapshot documentSnapshot in planQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> planDict = documentSnapshot.ToDictionary();

                    string json = JsonConvert.SerializeObject(planDict);
                    var plan = JsonConvert.DeserializeObject<PlanDocument>(json);

                    plan.Id = documentSnapshot.Id;

                    LstPlan.Add(plan);
                }
            }

            return LstPlan;
        }
        catch
        {
            throw;
        }
    }

    public async Task AddPlan(PlanDocument plan, string userEmail)
    {
        await FirestoreInit();

        try
        {
            CollectionReference colRef = _firestoreDb.Collection(userEmail);
            await colRef.AddAsync(plan);
        }
        catch
        {
            throw;
        }
    }

    public async Task UpdatePlan(PlanDocument plan, string userEmail)
    {
        await FirestoreInit();

        try
        {
            DocumentReference docRef = _firestoreDb.Collection(userEmail).Document(plan.Id);
            await docRef.SetAsync(plan, SetOptions.Overwrite);
        }
        catch
        {
            throw;
        }
    }

    public async Task DeletePlan(string id, string userEmail)
    {
        await FirestoreInit();

        try
        {
            DocumentReference docRef = _firestoreDb.Collection(userEmail).Document(id);
            await docRef.DeleteAsync();
        }
        catch
        {
            throw;
        }
    }
}
