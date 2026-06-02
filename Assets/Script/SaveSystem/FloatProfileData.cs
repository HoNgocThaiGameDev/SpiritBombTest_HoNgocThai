public class FloatProfileData
{
    private System.Func<float> getter;
    private System.Action<float> setter;
    private float defaultValue;

    public FloatProfileData(System.Func<float> getter, System.Action<float> setter, float defaultValue)
    {
        this.getter = getter;
        this.setter = setter;
        this.defaultValue = defaultValue;
    }

    public static implicit operator float(FloatProfileData data)
    {
        if (data == null) return 0f;
        return data.getter();
    }

    public void Set(float value)
    {
        setter(value);
        SaveController.MarkAsSaveIsRequired();
    }

    public override string ToString()
    {
        return getter().ToString();
    }
}
