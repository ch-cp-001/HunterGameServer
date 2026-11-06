using System.Collections;
using System.Collections.Generic;

public enum TerrainType
{
    // 正常
    Normal=0,
    // 障碍
    Obstacle=1
}

public class TerrainData:BaseData
{
    // 地图宽度
    public int width;
    // 地图高度
    public int height;
    // 地图数据
    public List<TerrainType> map;

    public override int GetByteNum()
    {
        return 4 + 4 + 4 + map.Count * 4;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        width = ReadInt(bytes,ref index);
        height = ReadInt(bytes,ref index);
        int count = ReadInt(bytes,ref index);
        if(count > 0)
        {
            map = new List<TerrainType>();
            for(int i = 0; i < count; i++)
            {
                map.Add((TerrainType)ReadInt(bytes,ref index));
            }
        }
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes,width,ref index);
        WriteInt(bytes,height,ref index);
        WriteInt(bytes,map.Count,ref index);
        if(map.Count > 0)
        {
            for(int i = 0;i < map.Count; i++)
            {
                WriteInt(bytes, (int)map[i],ref index);
            }
        }
        return bytes;

    }
}
