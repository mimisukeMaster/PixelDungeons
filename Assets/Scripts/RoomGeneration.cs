using UnityEngine;
using System.Collections.Generic;

//5x5のグリッドを作り、中心以外のマスにスタートを置く。(goalDistance)マス以上移動したところにゴールを置く
//スタートの位置から
public class RoomGeneration : MonoBehaviour
{
    public Material roomMaterial;//戦闘部屋のマテリアル
    public Material startMaterial;//スタート部屋のマテリアル
    public Material goalMaterial;//ゴール部屋のマテリアル
    public Material potentialGoalMaterial;//ゴール部屋候補のマテリアル
    public int x;//部屋の数（横）
    public int y;//部屋の数（縦）
    public int goalMinDistance = 5;//ゴールまでの最短距離
    public int maxRooms;//戦闘部屋の最大数
    public bool generateOnStart = false;//OnStart()で部屋を生成する

    private List<GameObject> cubes = new List<GameObject>();//部屋の区画

    public GameObject playerPrefab;//プレイヤーのプレハブ
    private GameObject playerInstance;//インスタンス化したプレイヤー

    public GameObject[] room;//戦闘部屋の候補
    public GameObject wall;//生成する壁のプレハブ
    public GameObject door;//生成するドアのプレハブ
    private List<GameObject> walls = new List<GameObject>();//生成された壁化ドア
    public Transform roomParent;//部屋を生成するときの親オブジェクト

    private void Start()
    {
        if (generateOnStart)
        {
            Generate();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    //部屋を生成する
    public void Generate()
    {
        //前生成されたものを消す
        if (cubes.Count != 0)
        {
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
            cubes.Clear();
        }
        if (walls.Count != 0)
        {
            foreach (GameObject wall in walls)
            {
                Destroy(wall);
            }
            walls.Clear();
        }

        //迷路を生成する
        MapCoordinate mapCoordinate = new MapCoordinate(x, y, goalMinDistance, maxRooms);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //作られた迷路の通りに部屋を生成する
                GameObject room_ = Instantiate(room[Random.Range(0, room.Length)], roomParent);
                room_.transform.position = new Vector3(i * 50, 0, j * 50);
                switch (mapCoordinate.coordinate[i, j])
                {
                    case 0://何もない場合
                        Destroy(room_);
                        break;
                    case 1://戦闘部屋の場合
                        cubes.Add(room_);
                        room_.GetComponent<MeshRenderer>().material = roomMaterial;
                        SetWall(i, j, mapCoordinate);
                        break;
                    case 2://スタート部屋の場合
                        cubes.Add(room_);
                        room_.GetComponent<MeshRenderer>().material = startMaterial;
                        SetWall(i, j, mapCoordinate);
                        if (playerInstance != null)
                        {
                            Destroy(playerInstance);
                        }
                        playerInstance = Instantiate(playerPrefab, room_.transform.position, Quaternion.identity);
                        break;
                    case 3://ゴール部屋の場合
                        cubes.Add(room_);
                        room_.GetComponent<MeshRenderer>().material = goalMaterial;
                        SetWall(i, j, mapCoordinate);
                        break;
                    case 4://ゴール候補の場合
                        Destroy(room_);
                        break;
                }
            }
        }
    }

    public void SetWall(int x, int y, MapCoordinate mapCoordinate)
    {
        //右
        if (x != this.x - 1)//coordinateが範囲を超えないようにする
        {
            //隣に何もないかゴール候補だったら壁を置き、それ以外だったら壁を置く
            if (mapCoordinate.coordinate[x + 1, y] != 0 && mapCoordinate.coordinate[x + 1, y] != 4)
            {
                GameObject wall_ = Instantiate(door, new Vector3(x * 50 + 25, 0, y * 50), Quaternion.Euler(0, 90, 0), roomParent);
                wall_.name = $"door_{x}_{y}_Rignt";
                walls.Add(wall_);
            }
            else
            {
                GameObject wall_ = Instantiate(wall, new Vector3(x * 50 + 25, 0, y * 50), Quaternion.Euler(0, 90, 0), roomParent);
                wall_.name = $"wall_{x}_{y}_Rignt";
                walls.Add(wall_);
            }
        }
        else//端だったら壁を置く
        {
            GameObject wall_ = Instantiate(wall, new Vector3(x * 50 + 25, 0, y * 50), Quaternion.Euler(0, 90, 0), roomParent);
            wall_.name = $"wall_{x}_{y}_Rignt";
            walls.Add(wall_);
        }
        //以下ほぼ同じ
        //左
        if (x != 0)
        {
            if (mapCoordinate.coordinate[x - 1, y] != 0 && mapCoordinate.coordinate[x - 1, y] != 4)
            {
                GameObject wall_ = Instantiate(door, new Vector3(x * 50 - 25, 0, y * 50), Quaternion.Euler(0, -90, 0), roomParent);
                wall_.name = $"door_{x}_{y}_Left";
                walls.Add(wall_);
            }
            else
            {
                GameObject wall_ = Instantiate(wall, new Vector3(x * 50 - 25, 0, y * 50), Quaternion.Euler(0, -90, 0), roomParent);
                wall_.name = $"wall_{x}_{y}_Left";
                walls.Add(wall_);
            }
        }
        else
        {
            GameObject wall_ = Instantiate(wall, new Vector3(x * 50 - 25, 0, y * 50), Quaternion.Euler(0, -90, 0), roomParent);
            wall_.name = $"wall_{x}_{y}_Left";
            walls.Add(wall_);
        }
        //前
        if (y != this.y - 1)
        {
            if (mapCoordinate.coordinate[x, y + 1] != 0 && mapCoordinate.coordinate[x, y + 1] != 4)
            {
                GameObject wall_ = Instantiate(door, new Vector3(x * 50, 0, y * 50 + 25), Quaternion.Euler(0, 0, 0), roomParent);
                wall_.name = $"door_{x}_{y}_Foward";
                walls.Add(wall_);
            }
            else
            {
                GameObject wall_ = Instantiate(wall, new Vector3(x * 50, 0, y * 50 + 25), Quaternion.Euler(0, 0, 0), roomParent);
                wall_.name = $"wall_{x}_{y}_Foward";
                walls.Add(wall_);
            }
        }
        else
        {
            GameObject wall_ = Instantiate(wall, new Vector3(x * 50, 0, y * 50 + 25), Quaternion.Euler(0, 0, 0), roomParent);
            wall_.name = $"wall_{x}_{y}_Foward";
            walls.Add(wall_);
        }
        //後
        if (y != 0)
        {
            if (mapCoordinate.coordinate[x, y - 1] != 0 && mapCoordinate.coordinate[x, y - 1] != 4)
            {
                GameObject wall_ = Instantiate(door, new Vector3(x * 50, 0, y * 50 - 25), Quaternion.Euler(0, 180, 0), roomParent);
                wall_.name = $"door_{x}_{y}_Back";
                walls.Add(wall_);
            }
            else
            {
                GameObject wall_ = Instantiate(wall, new Vector3(x * 50, 0, y * 50 - 25), Quaternion.Euler(0, 180, 0), roomParent);
                wall_.name = $"wall_{x}_{y}_Back";
                walls.Add(wall_);
            }
        }
        else
        {
            GameObject wall_ = Instantiate(wall, new Vector3(x * 50, 0, y * 50 - 25), Quaternion.Euler(0, 180, 0), roomParent);
            wall_.name = $"wall_{x}_{y}_Back";
            walls.Add(wall_);
        }
    }
}

public struct MapCoordinate
{
    public int[,] coordinate;//0:何もなし、1:戦闘部屋、2:スタート、3:ゴール、4:ゴール候補
    private Vector2 startCoordinate;//スタート位置
    private List<Vector2> potentialGoals;//ゴール候補のリスト

    public MapCoordinate(int x, int y, int goalMinDistance, int maxRoom)
    {
        coordinate = new int[x, y];
        potentialGoals = new List<Vector2>();
        startCoordinate = Vector2.zero;
        SetRoom(x, y, goalMinDistance, maxRoom);
    }

    private void SetRoom(int x, int y, int goalMinDistance, int maxRoom)
    {
        if (x + y - 3 < goalMinDistance)
        {
            Debug.Log("最小距離が足りません");
            return;
        }
        while (true)
        {

            startCoordinate = new Vector2(Random.Range(0, x), Random.Range(0, y));//スタート位置の座標

            for (int i = 0; i < x * y; i++)
            {
                int cx = i % x;//x座標
                int cy = i / x;//y座標

                if ((Mathf.Abs(cx - startCoordinate.x) + Mathf.Abs(cy - startCoordinate.y)) >= goalMinDistance)
                {
                    potentialGoals.Add(new Vector3(cx, cy));
                    coordinate[cx, cy] = 4;
                }
            }
            //ゴールがないときにもう一回スタートを探す
            if (potentialGoals.Count > 0)
            {
                coordinate[(int)startCoordinate.x, (int)startCoordinate.y] = 2;
                break;
            }
            Debug.Log("Repeat");
            potentialGoals.Clear();
        }
        //ランダムでゴールを決める
        Vector2 goalCoordinate = potentialGoals[Random.Range(0, potentialGoals.Count)];
        coordinate[(int)goalCoordinate.x, (int)goalCoordinate.y] = 3;

        Vector2 placeCoordinate = startCoordinate;//

        int count = 0;//無限ループを避ける

        //戦闘部屋を配置
        while (maxRoom > 0)
        {
            Vector2 direction = Vector2.zero;//次の部屋の座標
            bool moveX = false;//x方向への移動
            bool moveY = false;//y方向への移動

            if (placeCoordinate.x > goalCoordinate.x)
            {
                direction.x = -1;
                moveX = true;
            }
            else if (placeCoordinate.x < goalCoordinate.x)
            {
                direction.x = 1;
                moveX = true;
            }

            if (placeCoordinate.y > goalCoordinate.y)
            {
                direction.y = -1;
                moveY = true;
            }
            else if (placeCoordinate.y < goalCoordinate.y)
            {
                direction.y = 1;
                moveY = true;
            }

            if (moveX && moveY)//斜めに移動しないようにする
            {
                if (Mathf.RoundToInt(Random.value) == 1)
                {
                    direction.x = 0;
                }
                else
                {
                    direction.y = 0;
                }
            }

            placeCoordinate += direction;
            if (placeCoordinate == goalCoordinate)//ゴールについたらもう一回スタートから部屋を作る
            {
                placeCoordinate = startCoordinate;
                count++;
                if (count > 100)
                {
                    Debug.Log("Broke Goal");
                    break;
                }
                continue;
            }

            if (coordinate[(int)placeCoordinate.x, (int)placeCoordinate.y] != 1)//部屋じゃなかったら部屋にする
            {
                coordinate[(int)placeCoordinate.x, (int)placeCoordinate.y] = 1;
                maxRoom--;
                Debug.Log("MR:" + maxRoom);
                /*                if(maxRoom <= 0)
                                {
                                    Debug.Log("Broke Max Room");
                                    return;
                                }*/
            }
            //Debug.Log("PC:" + placeCoordinate + "GC:" + goalCoordinate);

            count++;//無限ループしないようにする
            if (count > 100)
            {
                Debug.Log("Broke Nothing");
                break;
            }
        }
    }
}