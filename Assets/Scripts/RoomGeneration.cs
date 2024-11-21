using UnityEngine;
using System.Collections.Generic;

//5x5�̃O���b�h�����A���S�ȊO�̃}�X�ɃX�^�[�g��u���B(goalDistance)�}�X�ȏ�ړ������Ƃ���ɃS�[����u��
//�X�^�[�g�̈ʒu����
public class RoomGeneration : MonoBehaviour
{
    public Material roomMaterial;//�퓬�����̃}�e���A��
    public Material startMaterial;//�X�^�[�g�����̃}�e���A��
    public Material goalMaterial;//�S�[�������̃}�e���A��
    public Material potentialGoalMaterial;//�S�[���������̃}�e���A��
    public int x;//�����̐��i���j
    public int y;//�����̐��i�c�j
    public int goalMinDistance = 5;//�S�[���܂ł̍ŒZ����
    public int maxRooms;//�퓬�����̍ő吔
    public bool generateOnStart = false;//OnStart()�ŕ����𐶐�����

    private List<GameObject> cubes = new List<GameObject>();//�����̋��

    public GameObject playerPrefab;//�v���C���[�̃v���n�u
    private GameObject playerInstance;//�C���X�^���X�������v���C���[

    public GameObject[] room;//�퓬�����̌��
    public GameObject wall;//��������ǂ̃v���n�u
    public GameObject door;//��������h�A�̃v���n�u
    private List<GameObject> walls = new List<GameObject>();//�������ꂽ�ǉ��h�A
    public Transform roomParent;//�����𐶐�����Ƃ��̐e�I�u�W�F�N�g

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

    //�����𐶐�����
    public void Generate()
    {
        //�O�������ꂽ���̂�����
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

        //���H�𐶐�����
        MapCoordinate mapCoordinate = new MapCoordinate(x, y, goalMinDistance, maxRooms);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                //���ꂽ���H�̒ʂ�ɕ����𐶐�����
                GameObject room_ = Instantiate(room[Random.Range(0, room.Length)], roomParent);
                room_.transform.position = new Vector3(i * 50, 0, j * 50);
                switch (mapCoordinate.coordinate[i, j])
                {
                    case 0://�����Ȃ��ꍇ
                        Destroy(room_);
                        break;
                    case 1://�퓬�����̏ꍇ
                        cubes.Add(room_);
                        room_.GetComponent<MeshRenderer>().material = roomMaterial;
                        SetWall(i, j, mapCoordinate);
                        break;
                    case 2://�X�^�[�g�����̏ꍇ
                        cubes.Add(room_);
                        room_.GetComponent<MeshRenderer>().material = startMaterial;
                        SetWall(i, j, mapCoordinate);
                        if (playerInstance != null)
                        {
                            Destroy(playerInstance);
                        }
                        playerInstance = Instantiate(playerPrefab, room_.transform.position, Quaternion.identity);
                        break;
                    case 3://�S�[�������̏ꍇ
                        cubes.Add(room_);
                        room_.GetComponent<MeshRenderer>().material = goalMaterial;
                        SetWall(i, j, mapCoordinate);
                        break;
                    case 4://�S�[�����̏ꍇ
                        Destroy(room_);
                        break;
                }
            }
        }
    }

    public void SetWall(int x, int y, MapCoordinate mapCoordinate)
    {
        //�E
        if (x != this.x - 1)//coordinate���͈͂𒴂��Ȃ��悤�ɂ���
        {
            //�ׂɉ����Ȃ����S�[����₾������ǂ�u���A����ȊO��������ǂ�u��
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
        else//�[��������ǂ�u��
        {
            GameObject wall_ = Instantiate(wall, new Vector3(x * 50 + 25, 0, y * 50), Quaternion.Euler(0, 90, 0), roomParent);
            wall_.name = $"wall_{x}_{y}_Rignt";
            walls.Add(wall_);
        }
        //�ȉ��قړ���
        //��
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
        //�O
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
        //��
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
    public int[,] coordinate;//0:�����Ȃ��A1:�퓬�����A2:�X�^�[�g�A3:�S�[���A4:�S�[�����
    private Vector2 startCoordinate;//�X�^�[�g�ʒu
    private List<Vector2> potentialGoals;//�S�[�����̃��X�g

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
            Debug.Log("�ŏ�����������܂���");
            return;
        }
        while (true)
        {

            startCoordinate = new Vector2(Random.Range(0, x), Random.Range(0, y));//�X�^�[�g�ʒu�̍��W

            for (int i = 0; i < x * y; i++)
            {
                int cx = i % x;//x���W
                int cy = i / x;//y���W

                if ((Mathf.Abs(cx - startCoordinate.x) + Mathf.Abs(cy - startCoordinate.y)) >= goalMinDistance)
                {
                    potentialGoals.Add(new Vector3(cx, cy));
                    coordinate[cx, cy] = 4;
                }
            }
            //�S�[�����Ȃ��Ƃ��ɂ������X�^�[�g��T��
            if (potentialGoals.Count > 0)
            {
                coordinate[(int)startCoordinate.x, (int)startCoordinate.y] = 2;
                break;
            }
            Debug.Log("Repeat");
            potentialGoals.Clear();
        }
        //�����_���ŃS�[�������߂�
        Vector2 goalCoordinate = potentialGoals[Random.Range(0, potentialGoals.Count)];
        coordinate[(int)goalCoordinate.x, (int)goalCoordinate.y] = 3;

        Vector2 placeCoordinate = startCoordinate;//

        int count = 0;//�������[�v�������

        //�퓬������z�u
        while (maxRoom > 0)
        {
            Vector2 direction = Vector2.zero;//���̕����̍��W
            bool moveX = false;//x�����ւ̈ړ�
            bool moveY = false;//y�����ւ̈ړ�

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

            if (moveX && moveY)//�΂߂Ɉړ����Ȃ��悤�ɂ���
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
            if (placeCoordinate == goalCoordinate)//�S�[���ɂ�����������X�^�[�g���畔�������
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

            if (coordinate[(int)placeCoordinate.x, (int)placeCoordinate.y] != 1)//��������Ȃ������畔���ɂ���
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

            count++;//�������[�v���Ȃ��悤�ɂ���
            if (count > 100)
            {
                Debug.Log("Broke Nothing");
                break;
            }
        }
    }
}