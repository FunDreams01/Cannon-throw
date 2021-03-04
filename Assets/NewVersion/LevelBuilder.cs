using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] BuildingPrefabs;
    public GameObject HoledBuildingPrefab;
    public GameObject CanonPrefab;
    public GameObject CoinPrefab;
    public GameObject RingPrefab;
    public GameObject CharacterPrefab;

    [Header("General Settings")]
    public int PathLength = 6;

    public int Seed = 0;
    public float HoverHeight = 5f;
    public float HoledBuildingHoverHeight = 1f;

    public float CharMovementExtent = 10f;

    public float PathSmoothingDistance = 10f;     
    [Header("Coin Settings")]
    public Vector2Int CoinInSeriesMinMax = new Vector2Int(6, 16);
    public int NewCoinSeriesPercentage = 4;
    public int OneRingPerHowManyCoinsMin = 3;
    public int RingChancePercentage = 40;
    public int HighCoinPercentageToContinueSeries = 60;
    public int LowCoinPercentageToContinueSeries = 15;
    public int CoinIdealSeriesCount = 14;
    public float CoinDistance = 2f;
    public float MinDistanceFromFirstCoin = 20f;
    public float CoinSideSpreadStep = 1f;
    public int CoinSideSpreadPercentage = 50;
    public int CoinSideSpreadKeepDirectionPercentage = 80;

    [Header("Main Building Settings")]
    public float MinimumDistance = 5f;
    public float MaximumDistance = 25f;

    public float MinimumStepSize = 4f;
    public float MaximumStepSize = 25f;
    public int StepPercentage = 65;

    public int UpStepChance = 70;
    public int HoledBuildingChanceIfUpStep = 90;
    public int MinNoOfBuildingsBeforeHoledBuilding = 3;
    public Vector3 MinSizingVector = new Vector3(1f, 0.7f, 0.55f);
    public Vector3 MaxSizingVector = new Vector3(1.7f, 1.3f, 1.2f);
    [Header("Decorative Building Settings")]
    public float DecorativeBuildingDistance = 80f;
    public Vector3 DecorativeBuildingSizingMin = new Vector3(1.3f, 1.45f, 0.35f);
    public Vector3 DecorativeBuildingSizingMax = new Vector3(2.3f, 3.6f, 0.9f);
    public Vector2 DecorativeBuildingExtraHeightMinMax = new Vector2(20f, 60f);
    public Vector2 DecorativeBuildingSpacingMinMax = new Vector2(5f, 40f);
    public int LayersOfDecorationOnOneSide = 2;
    public float DecorativeBuildingLayerSpacing = 30f;

    public float DecorativeBuildingRotationPercentage = 20f;

    public float DecorativeBuildingSideOffsetMax = 20f;

    [Header("Cannon Setting")]
    public float CanonEdgeDistanceStartingPos = 10f;

    Bounds[] BuildingBoxes;

    void Start()
    {
        BuildingBoxes = new Bounds[BuildingPrefabs.Length];
        for (int i = 0; i < BuildingBoxes.Length; i++)
        {
            BuildingBoxes[i] = BuildingPrefabs[i].GetComponentInChildren<MeshRenderer>().bounds;
        }
        BuildLevel(Vector3.zero, Vector3.forward);
    }
    PathManager BuildLevel(Vector3 LevelOrigin, Vector3 LevelDirection)
    {
        Random.InitState(Seed);
        PathManager PM = new PathManager(PathLength);
        PM.Direction = LevelDirection;
        PM.Origin = LevelOrigin;
        PM.IncomingDistance = PathSmoothingDistance;
        Vector3 CurrentPosition = LevelOrigin;
        Vector3 BuildingScalarVector = RandomVector(MinSizingVector, MaxSizingVector);
        //Create The Cannon
        CannonControl Canon = Instantiate(CanonPrefab, LevelOrigin, Quaternion.LookRotation(LevelDirection, Vector3.up)).GetComponent<CannonControl>();
        PM.StartCannon = Canon;
        CharController Char = Instantiate(CharacterPrefab, Canon.CharacterSpawner).GetComponent<CharController>();
        Char.transform.SetParent(null,true);
        Char.PM = PM;
        Char.MoveExtent = CharMovementExtent;
        FindObjectOfType<StateManager>().CurrentPath = PM;
        FindObjectOfType<StateManager>().CC = Char;
        FindObjectOfType<CamControl>().AssignCharToCam();
        //Add a building at a specific distance from the Cannon 
        int RandIndex = Random.Range(0, BuildingPrefabs.Length);
        CurrentPosition.z -= BuildingBoxes[RandIndex].size.z * BuildingScalarVector.z - CanonEdgeDistanceStartingPos;
        CurrentPosition = AddBuildingFromTopPosition(CurrentPosition, RandIndex, LevelDirection, BuildingScalarVector, ref PM);
        CurrentPosition += LevelDirection * Random.Range(MinimumDistance, MaximumDistance);

        bool AddHoledBuildingInstead = false;
        float MaxHeight = -999f;
        for (int i = 0; i < PathLength; i++)
        {

            if (AddHoledBuildingInstead)
            {
                CurrentPosition = AddHoledBuildingFromTopPosition(CurrentPosition, LevelDirection, ref PM);
                AddHoledBuildingInstead = false;
            }
            else CurrentPosition = AddBuildingFromTopPosition(CurrentPosition, Random.Range(0, BuildingPrefabs.Length), LevelDirection, BuildingScalarVector, ref PM);

            //Give some space
            CurrentPosition += LevelDirection * Random.Range(MinimumDistance, MaximumDistance);
            //Height change?
            if (Random.Range(0, 100) < StepPercentage)
            {
                //Up or down?
                int multiplier = -1;
                if (Random.Range(0, 100) < UpStepChance)
                {
                    multiplier *= -1;
                    if (i >= MinNoOfBuildingsBeforeHoledBuilding && Random.Range(0, 100) < HoledBuildingChanceIfUpStep)
                    {
                        if(i>=2)
                        if(PM.Path[PM.Path.Count-1].NodeType != PathManager.NODE_TYPE.HOLED_BUILDING  || PM.Path[PM.Path.Count-2].NodeType != PathManager.NODE_TYPE.HOLED_BUILDING )
                        AddHoledBuildingInstead = true;
                    }
                }
                CurrentPosition += multiplier * Vector3.up * Random.Range(MinimumStepSize, MaximumStepSize);
                if (AddHoledBuildingInstead && CurrentPosition.y < MaxHeight)
                    CurrentPosition.y = MaxHeight;
                MaxHeight = Mathf.Max(MaxHeight, CurrentPosition.y);
            }

            BuildingScalarVector = RandomVector(MinSizingVector, MaxSizingVector);
        }

        GenerateCoinsAndRings(LevelDirection, ref PM);
        AddBackgroundBuildings(LevelDirection, LevelOrigin, PM);


        return PM;
    }

    void GenerateCoinsAndRings(Vector3 LevelDirection, ref PathManager PM)
    { 
        float Length = PM.Path[PM.Path.Count - 1].EndPos;
        float Progress = MinDistanceFromFirstCoin;
        Vector3 side = Vector3.Cross(Vector3.up,LevelDirection);

        float LastMovement = Random.Range(-CharMovementExtent,CharMovementExtent);

        int SeriesCounter = 0;
        int LastRing = 0;
        int coinSideDirection = 0;
        
        
        
        while(Progress < Length)
        {

            bool holed = (PM.GetNodeType(Progress) == PathManager.NODE_TYPE.HOLED_BUILDING);
                    if(SeriesCounter == 0 && Random.Range(0,100) < NewCoinSeriesPercentage)
                    {
                            if(holed) LastMovement = 0;
                    Vector3 newMove = LastMovement * side;
                    Instantiate(CoinPrefab, Vector3.up * (PM.GetHeight(Progress)) + (Progress * LevelDirection) + newMove, Quaternion.identity);
                    
                    SeriesCounter++;
                    }
                    else if(SeriesCounter > 0)
                    {

                    if(coinSideDirection != 0)
                    {
                        if(Random.Range(0,100) < CoinSideSpreadKeepDirectionPercentage)
                        {
                        LastMovement += (coinSideDirection) * CoinSideSpreadStep;
                        }
                        else
                        {
                            coinSideDirection = 0;
                            //LasTMovementKalır
                        }
                    }
                    else if(Random.Range(0,100) < CoinSideSpreadPercentage)
                    {
                        coinSideDirection =  (Random.Range(0f,1f) < 0.5f)? (-1) : (1);
                        LastMovement += (coinSideDirection) * CoinSideSpreadStep;
                    }
                        if(SeriesCounter <= CoinIdealSeriesCount)
                        {
                             if (Random.Range(0,100) < HighCoinPercentageToContinueSeries)
                             {
                                 
                            if(holed) LastMovement = 0;
                    Vector3 newMove = LastMovement * side;
                    Instantiate(CoinPrefab, Vector3.up * (PM.GetHeight(Progress)) + (Progress * LevelDirection) + newMove, Quaternion.identity);
                                SeriesCounter++;
                             }
                             else {
                                 SeriesCounter = 0;
                                 LastRing = SeriesCounter;
                                 coinSideDirection = 0;
        LastMovement = Random.Range(-CharMovementExtent,CharMovementExtent);
                             }
                        }
                        else
                        {
                             if (Random.Range(0,100) < LowCoinPercentageToContinueSeries)
                             {
                                 
                            if(holed) LastMovement = 0;
                    Vector3 newMove = LastMovement * side;
                    Instantiate(CoinPrefab, Vector3.up * (PM.GetHeight(Progress)) + (Progress * LevelDirection) + newMove, Quaternion.identity);
                                SeriesCounter++;
                             }
                             else {
                                 SeriesCounter = 0;
                                 LastRing = SeriesCounter;
                                 coinSideDirection = 0;
        LastMovement = Random.Range(-CharMovementExtent,CharMovementExtent);
                             }
                        }

                    }

                    if(SeriesCounter > 0)
                    {
                        if(((SeriesCounter - LastRing > OneRingPerHowManyCoinsMin) || (SeriesCounter <= OneRingPerHowManyCoinsMin && LastRing == 0)) && Random.Range(0,100) < RingChancePercentage)
                        {
                            
                            if(holed) LastMovement = 0;
                    Vector3 newMove = LastMovement * side;
                            Instantiate(RingPrefab, Vector3.up * (PM.GetHeight(Progress)) + (Progress * LevelDirection) + newMove, Quaternion.identity);
                            LastRing = SeriesCounter;
                        }

                    }
                    
                   Progress += CoinDistance;
                   
        }
        /* 
           float ChunkSize = ((Length - MinDistanceFromFirstCoin) / CoinSeriesCount);
           int LastCoinJ = 0;
           float ChunkOverflow = 0;
           for (int i = 0; i < CoinSeriesCount; i++)
           {
               LastCoinJ = 0;
               float StartProgress = Random.Range(i * ChunkSize, (i + 1) * ChunkSize) + MinDistanceFromFirstCoin + ChunkOverflow;
               ChunkOverflow = 0;
               int CoinCount = Random.Range(CoinInSeriesMinMax.x, CoinInSeriesMinMax.y);
               for (int j = 0; j < CoinCount; j++)
               {
                   float CoinPos = StartProgress + j * CoinDistance;
                   if (CoinPos >= Length) { Debug.Log("Exceed!"); return; }
                   if (CoinPos < MinDistanceFromFirstCoin) { Debug.Log("Exceed222222!"); }
                   if (CoinPos > (i + 1) * ChunkSize) { ChunkOverflow = CoinPos - (i + 1) * ChunkSize; }
                   Instantiate(CoinPrefab, Vector3.up * (PM.GetHeight(CoinPos)) + (CoinPos * LevelDirection), Quaternion.identity);
                   if (j - LastCoinJ > OneRingPerHowManyCoinsMin && Random.Range(0, 100) < RingChancePercentage)
                   {
                       LastCoinJ = j;
                       Instantiate(RingPrefab, Vector3.up * (PM.GetHeight(CoinPos)) + (CoinPos * LevelDirection), Quaternion.identity);
                   }
               }
           }
   */

    }

    void AddBackgroundBuildings(Vector3 LevelDirection, Vector3 LevelOrigin, PathManager PM)
    {
        //Do a cross product to find the side vector
        Vector3 side = Vector3.Cross(Vector3.up, LevelDirection);
        float Length = PM.Path[PM.Path.Count - 1].EndPos;
        List<Vector3> Origins = new List<Vector3>(2 * LayersOfDecorationOnOneSide);
        for (int i = 1; i <= LayersOfDecorationOnOneSide; i++)
        {
            Origins.Add(LevelOrigin + (side * (DecorativeBuildingDistance + (i - 1) * DecorativeBuildingLayerSpacing)));
            Origins.Add(LevelOrigin - (side * (DecorativeBuildingDistance + (i - 1) * DecorativeBuildingLayerSpacing)));
        }

        foreach (Vector3 Origin in Origins)
        {
            Vector3 scalar = RandomVector(DecorativeBuildingSizingMin, DecorativeBuildingSizingMax);
            Vector3 CurPos = Origin;
            while (Vector3.Dot(CurPos, LevelDirection) < Length)
            {
                float sideDist = Random.Range(-DecorativeBuildingSideOffsetMax, DecorativeBuildingSideOffsetMax);
                CurPos += (side * sideDist);
                CurPos.y = PM.GetHeight(Vector3.Dot(CurPos, LevelDirection)) + Random.Range(DecorativeBuildingExtraHeightMinMax.x, DecorativeBuildingExtraHeightMinMax.y);
                int index = Random.Range(0, BuildingPrefabs.Length);
                GameObject BaseBuilding = Instantiate(BuildingPrefabs[index], CurPos, Quaternion.identity);
                Vector3 newSize = BuildingBoxes[index].size;
                Vector3 newScale = BaseBuilding.transform.localScale;
                newScale = ScaleWithScalarVector(scalar, newScale);
                newSize = ScaleWithScalarVector(scalar, newSize);
                BaseBuilding.transform.localScale = newScale;
                CurPos += (Vector3.Dot(LevelDirection, newSize) + Random.Range(DecorativeBuildingSpacingMinMax.x, DecorativeBuildingSpacingMinMax.y)) * LevelDirection;
                CurPos -= (side * sideDist);
            }

        }

    }
    static Vector3 RandomVector(Vector3 Min, Vector3 Max)
    {
        Vector3 rand = Vector3.one;
        rand.x = Random.Range(Min.x, Max.x);
        rand.y = Random.Range(Min.y, Max.y);
        rand.z = Random.Range(Min.z, Max.z);
        return rand;
    }

    Vector3 AddBuildingFromTopPosition(Vector3 Pos, int index, Vector3 LevelDirection, Vector3 scalar, ref PathManager PM)
    {
        //These were added supposing the origin was at the bottom. Changed it after to make things cleaner.
        //Vector3 TopOrigin = Pos;
        //TopOrigin.y -= BuildingBoxes[index].size.y*scalar.y;
        GameObject BaseBuilding = Instantiate(BuildingPrefabs[index], Pos, Quaternion.identity);
        Vector3 newScale = BaseBuilding.transform.localScale;
        //newScale.z *= (BuildingBoxes[index].size.x/BuildingBoxes[index].size.z); //Make it square prism
        Vector3 newSize = BuildingBoxes[index].size;
        newScale = ScaleWithScalarVector(scalar, newScale);
        newSize = ScaleWithScalarVector(scalar, newSize);
        BaseBuilding.transform.localScale = newScale;
        Vector3 FinalPos = Pos + Vector3.Dot(LevelDirection, newSize) * LevelDirection;
        PathManager.PathNode Node;
        Node.Height = FinalPos.y + HoverHeight;
        Node.EndPos = Vector3.Dot(FinalPos, LevelDirection);
        Node.StartPos = Vector3.Dot(Pos, LevelDirection);
        Node.NodeType = PathManager.NODE_TYPE.REGULAR_BUILDING;
        PM.Path.Add(Node);
        return FinalPos;
    }
    Vector3 AddHoledBuildingFromTopPosition(Vector3 Pos, Vector3 LevelDirection, ref PathManager PM)
    {

        GameObject BaseBuilding = Instantiate(HoledBuildingPrefab, Pos, Quaternion.identity);
        Vector3 FinalPos = Pos + Vector3.Dot(LevelDirection, HoledBuildingPrefab.GetComponentInChildren<MeshRenderer>().bounds.size) * LevelDirection;
        PathManager.PathNode Node;
        Node.Height = FinalPos.y + HoledBuildingHoverHeight;
        Node.EndPos = Vector3.Dot(FinalPos, LevelDirection);
        Node.StartPos = Vector3.Dot(Pos, LevelDirection);
        Node.NodeType = PathManager.NODE_TYPE.HOLED_BUILDING;
        PM.Path.Add(Node);
        return FinalPos;

    }

    static Vector3 ScaleWithScalarVector(Vector3 scalar, Vector3 OriginalVector)
    {
        Vector3 res = OriginalVector;
        res.x *= scalar.x;
        res.y *= scalar.y;
        res.z *= scalar.z;
        return res;
    }
}
