namespace WebAPI.Utils
{
    public class PlayerKeyComparer : IEqualityComparer<PlayerStruct>
    {
        public bool Equals(PlayerStruct x, PlayerStruct y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(PlayerStruct obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
