public class BoolProfileData
{
    private System.Func<bool> getter;
    private System.Action<bool> setter;
    private bool defaultValue;

    public BoolProfileData(System.Func<bool> getter, System.Action<bool> setter, bool defaultValue)
    {
        this.getter = getter;
        this.setter = setter;
        this.defaultValue = defaultValue;
    }

    public static implicit operator bool(BoolProfileData data)
    {
        if (data == null) return false;
        return data.getter();
    }

    public void Set(bool value)
    {
        setter(value);
        SaveController.MarkAsSaveIsRequired();
    }

    public override string ToString()
    {
        return getter().ToString();
    }
}
