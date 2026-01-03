using UnityEngine;
using System.Collections.Generic;

public class PackageLocalItem 
{
    public string uid;

    public string id;

    public int num;

    public int level;

    public bool isNew;

    public override string ToString()
    {
        return string.Format("[id]:[0] [num]:[1]", id, num);
    }
}
