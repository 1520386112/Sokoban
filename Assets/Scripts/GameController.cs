using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapContainer;

/// <summary>
/// 游戏逻辑核心类
/// </summary>
public class GameController : MonoBehaviour
{
    //初始化地图时根据 prefab 生成物体
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject targetPrefab;
    public GameObject wallPrefab;

    public GameObject gameWinText;

    /// <summary>
    /// 记录游戏是否胜利，胜利后取消监测玩家输入
    /// </summary>
    private bool gameWin;

    // 记录 Player 当前在 snapshoot 数组中的下标
    private int playerIndX;
    private int playerIndY;

    private Player player;

    /// <summary>
    /// 存储某个位置对应的箱子，方便根据位置拿到该处箱子
    /// </summary>
    private Box[,] boxes;

    /// <summary>
    /// 目标点数目，用于判断游戏是否结束
    /// </summary>
    private int targetNum;
    /// <summary>
    /// 目前已经完成的目标点数目
    /// </summary>
    private int currentCompleteNum;

    private enum TileType { NULL = 0, WALL = 1, PLAYER = 2, BOX = 3, TARGET = 5, PLAYER_TARGET = 7, BOX_TARGET = 8}
    
    /// <summary>
    /// direction 数组四个方向对应的下标
    /// </summary>
    public enum Direction {DOWN, UP, LEFT, RIGHT}

    private int[,] direction = { {1, 0}, {-1, 0}, {0, -1},{0, 1} };

    private void Start()
    {
        boxes = new Box[9, 9];

        InitMap();
    }

    private void Update()
    {
        if (!gameWin)
        {
            DetectInput();
        }
    }

    void InitMap()
    {
        for(int i = 0; i < 9; ++i)
        {
            for(int j = 0; j < 9; ++j)
            {
                GameObject go = null;
                switch (snapshoot[i,j])
                {
                    case (int)TileType.WALL:
                        go = CreateGameObject(wallPrefab, i, j);
                        go.name = "Wall" + i + "_" + j;
                        break;
                    case (int)TileType.PLAYER:
                        go = CreateGameObject(playerPrefab, i, j);
                        go.name = "Player";
                        player = go.GetComponent<Player>();
                        playerIndX = i;
                        playerIndY = j;
                        break;
                    case (int)TileType.BOX:
                        go = CreateGameObject(boxPrefab, i, j);
                        go.name = "Block";
                        boxes[i, j] = go.GetComponent<Box>();
                        break;
                    case (int)TileType.TARGET:
                        go = CreateGameObject(targetPrefab, i, j);
                        go.name = "Target";
                        targetNum++;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 监测玩家输入
    /// </summary>
    private void DetectInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Direction.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Direction.UP);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Direction.RIGHT);
        }
    }

    private void Move(Direction dir)
    {
        //播放角色动画
        player.PlayPlayerAnimation(dir);

        int aimPosX = playerIndX + direction[(int)dir, 0];
        int aimPosY = playerIndY + direction[(int)dir, 1];
        
        switch (snapshoot[aimPosX, aimPosY])
        {
            //目标点为空或者是目标点，直接将玩家移动过去即可
            case (int)TileType.NULL:
            case (int)TileType.TARGET:
                MoveTile(playerIndX, playerIndY, aimPosX, aimPosY, TileType.PLAYER);
                break;
            
            //目标点是箱子
            case (int)TileType.BOX:
            case (int)TileType.BOX_TARGET:
                int nextNextX = aimPosX + direction[(int)dir, 0];
                int nextNextY = aimPosY + direction[(int)dir, 1];             

                switch (snapshoot[nextNextX, nextNextY])
                {
                    //目标方向的下下块没有阻挡物时，方可移动
                    case (int)TileType.NULL:
                    case (int)TileType.TARGET:
                        MoveTile(playerIndX, playerIndY, aimPosX, aimPosY, TileType.PLAYER);
                        MoveTile(aimPosX, aimPosY, nextNextX, nextNextY, TileType.BOX);
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// 移动某个Tile，更新地图快照以及物体位置
    /// <param name="tileType">需要移动的块类型</param>
    /// </summary>
    private void MoveTile(int originX, int originY, int aimX, int aimY, TileType tileType)
    {
        //在目标点的箱子被推动，当前完成数减一
        if(snapshoot[originX, originY] == (int)TileType.BOX_TARGET)
        {
            currentCompleteNum--;
        }

        /* 直接将目标点处的信息数字加上移动的块对应的数即可
         * TileType 枚举的特殊数字就是为了如此操作而设计的
         */
        snapshoot[aimX, aimY] += (int)tileType;
        snapshoot[originX, originY] -= (int)tileType;

        //推动一个箱子到目标点，当前完成数加一
        if(snapshoot[aimX, aimY] == (int)TileType.BOX_TARGET)
        {
            currentCompleteNum++;
            if(currentCompleteNum == targetNum)
            {
                GameWin();
            }
        }

        switch (tileType)
        {
            case TileType.PLAYER:
            case TileType.PLAYER_TARGET:
                player.transform.position = new Vector2(aimY, -aimX);
                playerIndX = aimX;
                playerIndY = aimY;
                break;

            case TileType.BOX:
            case TileType.BOX_TARGET:
                boxes[aimX, aimY] = boxes[originX, originY];
                boxes[originX, originY] = null;

                boxes[aimX, aimY].transform.position = new Vector2(aimY, -aimX);

                //移动后根据箱子是否在目标点上修改箱子的Sprite
                if(snapshoot[aimX, aimY] == (int)TileType.BOX_TARGET)
                    boxes[aimX, aimY].SetSpriteToOnTargetSprite();
                else
                    boxes[aimX, aimY].SetSpriteToNormal();

                break;
        }
    }

    GameObject CreateGameObject(GameObject go, int row, int col)
    {
        return Instantiate(go, new Vector3(col, -row), Quaternion.identity);
    }

    private void GameWin()
    {
        gameWin = true;
        gameWinText.SetActive(true);
        Debug.Log("游戏胜利");
    }
}
