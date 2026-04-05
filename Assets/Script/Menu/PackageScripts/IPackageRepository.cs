using System.Collections.Generic;

public interface IPackageRepository
{
    void Save(Dictionary<string, PackageItemData> data);
    PackageSaveData LoadRaw(); // 返回原始的保存结构以便 Controller 映射 template
}