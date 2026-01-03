using UnityEngine;

[System.Serializable]
public class PackageItem
{
    //编号
    public int id;
    //分类
    //1-武器，2-护甲，3-饰品，4-药物，5-任务物品
    public int type;
    //星级
    public int star;
    //名称
    public string name;
    //简单描述
    public string description;
    //详细描述
    public string skillDescription;
    //图片路径
    public string imagePath;
    //数量
    public int num;
}
