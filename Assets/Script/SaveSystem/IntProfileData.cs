public class IntProfileData
{
    private System.Func<int> getter;
    private System.Action<int> setter;
    private int defaultValue;

    public IntProfileData(System.Func<int> getter, System.Action<int> setter, int defaultValue)
    {
        this.getter = getter;
        this.setter = setter;
        this.defaultValue = defaultValue;
    }

    public static implicit operator int(IntProfileData data)
    {
        if (data == null) return 0;
        return data.getter();
    }

    public void Set(int value)
    {
        setter(value);
        SaveController.MarkAsSaveIsRequired();
    }

    public override string ToString()
    {
        return getter().ToString();
    }
}
