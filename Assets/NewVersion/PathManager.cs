using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    public enum NODE_TYPE{
        REGULAR_BUILDING,
        HOLED_BUILDING
    }

    public struct PathNode
    {
        public float StartPos;
        public float EndPos;
        
        public float Height;
        public NODE_TYPE NodeType;
        
    }
    public List<PathNode> Path;
    public PathManager(int size)
    {
        Path = new List<PathNode>(size);
    }
    public float IncomingDistance;
    public Vector3 Direction, Origin;
    public CannonControl StartCannon;
    public float GetHeight(float Position, float MaxHeightSince= -999f, int StartIndex=0 )
    {
        for(int i = StartIndex; i < Path.Count-1; i++)
        {
            MaxHeightSince = Mathf.Max(Path[i].Height,MaxHeightSince);
            float MaxHeightNext = Mathf.Max(Path[i+1].Height,MaxHeightSince);
            if(Position >= Path[i].StartPos && Position < Path[i+1].StartPos)
            {
                //return MaxHeightSince;
                return Mathf.Lerp(MaxHeightSince,MaxHeightNext,((IncomingDistance - (Path[i+1].StartPos - Position))/IncomingDistance));
            }
        }
        return MaxHeightSince;
    }
    
    public float GetHeight(float Position, ref float MaxHeightSince,ref int StartIndex, out float angle, out bool isHole)
    {
        for(int i = StartIndex; i < Path.Count-1; i++)
        {
            MaxHeightSince = Mathf.Max(Path[i].Height,MaxHeightSince);
            float MaxHeightNext = Mathf.Max(Path[i+1].Height,MaxHeightSince);
            if(Position >= Path[i].StartPos && Position < Path[i+1].StartPos)
            {
                //return MaxHeightSince;
                StartIndex = i;
                float normalizedLerpParameter = ((IncomingDistance - (Path[i+1].StartPos - Position))/IncomingDistance);

                
                float resultHeight = Mathf.Lerp(MaxHeightSince,MaxHeightNext,normalizedLerpParameter);
                float a = Mathf.Atan((MaxHeightNext-resultHeight) / (Path[i+1].StartPos - Position));
                angle =- Mathf.Rad2Deg*a;
                isHole = (a>=0 && Path[i+1].NodeType == NODE_TYPE.HOLED_BUILDING) ||(Path[i].NodeType == NODE_TYPE.HOLED_BUILDING);
        
                return resultHeight;
            }
        }
        angle = 0f;
        isHole = Path[Path.Count-1].NodeType == NODE_TYPE.HOLED_BUILDING;
        return MaxHeightSince;
    }
    public NODE_TYPE GetNodeType (float Position)
    {
        
        for(int i = 0; i < Path.Count-1; i++)
        {
            if(Position >= Path[i].StartPos && Position < Path[i+1].StartPos)
            {
                return Path[i].NodeType;
            }
        }
        return NODE_TYPE.REGULAR_BUILDING;
    }
    

}