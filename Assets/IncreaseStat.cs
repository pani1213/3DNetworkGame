public struct IncreaseStat 
{
    public ItemType ItemType;
    public float Value;

    public IncreaseStat(ItemType _itemType, float _value)
    {
        ItemType = _itemType;
        Value = _value;
    }
}
