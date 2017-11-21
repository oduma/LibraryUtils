namespace Sciendo.Mixx.DataAccess
{
    public interface IMap<T, T1>
    {
        T1 Transform(T fromDataType);

        T Transform(T1 fromDataType);
    }
}
