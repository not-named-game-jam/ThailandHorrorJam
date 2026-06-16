using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueRewards
{
    public enum RewardTypes{ Boolean, Integer }
    public RewardTypes rewardTypes;
    public string rewardName;
    public string rewardValue;
}
