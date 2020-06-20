namespace Ellumination.Collections
{
    /// <summary>
    /// Callback for purposes of verifying the Binary Operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public delegate T CalculateBinaryOperationCallback<T>(T a, T b);

    /// <summary>
    /// Callback for purposes of verifying the Unary Operation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a"></param>
    /// <returns></returns>
    public delegate T CalculateUnaryOperationCallback<T>(T a);
}
