using System;
using Deepwell;

public class ElementTags : DWTags
{
    public static int CanStandOn = (int)((UInt32)1 << 0);
    public static int CanRunInto = (int)((UInt32)1 << 1);
    public static int IsCharacter = (int)((UInt32)1 << 2);
    public static int IsPlayer = (int)((UInt32)1 << 3);
}
