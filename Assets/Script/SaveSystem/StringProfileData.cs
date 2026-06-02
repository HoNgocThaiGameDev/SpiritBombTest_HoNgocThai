public class StringProfileData
{
    private System.Func<string> getter;
    private System.Action<string> setter;
    private string defaultValue;

    public StringProfileData(System.Func<string> getter, System.Action<string> setter, string defaultValue)
    {
        this.getter = getter;
        this.setter = setter;
        this.defaultValue = defaultValue;
    }

    public static implicit operator string(StringProfileData data)
    {
        if (data == null) return string.Empty;
        string value = data.getter();
        return value ?? string.Empty;
    }

    public void Set(string value)
    {
        setter(value ?? defaultValue);
        SaveController.MarkAsSaveIsRequired();
    }

    public override string ToString()
    {
        string value = getter();
        return value ?? string.Empty;
    }
}
