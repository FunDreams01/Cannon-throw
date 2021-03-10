using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager
{
    public enum NODE_TYPE
    {
        REGULAR_BUILDING,
        HOLED_BUILDING,
        LAST_BUILDING
    }
    public struct WallNode{
        public float StartPos;
        public float EndPos;
        public int SideMultipliers;
    }

    public struct PathNode
    {
        public float StartPos;
        public float EndPos;

        public float Height;
        public NODE_TYPE NodeType;

    }

    public List<WallNode> Walls;




    public List<PathNode> Path;
    public PathManager(int size)
    {
        Path = new List<PathNode>(size);
        Walls = new List<WallNode>();
    }
    public float IncomingDistance;
    public Vector3 Direction, Origin;
    public CannonControl StartCannon, EndCannon;

    public bool isWall (float Position, ref int StartIndex, out int mul) {
        
        for (int i = StartIndex; i < Walls.Count - 1; i++)
        {
            if(Position < Walls[i].EndPos && Position>Walls[i].StartPos)
            {
                StartIndex = i;
                mul = Walls[i].SideMultipliers;
                return true;
            }
        }
        mul = 0;
        return false;

    }
    public float End;
    public float GetHeight(float Position, float MaxHeightSince = -999f, int StartIndex = 0)
    {
        for (int i = StartIndex; i < Path.Count - 1; i++)
        {
            MaxHeightSince = Mathf.Max(Path[i].Height, MaxHeightSince);
            float MaxHeightNext = Mathf.Max(Path[i + 1].Height, MaxHeightSince);
            if (i == Path.Count - 2) { MaxHeightNext = Path[i + 1].Height; }
            if (Position >= Path[i].StartPos && Position < Path[i + 1].StartPos)
            {
                //return MaxHeightSince;
                return Mathf.Lerp(MaxHeightSince, MaxHeightNext, ((IncomingDistance - (Path[i + 1].StartPos - Position)) / IncomingDistance));
            }
        }
        if (Position < Path[StartIndex].StartPos)
        {
            return 0;
        }
        return Path[Path.Count - 1].Height;
    }

    public float GetHeight(float Position, ref float MaxHeightSince, ref int StartIndex, out float angle, out bool isHole)
    {
        for (int i = StartIndex; i < Path.Count - 1; i++)
        {
            MaxHeightSince = Mathf.Max(Path[i].Height, MaxHeightSince);
            float MaxHeightNext = Mathf.Max(Path[i + 1].Height, MaxHeightSince);
            if (i == Path.Count - 2) { MaxHeightNext = Path[i + 1].Height; }
            if (Position >= Path[i].StartPos && Position < Path[i + 1].StartPos)
            {
                //return MaxHeightSince;
                StartIndex = i;
                float normalizedLerpParameter = ((IncomingDistance - (Path[i + 1].StartPos - Position)) / IncomingDistance);


                float resultHeight = Mathf.Lerp(MaxHeightSince, MaxHeightNext, normalizedLerpParameter);
                float a = Mathf.Atan((MaxHeightNext - resultHeight) / (Path[i + 1].StartPos - Position));
                angle = -Mathf.Rad2Deg * a;
                isHole = (a >= 0 && Path[i + 1].NodeType == NODE_TYPE.HOLED_BUILDING) || (Path[i].NodeType == NODE_TYPE.HOLED_BUILDING);

                return resultHeight;
            }
        }
        angle = 0f;
        isHole = Path[Path.Count - 1].NodeType == NODE_TYPE.HOLED_BUILDING;
        if (Position < Path[StartIndex].StartPos)
        {
            MaxHeightSince = 0;
            StartIndex = 0;
            isHole = false;
            angle = 0f;

         //   float MaxHeightNext = Mathf.Max(Path[0].Height, MaxHeightSince);
          //  float normalizedLerpParameter = ((IncomingDistance - (Path[0].StartPos - Position)) / IncomingDistance);
          //  float resultHeight = Mathf.Lerp(MaxHeightSince, MaxHeightNext, normalizedLerpParameter);
          //  float a = Mathf.Atan((MaxHeightNext - resultHeight) / (Path[0].StartPos - Position));
          //  angle = -Mathf.Rad2Deg * a;
            return 0;
        }
        return Path[Path.Count - 1].Height;
    }
    public NODE_TYPE GetNodeType(float Position)
    {

        for (int i = 0; i < Path.Count - 1; i++)
        {
            if (Position >= Path[i].StartPos && Position < Path[i + 1].StartPos)
            {
                return Path[i].NodeType;
            }
        }
        return NODE_TYPE.LAST_BUILDING;
    }


}