using System.ComponentModel;

namespace Ellumination.Collections
{
    public abstract partial class Enumeration
    {

#if false // TODO: TBD: what to do about CATEGORY ...

        ///// <summary>
        ///// CategoryName backing field.
        ///// </summary>
        //private string _categoryName;

        ///// <summary>
        ///// Gets the CategoryName. Depends upon a field being optionally decorated
        ///// by the <see cref="CategoryAttribute"/> attribute.
        ///// </summary>
        ///// <see cref="CategoryAttribute"/>
        //public string CategoryName
        //{
        //    get
        //    {
        //        TryResolveCategoryName(ref _categoryName);
        //        return _categoryName;
        //    }
        //}

        ///// <summary>
        ///// Tries to resolve the CategoryName.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        ///// <see cref="CategoryAttribute"/>
        //private bool TryResolveCategoryName(ref string value)
        //{
        //    if (value != null)
        //    {
        //        return true;
        //    }

        //    var fi = GetDeclaringTypes().SelectMany(type => type.GetFields())
        //        .SingleOrDefault(info => ReferenceEquals(this, info.GetValue(null)));

        //    var category = fi?.GetCustomAttribute<CategoryAttribute>(false);

        //    value = category?.Category;

        //    return !string.IsNullOrEmpty(value);
        //}

        ///// <summary>
        ///// CategoryDisplayName backing field.
        ///// </summary>
        //private string _categoryDisplayName;

        ///// <summary>
        ///// Gets the CategoryDisplayName.
        ///// </summary>
        ///// <see cref="CategoryName"/>
        //public string CategoryDisplayName
        //{
        //    get
        //    {
        //        TryResolveHumanReadableCamelCase(ref _categoryDisplayName, CategoryName);
        //        return _categoryDisplayName;
        //    }
        //}

#endif // false

    }
}
