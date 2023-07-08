namespace WebAPI.Utils
{
    public class PlayerKeyComparer : IEqualityComparer<PlayerStruct>
    {
        public bool Equals(PlayerStruct x, PlayerStruct y)
        {
            return x.PlayerId.Equals(y.PlayerId);
        }

        public int GetHashCode(PlayerStruct obj)
        {
            return obj.PlayerId.GetHashCode();
        }
    }
}
