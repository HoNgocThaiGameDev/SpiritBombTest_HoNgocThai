using System;

[Serializable]
public struct SecuredInt
{
    private int value;

    public SecuredInt(int value)
    {
        this.value = value;
    }

    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public static implicit operator int(SecuredInt secured)
    {
        return secured.value;
    }

    public static implicit operator SecuredInt(int value)
    {
        return new SecuredInt(value);
    }
}

[Serializable]
public struct SecuredFloat
{
    private float value;

    public SecuredFloat(float value)
    {
        this.value = value;
    }

    public float Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public override string ToString()
    {
        return value.ToString();
    }

    public static implicit operator float(SecuredFloat secured)
    {
        return secured.value;
    }

    public static implicit operator SecuredFloat(float value)
    {
        return new SecuredFloat(value);
    }
}
