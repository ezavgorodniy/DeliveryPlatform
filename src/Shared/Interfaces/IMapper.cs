namespace Shared.Interfaces
{
    public interface IMapper<TFrom, TTo>
    {
        TTo To(TFrom from);

        TFrom From(TTo to);
    }
}
