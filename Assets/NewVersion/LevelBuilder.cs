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

    public GameObject PoolPrefab;

    [Header("General Settings")]
    public int PathLength = 6;

    public int Seed = 0;
    public float HoverHeight = 5f;
    public float HoledBuildingHoverHeight = 1f;
    public float LastBuildingHoverHeight = 0.3f;

    public float CharMovementExtent = 10f;

    public float PathSmoothingDistance = 10f;
    public float LastCannonDistance = 35f;
    public float CannonNozzleDistance = 10f;
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

    [Header("Wall")]
    public GameObject WallPrefab;
    public int WallPercentNew = 5;
    public int WallPercentSeries = 60;
    public int WallMaxSeries = 4;


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

    List<PathManager> Roads;

    int currentPathIndex = 0;
    CharController myChar;

    void Start()
    {
        BuildingBoxes = new Bounds[BuildingPrefabs.Length];
        for (int i = 0; i < BuildingBoxes.Length; i++)
        {
            BuildingBoxes[i] = BuildingPrefabs[i].GetComponentInChildren<MeshRenderer>().bounds;
        }
        layerno = LayerMask.NameToLayer("Building");
        Random.InitState(Seed);
        Roads = BuildMeALevel(2);
        myChar = CreateChar(Roads[currentPathIndex]);
        SwitchPath(myChar);
    }
    public PathManager NextPath()
    {
        currentPathIndex++;
        return SwitchPath(myChar);
    }

    public PathManager SwitchPath(CharController ch)
    {
        PathManager newPath = Roads[currentPathIndex];
        ch.PM = newPath;
        FindObjectOfType<StateManager>().CurrentPath = newPath;
        GameObject go = new GameObject("empty_target");
        if (newPath.EndCannon != null)
        {
            go.transform.position = newPath.EndCannon.transform.position;
            go.transform.rotation = newPath.EndCannon.transform.rotation;
            FindObjectOfType<CamControl>().AssignAwayCamObject(go.transform);
        }
        else
        {

            FindObjectOfType<CamControl>().AssignAwayCamObject(Pool.transform);
        }
        return newPath;
    }

    List<PathManager> BuildMeALevel(int pathCount)
    {
        List<PathManager> res = new List<PathManager>();
        Vector3 dir = Vector3.forward; //  (new Vector3(1,0,1)).normalized;
        PathManager PreviousPath = BuildLevel(Vector3.zero + 10 * Vector3.up, dir, true, null);
        res.Add(PreviousPath);
        for (int i = 1; i < pathCount; i++)
        {
            PreviousPath = res[res.Count - 1];
            res.Add(BuildLevel(PreviousPath.Origin + PreviousPath.Direction * PreviousPath.End + Vector3.up * (PreviousPath.GetHeight(PreviousPath.End)), -Vector3.Cross(PreviousPath.Direction, Vector3.up), false, PreviousPath));
        }
        for (int i = 0; i < pathCount; i++)
        {
            PathManager PM = res[i];
            RaycastHit[] hits;
            Ray r = new Ray(PM.Origin, PM.Direction);
            hits = Physics.RaycastAll(r, (2000f), LayerMask.GetMask("Building"));
            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }
            hits = Physics.RaycastAll(PM.Origin - Vector3.up * 3.4f - Vector3.left * 3.4f - Vector3.forward * 3.4f, PM.Direction, (2000f), LayerMask.GetMask("Building"));
            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }
            hits = Physics.RaycastAll(PM.Origin + Vector3.up * 3.4f + Vector3.left * 3.4f + Vector3.forward * 3.4f, PM.Direction, (2000f), LayerMask.GetMask("Building"));

            Debug.DrawRay(PM.Origin - Vector3.up * 3.4f - Vector3.left * 3.4f - Vector3.forward * 3.4f, (PM.Direction) * 2000f, Color.red, 400f);
            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }
            Vector3 endpos;
            if (i + 1 < res.Count)
            {
                if (PM.EndCannon == null)
                {
                    endpos = Pool.transform.position;
                }
                else
                    endpos = PM.EndCannon.transform.position;
            }
            else
            {
                endpos = Pool.transform.position;
            }

            hits = Physics.RaycastAll(endpos + Vector3.up * 0.4f + Vector3.left * 1.4f + Vector3.forward * 1.4f, -PM.Direction, (2000f), LayerMask.GetMask("Building"));

            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }

            hits = Physics.RaycastAll(endpos - Vector3.up * 0.4f - Vector3.left * 1.4f - Vector3.forward * 1.4f, -PM.Direction, (2000f), LayerMask.GetMask("Building"));

            Debug.DrawRay(endpos - Vector3.up * 0.4f - Vector3.left * 1.4f - Vector3.forward * 1.4f, (PM.Direction) * -2000f, Color.green, 400f);
            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }

            hits = Physics.RaycastAll(endpos + Vector3.up * 4f + Vector3.left * 2.2f + Vector3.forward * 2.2f, -PM.Direction, (2000f), LayerMask.GetMask("Building"));

            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }

            hits = Physics.RaycastAll(endpos - Vector3.up * 3f - Vector3.left * 3f - Vector3.forward * 3f, -PM.Direction, (2000f), LayerMask.GetMask("Building"));

            Debug.DrawRay(endpos + Vector3.up * 4f - Vector3.left * 2f - Vector3.forward * 2f, (PM.Direction) * -2000f, Color.green, 400f);
            foreach (RaycastHit rH in hits)
            {
                Destroy(rH.transform.gameObject);
            }
        }
        //var a = new GameObject("AAA");
        //a.transform.position = P.EndCannon.transform.position;
        return res;
    }

    CharController CreateChar(PathManager PM)
    {

        CannonControl Canon = PM.StartCannon;
        CharController Char = Instantiate(CharacterPrefab, Canon.CharacterSpawner).GetComponent<CharController>();
        Char.transform.SetParent(null, true);
        Char.LastCannonDistance = LastCannonDistance;
        Char.NozzleDistance = CannonNozzleDistance;
        Char.MoveExtent = CharMovementExtent;
        FindObjectOfType<StateManager>().CC = Char;
        FindObjectOfType<CamControl>().AssignCharToCam(Char.transform);
        FindObjectOfType<CharController>().GetComponent<Jump2TargetFinal>().TargetTransform = Pool.transform;
        return Char;
    }

    PathManager BuildLevel(Vector3 LevelOrigin, Vector3 LevelDirection, bool isCannonEnded, PathManager PreviousPath)
    {
        PathManager PM = new PathManager(PathLength);
        PM.Direction = LevelDirection;
        PM.Origin = LevelOrigin;
        PM.IncomingDistance = PathSmoothingDistance;
        Vector3 CurrentPosition = LevelOrigin;

        Vector3 BuildingScalarVector = RandomVector(MinSizingVector, MaxSizingVector);
        int RandIndex = Random.Range(0, BuildingPrefabs.Length);
        //Create The Cannon
        if (PreviousPath == null)
        {
            CannonControl Canon = Instantiate(CanonPrefab, LevelOrigin, Quaternion.LookRotation(LevelDirection, Vector3.up)).GetComponent<CannonControl>();
            PM.StartCannon = Canon;

            // CurrentPosition.z -= BuildingBoxes[RandIndex].size.z * BuildingScalarVector.z - CanonEdgeDistanceStartingPos;
            CurrentPosition -= CanonEdgeDistanceStartingPos * LevelDirection;
            CurrentPosition = AddBuildingFromTopPosition(CurrentPosition, RandIndex, LevelDirection, BuildingScalarVector, ref PM);
        }
        else
        {
            PM.StartCannon = PreviousPath.EndCannon;
            PreviousPath.EndCannon.newForward = LevelDirection;
            CurrentPosition += LevelDirection * 0.5f * (MinimumDistance + MaximumDistance);

        }
        //Add a building at a specific distance from the Cannon 
        //TODO: Fix this for all directions 
        CurrentPosition += LevelDirection * Random.Range(MinimumDistance, MaximumDistance);

        bool AddHoledBuildingInstead = false;
        float MaxHeight = LevelOrigin.y;
        for (int i = 0; i < PathLength; i++)
        {

            if (AddHoledBuildingInstead)
            {
                CurrentPosition = AddHoledBuildingFromTopPosition(CurrentPosition, LevelDirection, ref PM);
                AddHoledBuildingInstead = false;
            }
            else CurrentPosition = AddBuildingFromTopPosition(CurrentPosition, Random.Range(0, BuildingPrefabs.Length), LevelDirection, BuildingScalarVector, ref PM);

            //Give some space
            CurrentPosition += LevelDirection * (Random.Range(MinimumDistance, MaximumDistance) * 1f);
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
                        if (i >= 2 && i < PathLength - 2)
                            if (PM.Path[PM.Path.Count - 1].NodeType != PathManager.NODE_TYPE.HOLED_BUILDING || PM.Path[PM.Path.Count - 2].NodeType != PathManager.NODE_TYPE.HOLED_BUILDING)
                            { AddHoledBuildingInstead = true; }
                    }
                }
                CurrentPosition += multiplier * Vector3.up * Random.Range(MinimumStepSize, MaximumStepSize);
                if ((AddHoledBuildingInstead && CurrentPosition.y < MaxHeight)) { CurrentPosition.y = MaxHeight; }
                MaxHeight = Mathf.Max(MaxHeight, CurrentPosition.y);
            }

            if (i >= PathLength - 2)
            {
                MaxHeight = CurrentPosition.y;
            }
            BuildingScalarVector = RandomVector(MinSizingVector, MaxSizingVector);
        }
        PathManager.PathNode LastBuilding = (PM.Path[PM.Path.Count - 1]);
        LastBuilding.NodeType = PathManager.NODE_TYPE.LAST_BUILDING;
        LastBuilding.Height -= (HoverHeight - LastBuildingHoverHeight);
        PM.Path[PM.Path.Count - 1] = LastBuilding;
        float pos = Mathf.Lerp(LastBuilding.StartPos, LastBuilding.EndPos, 0.5f);
        PM.End = pos;
        Vector3 FinalCannonPos = LevelOrigin + LevelDirection * pos;
        FinalCannonPos.y = LevelOrigin.y + LastBuilding.Height - LastBuildingHoverHeight; //- HoverHeight;
        if (isCannonEnded)
        {
            GameObject LastCannon = Instantiate(CanonPrefab, FinalCannonPos, Quaternion.Euler(0, 180f, 0) * Quaternion.LookRotation(LevelDirection, Vector3.up));
            LastCannon.transform.localScale *= 3f;
            PM.EndCannon = LastCannon.GetComponent<CannonControl>();
        }
        else
        {

            Pool = Instantiate(PoolPrefab, FinalCannonPos + 40 * Vector3.down + 30 * LevelDirection, Quaternion.Euler(0, 180f, 0) * Quaternion.LookRotation(LevelDirection, Vector3.up));
            Destroy(LastB);
            //PM.EndCannon = LastCannon.GetComponent<CannonControl>();
        }
        GenerateCoinsAndRings(ref PM);
        GenerateWalls(ref PM);
        AddBackgroundBuildings(LevelDirection, LevelOrigin, PM);
        return PM;
    }

    GameObject LastB, Pool;
    void GenerateCoinsAndRings(ref PathManager PM)
    {
        float Length = PM.End - LastCannonDistance;
        float Progress = MinDistanceFromFirstCoin;
        Vector3 side = Vector3.Cross(Vector3.up, PM.Direction);

        float LastMovement = Random.Range(-CharMovementExtent, CharMovementExtent);

        int SeriesCounter = 0;
        int LastRing = 0;
        int coinSideDirection = 0;



        while (Progress < Length)
        {

            GameObject coin = null;
            bool holed = (PM.GetNodeType(Progress) == PathManager.NODE_TYPE.HOLED_BUILDING);
            if (SeriesCounter == 0 && Random.Range(0, 100) < NewCoinSeriesPercentage)
            {
                if (holed) LastMovement = 0;
                Vector3 newMove = LastMovement * side;
                coin = Instantiate(CoinPrefab, PM.Origin + (Vector3.up * (PM.GetHeight(Progress)) + (Progress * PM.Direction) + newMove), Quaternion.identity);

                SeriesCounter++;
            }
            else if (SeriesCounter > 0)
            {
                if (coinSideDirection != 0)
                {
                    if (Random.Range(0, 100) < CoinSideSpreadKeepDirectionPercentage)
                    {

                        LastMovement += (coinSideDirection) * CoinSideSpreadStep;
                        if (LastMovement < -CharMovementExtent || LastMovement > CharMovementExtent)
                        {
                            LastMovement -= 2 * coinSideDirection * CoinSideSpreadStep;
                            coinSideDirection *= -1;
                        }
                    }
                    else
                    {
                        coinSideDirection = 0;
                        //LastMovementKalır
                    }
                }
                else if (Random.Range(0, 100) < CoinSideSpreadPercentage)
                {
                    coinSideDirection = (Random.Range(0f, 1f) < 0.5f) ? (-1) : (1);
                    LastMovement += (coinSideDirection) * CoinSideSpreadStep;

                    if (LastMovement < -CharMovementExtent || LastMovement > CharMovementExtent)
                    {
                        LastMovement -= 2 * coinSideDirection * CoinSideSpreadStep;
                        coinSideDirection *= -1;
                    }
                }
                if (SeriesCounter <= CoinIdealSeriesCount)
                {
                    if (Random.Range(0, 100) < HighCoinPercentageToContinueSeries)
                    {

                        if (holed) LastMovement = 0;
                        Vector3 newMove = LastMovement * side;
                        coin = Instantiate(CoinPrefab, PM.Origin + (Vector3.up * (PM.GetHeight(Progress)) + (Progress * PM.Direction) + newMove), Quaternion.identity);
                        SeriesCounter++;
                    }
                    else
                    {
                        SeriesCounter = 0;
                        LastRing = SeriesCounter;
                        coinSideDirection = 0;
                        LastMovement = Random.Range(-CharMovementExtent, CharMovementExtent);
                    }
                }
                else
                {
                    if (Random.Range(0, 100) < LowCoinPercentageToContinueSeries)
                    {

                        if (holed) LastMovement = 0;

                        Vector3 newMove = LastMovement * side;
                        coin = Instantiate(CoinPrefab, PM.Origin + (Vector3.up * (PM.GetHeight(Progress)) + (Progress * PM.Direction) + newMove), Quaternion.identity);
                        SeriesCounter++;
                    }
                    else
                    {
                        SeriesCounter = 0;
                        LastRing = SeriesCounter;
                        coinSideDirection = 0;
                        LastMovement = Random.Range(-CharMovementExtent, CharMovementExtent);
                    }
                }

            }

            if (SeriesCounter > 0 && PM.GetNodeType(Progress) != PathManager.NODE_TYPE.LAST_BUILDING)
            {
                if (((SeriesCounter - LastRing > OneRingPerHowManyCoinsMin) || (SeriesCounter <= OneRingPerHowManyCoinsMin && LastRing == 0)) && Random.Range(0, 100) < RingChancePercentage)
                {

                    if (holed) LastMovement = 0;
                    Vector3 newMove = LastMovement * side;
                    Instantiate(RingPrefab, PM.Origin + (Vector3.up * (PM.GetHeight(Progress)) + (Progress * PM.Direction) + newMove), Quaternion.identity).transform.rotation = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, PM.Direction), 0);
                    LastRing = SeriesCounter;
                }

            }
            if (coin != null)
            {
                coin.transform.rotation = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, PM.Direction), 0);
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


    void GenerateWalls(ref PathManager PM)
    {
        float Length = PM.End - LastCannonDistance;
        float Progress = MinDistanceFromFirstCoin;
        Vector3 side = Vector3.Cross(Vector3.up, PM.Direction);
        float LastMovement = CharMovementExtent;
        int SeriesCounter = 0;
        int LastRing = 0;
        int WallDirection = 0;
        float WallSize = WallPrefab.GetComponent<BoxCollider>().size.z;
        Vector3 sideDirBase = 1.25f * LastMovement * side;
        Vector3 sideDir = sideDirBase;
        int mul = 1;
        float a = -999;
        int b = 0;

        float a2 = -999;
        int b2 = 0;
        while (Progress < Length)
        {
            if (SeriesCounter == 0 && Random.Range(0, 100) < WallPercentNew)
            {
                mul = ((Random.Range(0f, 1f) < 0.5f) ? (-1) : (1));
                sideDir = sideDirBase * mul;
                float angle, angle2; bool hole, hole2;
                float d = PM.GetHeight(Progress, ref a, ref b, out angle, out hole);
                float e = PM.GetHeight(Progress + WallSize, ref a2, ref b2, out angle2, out hole2);

                if (angle != 0 || angle2 != 0 || hole || hole2)
                {
                    Progress += WallSize;
                    continue;
                }
                Instantiate(WallPrefab, PM.Origin + Vector3.up * d + sideDir + Progress * PM.Direction, Quaternion.identity).transform.rotation = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, PM.Direction), mul * 90f - 90f); ;
                PathManager.WallNode wn = new PathManager.WallNode();
                wn.StartPos = Progress;
                wn.EndPos = Progress + WallSize;
                wn.SideMultipliers = mul;
                PM.Walls.Add(wn);
                SeriesCounter++;
            }
            else if (SeriesCounter > 0)
            {
                if (Random.Range(0, 100) < WallPercentSeries && SeriesCounter <= WallMaxSeries)
                {
                    float angle, angle2;
                    bool hole, hole2;
                    float d = PM.GetHeight(Progress, ref a, ref b, out angle, out hole);
                    float e = PM.GetHeight(Progress + WallSize, ref a2, ref b2, out angle2, out hole2);
                    if (angle != 0 || angle2 != 0 || hole || hole2)
                    {
                        Progress += WallSize;
                        SeriesCounter = 0;
                        continue;
                    }
                    Instantiate(WallPrefab, PM.Origin + Vector3.up * PM.GetHeight(Progress) + sideDir + Progress * PM.Direction, Quaternion.identity).transform.rotation = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, PM.Direction), mul * 90f - 90f); ;
                    PathManager.WallNode wn = PM.Walls[PM.Walls.Count - 1];
                    wn.EndPos += WallSize;
                    PM.Walls[PM.Walls.Count - 1] = wn;
                    SeriesCounter++;
                }
                else
                {
                    SeriesCounter = 0;
                }
            }
            Progress += WallSize;
        }

    }


    int layerno;

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
                CurPos.y = PM.Origin.y + PM.GetHeight(Vector3.Dot(CurPos, LevelDirection)) + Random.Range(DecorativeBuildingExtraHeightMinMax.x, DecorativeBuildingExtraHeightMinMax.y);
                int index = Random.Range(0, BuildingPrefabs.Length);
                GameObject BaseBuilding = Instantiate(BuildingPrefabs[index], CurPos, Quaternion.identity);
                SetLayerRecursively(BaseBuilding, layerno);
                Quaternion rot = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, LevelDirection), 0);
                BaseBuilding.transform.rotation = rot;
                Vector3 newSize = BuildingBoxes[index].size;
                Vector3 newScale = BaseBuilding.transform.localScale;
                newScale = ScaleWithScalarVector(scalar, newScale);
                newSize = ScaleWithScalarVector(scalar, newSize);
                BaseBuilding.transform.localScale = newScale;
                CurPos += (Vector3.Dot(LevelDirection, rot * newSize) + Random.Range(DecorativeBuildingSpacingMinMax.x, DecorativeBuildingSpacingMinMax.y)) * LevelDirection;
                CurPos -= (side * sideDist);
            }

        }

    }
    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
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
        Debug.Log("P1:" + Pos.y);
        GameObject BaseBuilding = Instantiate(BuildingPrefabs[index], Pos, Quaternion.identity);
        Vector3 newScale = BaseBuilding.transform.localScale;
        //newScale.z *= (BuildingBoxes[index].size.x/BuildingBoxes[index].size.z); //Make it square prism
        Vector3 newSize = BuildingBoxes[index].size;
        newScale = ScaleWithScalarVector(scalar, newScale);
        newSize = ScaleWithScalarVector(scalar, newSize);
        BaseBuilding.transform.localScale = newScale;
        Quaternion rot = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, LevelDirection), 0);
        BaseBuilding.transform.rotation = rot;
        Vector3 FinalPos = Pos + Mathf.Abs(Vector3.Dot(LevelDirection, rot * newSize)) * LevelDirection;
        PathManager.PathNode Node;
        Node.Height = FinalPos.y + HoverHeight - PM.Origin.y;
        Node.EndPos = Vector3.Dot(FinalPos, LevelDirection);
        Node.StartPos = Vector3.Dot(Pos, LevelDirection);
        Node.NodeType = PathManager.NODE_TYPE.REGULAR_BUILDING;
        PM.Path.Add(Node);
        LastB = BaseBuilding;
        return FinalPos;
    }
    Vector3 AddHoledBuildingFromTopPosition(Vector3 Pos, Vector3 LevelDirection, ref PathManager PM)
    {
        GameObject BaseBuilding = Instantiate(HoledBuildingPrefab, Pos, Quaternion.identity);

        Quaternion rot = Quaternion.Euler(0, Vector3.Angle(Vector3.forward, LevelDirection), 0);
        BaseBuilding.transform.rotation = rot;
        Vector3 FinalPos = Pos + Mathf.Abs(Vector3.Dot(LevelDirection, rot * HoledBuildingPrefab.GetComponentInChildren<MeshRenderer>().bounds.size)) * LevelDirection;
        PathManager.PathNode Node;
        Node.Height = FinalPos.y + HoledBuildingHoverHeight - PM.Origin.y;
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
