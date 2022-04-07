using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardManager {

    event Action<int> boardsUpdated;

    void RequestBoardUpdate();
    void SwitchToBoard(int boardIndex);
    void CreateNewBoard();
    void DeleteBoard(int boardIndex);

}