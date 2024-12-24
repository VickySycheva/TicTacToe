using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class GameService : MonoBehaviour
{
    [SerializeField] private PlayingField playingFieldPrefab;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private InputManager inputManager;
    
    bool isWin;
    PlayerType currentPlayer;
    PlayingField playingField;
    Cell[,] cells;

    List<Cell> matchingCells = new List<Cell> ();
    Cell startCell = null;
    int startInd = 0;

    void Start()
    {
        uiManager.Init(StartGame, RestartGame);
        inputManager.ActionOnCellClick += OnCellClick;
    }

    void StartGame()
    {
        uiManager.CloseStartScreen();
        currentPlayer = PlayerType.Cross;
        uiManager.UpdateGameText(currentPlayer);
        playingField = Instantiate(playingFieldPrefab);
        playingField.Init(out cells);

        inputManager.EnableInput(true);
        uiManager.ShowCloseGameScreen(true);
    }
    
    void RestartGame()
    {
        uiManager.CloseEndScreen();
        foreach (Cell cell in cells)
        {
            cell.ClearCell();
        }
        currentPlayer = PlayerType.Cross;
        uiManager.UpdateGameText(currentPlayer);
        isWin = false;
        inputManager.EnableInput(true);
        uiManager.ShowCloseGameScreen(true);
    }

    void OnCellClick(Cell cell)
    {
        if (!cell.CanPlace) return;
        cell.InstCellObj(currentPlayer);
        Check(currentPlayer);
        ChangePlayer();
    }

    void ChangePlayer()
    {
        currentPlayer = currentPlayer == PlayerType.Cross ? PlayerType.Naught : PlayerType.Cross;
        uiManager.UpdateGameText(currentPlayer);
    }

    void Check(PlayerType currentPlayer)
    {
        CheckHorizontal(currentPlayer);
        if(!isWin)
           CheckVertical(currentPlayer);
        if(!isWin)
           CheckDiagonal(currentPlayer);

        if(isWin || !CheckFreeCell())
        {
           EndGame(isWin, currentPlayer);
        }
    }

    void CheckHorizontal(PlayerType currentPlayer)
    {
        for (int i = 0; i < 3; i++)
        {
            matchingCells.Clear();
            startCell = null;

            for(int j = 0; j < 3; j++)
            {
                if (cells[i,j].CellObject != null && cells[i,j].CellObjectType == currentPlayer)
                {
                    if(startCell == null || MathF.Abs(startInd - j) == 1)
                    {
                        startCell = cells[i,j];
                        startInd = j;
                        matchingCells.Add(startCell);
                    }
                    else
                    {
                        matchingCells.Clear();
                        startCell = null;
                    }
                }
                if(matchingCells.Count == 3)
                {
                    isWin = true;
                    return;
                }
            }
        }
    }

    void CheckVertical(PlayerType currentPlayer)
    {
        for (int j = 0; j < 3; j++)
        {
            matchingCells.Clear();
            startCell = null;

            for(int i = 0; i < 3; i++)
            {
                if (cells[i,j].CellObject != null && cells[i,j].CellObjectType == currentPlayer)
                {
                    if(startCell == null || MathF.Abs(startInd - i) == 1)
                    {
                        startCell = cells[i,j];
                        startInd = i;
                        matchingCells.Add(startCell);
                    }
                    else
                    {
                        matchingCells.Clear();
                        startCell = null;
                    }
                }
                if(matchingCells.Count == 3)
                {
                    isWin = true;
                    return;
                }
            }
        }
    }
    
    void CheckDiagonal(PlayerType currentPlayer)
    {
        matchingCells.Clear();
        for (int i = 0; i < 3; i++)
        {
            if (cells[i,i].CellObject != null && cells[i,i].CellObjectType == currentPlayer)
            {
                matchingCells.Add(cells[i,i]);
            }
            if(matchingCells.Count == 3)
            {
                isWin = true;
                return;
            }
        }
        matchingCells.Clear();
        for (int i = 0; i < 3; i++)
        {
            if (cells[i,2-i].CellObject != null && cells[i,2-i].CellObjectType == currentPlayer)
            {
                matchingCells.Add(cells[i,2-i]);
            }
            if(matchingCells.Count == 3)
            {
                isWin = true;
                return;
            }
        } 
    }

    bool CheckFreeCell()
    {
        foreach (Cell cell in cells)
        {
            if (cell.CanPlace) return true;
        }
        return false;
    }

    void EndGame(bool isWin, PlayerType player)
    {
        inputManager.EnableInput(false);
        uiManager.ShowEndScreen(isWin, player);
        uiManager.ShowCloseGameScreen(false);
    }
}
