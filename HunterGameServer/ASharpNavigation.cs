
using HunterGameServer;
using System.Numerics;

public class ASharpNavigation
{
    private static ASharpNavigation instance = new ASharpNavigation();
    public static ASharpNavigation Instance => instance;

    // 地形数据
    //public List<TerrainType> terrains;
    // 
    private TerrainData terrain;
    // 开始节点坐标
    private int startX;
    private int startY;
    // 结束节点坐标
    private int endX;
    private int endY;

    // 开始列表
    private List<ASharpData> startList;
    // 结束列表
    private List<ASharpData> endList;
    // 已处理过的节点索引,用于获取
    private List<int> handleIndex;
    private List<int> res;// 最后记录路径（顺序）

    public ASharpNavigation()
    {
        startList = new List<ASharpData>();
        endList = new List<ASharpData>();
        handleIndex = new List<int>();
        res = new List<int>();
    }

    public void SetTerrain(TerrainData terrainData)
    {
        terrain = terrainData;
    }

    private void Clear()
    {
        startList.Clear();
        endList.Clear();
        handleIndex.Clear();
        res.Clear();
    }

    // 提供外部使用的获取路径方法，传入开始坐标和结束坐标，返回路径索引的列表
    public List<int> GetPath(int startX,int startY, int endX,int endY)
    {
        Clear();
        this.startX = startX;
        this.startY = startY;
        this.endX = endX;
        this.endY = endY;
        
        
        // 计算目标位置索引
        int endIndex = endX + endY * terrain.width;
        
        // 先将开始节点放入开始列表中，设置-1作为后续推出条件
        AddStartList(startX,startY,-1);

        //之后每次从开始列表选择寻路消耗最少的节点加入结束列表后以此节点为起始节点寻路
        ASharpData minConsume;
        while (startList.Count > 0)
        {
            // 按照consume升序排列
            startList.Sort();
            minConsume = startList[0];
            // 找到最后节点
            if (minConsume.myIndex == endIndex)
            {
                // 倒推路径后直接退出
                SearchPath(minConsume);
                res.Reverse();
                return res;
            }

            endList.Add(minConsume);
            startList.RemoveAt(0);
            
            AddStartList(minConsume.myIndex,endList.Count-1);
        }
        // 返回空表示没找到路径
        return null;
    } 
    

    /// <summary>
    /// 提供一个索引，将它周围的四个节点添加到列表添加(这里不允许斜向移动，只考虑上下左右)
    /// </summary>
    /// <param name="index">地图数据列表中的索引，terrains中的</param>
    /// <param name="endIndex">endList中的索引，用于在倒推路径时快速访问</param>
    private void AddStartList(int index,int endIndex)
    {
        int x = index % terrain.width;
        int y = index / terrain.width;

        AddStartList(x+1,y, endIndex);
        AddStartList(x,y-1, endIndex);
        AddStartList(x-1,y, endIndex);
        AddStartList(x,y+1, endIndex);
    }

    // 提供一个节点坐标xy，将该索引节点添加到开始列表中
    private void AddStartList(int x,int y,int parentIndex)
    {
        // 节点坐标在地图范围内
        if (x>=0 && x < terrain.width && y>=0 && y<terrain.height)
        {
            int index = x + y * terrain.width;
            // 障碍不可通行，不加入寻路计算
            if (terrain.map[index] == TerrainType.Obstacle) return;
            // 节点未处理过就计算寻路消耗后添加到开始列表中
            if (!handleIndex.Contains(index))
            {
                ASharpData data = new ASharpData();
                data.myIndex = index;
                data.parentIndex = parentIndex;
                data.consume = CalConsume(x,y);
                startList.Add(data);
                handleIndex.Add(index);
            }
        }
    }

    // 计算寻路消耗（f(寻路消耗预估) = g（勾股计算距离开始节点距离） + h（街区距离计算结束节点距离））
    private float CalConsume(int x,int y)
    {
        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(startX, startY))+            
            Math.Abs(x-endX)+Math.Abs(y-endY);
        return distance;
    }

    // 从结束列表中倒推路径
    private void SearchPath(ASharpData end)
    {
        res.Add(end.myIndex);
        while (true)
        {
            if (end.parentIndex == -1) break;
            end = endList[end.parentIndex];
            res.Add(end.myIndex);
        }
    }
   
}

class ASharpData:IComparable<ASharpData>
{
    // 自己的索引(terrain地图数据列表中的索引位置)
    public int myIndex;
    // 父节点索引(结束列表中该父节点的位置)
    public int parentIndex;
    // 寻路消耗
    public float consume;

    public int CompareTo(ASharpData other)
    {
        if (this.consume > other.consume) return 1;
        return -1;
    }
}