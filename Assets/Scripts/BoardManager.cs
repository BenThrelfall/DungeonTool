using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class BoardManager : NetworkBehaviour, IBoardManager, IRequiresDependancy {

    List<string> boards;
    int activeBoard = -1;
    
    string boardsFile;
    string boardsFolder;

    ISaveablesManager saveManager;

    public event Action<int> boardsUpdated;

    public override void OnStartServer() {
        base.OnStartServer();
        SetUpDependancies(DependancyInjector.instance.Services);
        LoadBoardsFile();
        RpcBoardsUpdated(boards.Count);
    }

    private void Awake() {
        boardsFile = Application.persistentDataPath + "/boards.txt";
        boardsFolder = Application.persistentDataPath;
    }

    #region IBoardManager

    public void SwitchToBoard(int boardIndex) {
        CmdSwitchToBoard(boardIndex);
    }


    public void CreateNewBoard() {
        CmdCreateNewBoard();
    }


    public void DeleteBoard(int boardIndex) {
        CmdDeleteBoard(boardIndex);
    }


    #endregion

    [Command(requiresAuthority = false)]
    private void CmdSwitchToBoard(int boardIndex) {
        SaveActiveBoard();
        LoadBoard(boardIndex);
    }

    [Command(requiresAuthority = false)]
    private void CmdCreateNewBoard() {
        for (int i = 0; ; i++) {
            string boardName = boardsFolder + $"/board{i}.json";
            if (File.Exists(boardName)) continue;

            File.Create(boardName).Close();
            boards.Add(boardName);

            break;
        }

        BoardsUpdated();

    }


    [Command(requiresAuthority = false)]
    private void CmdDeleteBoard(int boardIndex) {
        string boardFile = boards[boardIndex];
        File.Delete(boardFile);
        boards.RemoveAt(boardIndex);
        if (activeBoard > boardIndex) activeBoard--;
        BoardsUpdated();
    }
    private void BoardsUpdated() {
        File.WriteAllLines(boardsFile, boards);
        RpcBoardsUpdated(boards.Count);
    }

    [ClientRpc]
    void RpcBoardsUpdated(int boards) {
        boardsUpdated?.Invoke(boards);
    }

    [Server]
    private void LoadBoardsFile() {
        if (File.Exists(boardsFile)) {
            boards = new List<string>(File.ReadAllLines(boardsFile));
        }
        else {
            File.Create(boardsFile).Close();
            boards = new List<string>();
        }
    }

    [Server]
    void SaveActiveBoard() {

        if (activeBoard < 0) return;

        var saveables = saveManager.GetActiveSaveData();
        string boardFile = boards[activeBoard];

        SaveData[] saveCollection = saveables.ToArray();
        string json = JsonHelper.ToJson<SaveData>(saveCollection);
        File.WriteAllText(boardFile, json);
    }

    [Server]
    void LoadBoard(int boardIndex) {
        activeBoard = boardIndex;

        string boardFile = boards[boardIndex];
        string json = File.ReadAllText(boardFile);
        if (string.IsNullOrEmpty(json)) return;
        var savables = JsonHelper.FromJson<SaveData>(json);
        saveManager.LoadFromSaveData(savables);
    }

    public void SetUpDependancies(ServiceCollection serviceCollection) {
        saveManager = serviceCollection.GetService<ISaveablesManager>();
    }

    public void RequestBoardUpdate() {
        CmdRequestBoardUpdate();
    }

    [Command(requiresAuthority = false)]
    private void CmdRequestBoardUpdate() {
        RpcBoardsUpdated(boards.Count);
    }
}
