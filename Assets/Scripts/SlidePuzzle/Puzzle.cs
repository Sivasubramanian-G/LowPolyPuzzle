using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int blocksPerLine = 4;
    public int shuffleLength = 20;

    public Camera cam = null;
    public Camera playerCam = null;

    public PlayerMovements playerMov = null;

    public float defaultDuration = 0.2f;
    public float shuffleDuration = 0.1f;

    public Texture2D image;

    BlockEvent emptyBlock;
    BlockEvent[,] blocks;
    Queue<BlockEvent> inputs;

    bool blockIsMoving;
    int shuffleMovesRemaining;

    Vector2Int prevShuffleOffset;

    public enum PuzzleState { Start, Solved, Shuffling, InPlay };
    public PuzzleState state;

    void Start()
    {
        CreatePuzzle();
    }

    void Update()
    {
        if (state == PuzzleState.Start)
        {
            StartShuffle();
        }
        if (state == PuzzleState.Solved)
        {
            cam.enabled = false;
            playerCam.enabled = true;
            playerMov.enabled = true;
            PuzzleObjs.doorOpen = true;
        }
    }

    void CreatePuzzle()
    {
        blocks = new BlockEvent[blocksPerLine, blocksPerLine];
        Texture2D[,] imageSlices = ImageSlicer.GetSlices(image, blocksPerLine);
        for (int i = 0; i < blocksPerLine; i++)
        {
            for (int j = 0; j < blocksPerLine; j++)
            {
                GameObject blockObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                blockObject.transform.position = -Vector2.one * (blocksPerLine - 1) * 0.5f + new Vector2(j, i) + new Vector2(transform.position.x, transform.position.y);
                blockObject.transform.position = new Vector3(blockObject.transform.position.x, blockObject.transform.position.y, transform.position.z - 0.1f);
                blockObject.transform.parent = transform;

                BlockEvent block = blockObject.AddComponent<BlockEvent>();
                block.OnBlockPressed += PlayerMouseBlockInput;
                block.OnFinishedMoving += OnBlockFinishedMoving;
                block.Init(new Vector2Int(j, i), imageSlices[j, i]);
                blocks[j, i] = block;

                if (i == 0 && j == blocksPerLine - 1)
                {
                    emptyBlock = block;
                }

            }
        }
        cam.orthographicSize = blocksPerLine * 0.55f;
        inputs = new Queue<BlockEvent>();
    }

    void PlayerMouseBlockInput(BlockEvent blockToMove)
    {
        if (state == PuzzleState.InPlay)
        {
            inputs.Enqueue(blockToMove);
            MakeNextPlayerMove();
        }
    }

    void MoveBlock(BlockEvent blockToMove, float duration)
    {
        if ((blockToMove.coord - emptyBlock.coord).sqrMagnitude == 1)
        {
            blocks[blockToMove.coord.x, blockToMove.coord.y] = emptyBlock;
            blocks[emptyBlock.coord.x, emptyBlock.coord.y] = blockToMove;

            Vector2Int targetCoord = emptyBlock.coord;
            emptyBlock.coord = blockToMove.coord;
            blockToMove.coord = targetCoord;

            Vector3 targetPosition = emptyBlock.transform.position;
            emptyBlock.transform.position = blockToMove.transform.position;
            blockToMove.MoveToPosition(targetPosition, duration);
            blockIsMoving = true;
        }
    }

    void MakeNextPlayerMove()
    {
        while (inputs.Count > 0 && !blockIsMoving)
        {
            MoveBlock(inputs.Dequeue(), defaultDuration);
        }
    }

    void OnBlockFinishedMoving()
    {
        blockIsMoving = false;
        CheckIfSolved();
        if (state == PuzzleState.InPlay)
        {
            MakeNextPlayerMove();
        }
        else if (state == PuzzleState.Shuffling)
        {
            if (shuffleMovesRemaining > 0)
            {
                MakeNextShuffleMove();
            }
            else
            {
                state = PuzzleState.InPlay;
            }
        }
    }

    void StartShuffle()
    {
        state = PuzzleState.Shuffling;
        shuffleMovesRemaining = shuffleLength;
        emptyBlock.gameObject.SetActive(false);
        MakeNextShuffleMove();
    }

    void MakeNextShuffleMove()
    {
        Vector2Int[] offsets = { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        int randomIndex = Random.Range(0, offsets.Length);

        for (int i = 0; i < offsets.Length; i++)
        {
            Vector2Int offset = offsets[(randomIndex + i) % offsets.Length];
            if (offset != prevShuffleOffset * -1)
            {
                Vector2Int moveBlockCoord = emptyBlock.coord + offset;
                if (moveBlockCoord.x >= 0 && moveBlockCoord.x < blocksPerLine && moveBlockCoord.y >= 0 && moveBlockCoord.y < blocksPerLine)
                {
                    MoveBlock(blocks[moveBlockCoord.x, moveBlockCoord.y], shuffleDuration);
                    shuffleMovesRemaining--;
                    prevShuffleOffset = offset;
                    break;
                }
            }
        }
    }

    void CheckIfSolved()
    {
        foreach (BlockEvent block in blocks)
        {
            if (!block.IsAtStartingCoord())
            {
                return;
            }
        }
        state = PuzzleState.Solved;
        emptyBlock.gameObject.SetActive(true);
    }
}