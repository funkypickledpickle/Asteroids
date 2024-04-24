namespace Asteroids.ValueTypeECS.Delegates
{
    public delegate void ActionReference<TReferenced>(ref TReferenced referenced) where TReferenced : struct;
    public delegate void ActionReferenceValue<TReferenced, TValue>(ref TReferenced referenced, TValue value) where TReferenced : struct;
    public delegate TReturn FunctionReference<TReferenced, TReturn>(ref TReferenced referenced) where TReferenced : struct;
}
