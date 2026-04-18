using Feature.GameSessionData;

namespace Feature.Entity.Script
{
    public interface ITargetable
    {
        CardAndHealthEntityOwnerData Owner { get; set; }
    }
}