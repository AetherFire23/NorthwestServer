using Newtonsoft.Json;
namespace WebAPI.Landmasses;

public static class LandmassGetter
{
    private static List<LandmassLayout> ReadLandmassLayouts()
    {
        string allLandmassesSerialized = File.ReadAllText("Layout.txt");
        List<LandmassLayout>? layouts = JsonConvert.DeserializeObject<List<LandmassLayout>>(allLandmassesSerialized) ?? new List<LandmassLayout>();
        return layouts;
    }

    private static LandmassLayout GetRandomLandmassLayout()
    {
        List<LandmassLayout> allLayouts = ReadLandmassLayouts();
        int randomIndex = Random.Shared.Next(0, allLayouts.Count);
        LandmassLayout randomLayout = allLayouts.ElementAt(randomIndex);
        return randomLayout;
    }

    public static LandmassLayout CreateNewLandmassLayoutAndInsertNames(List<string> drawnRoomNames)
    {
        LandmassLayout layout = GetRandomLandmassLayout();
        for (int i = 0; i < drawnRoomNames.Count; i++)
        {
            string name = drawnRoomNames.ElementAt(i);
            layout.AllRooms.ElementAt(i).Name = name;
        }

        return layout;
    }

    public static LandmassLayout CreateNewLandmass()
    {
        LandmassLayout layout = GetRandomLandmassLayout();
        return layout;
    }

    public static LandmassLayout InsertLandmassNamesInLayout(LandmassLayout layout, List<string> drawnRoomNames)
    {
        for (int i = 0; i < drawnRoomNames.Count; i++)
        {
            string name = drawnRoomNames.ElementAt(i);
            layout.AllRooms.ElementAt(i).Name = name;
        }

        return layout;
    }
}